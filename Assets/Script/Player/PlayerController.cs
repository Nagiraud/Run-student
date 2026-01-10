using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Windows;

// Gére les mouvement du joueur, et les interraction possible
public class PlayerController : MonoBehaviour
{
    [Header("Input Actions")]
    public InputActionReference moveAction;
    public InputActionReference InterractAction;
    public InputActionReference sitAction;

    [Header("Action")]
    public int speed;
    public int rotationSpeed;

    // GameObject rencontré
    private GameObject TableTriggered;
    private GameObject SeatTrigger;
    private GameObject HidingPlaceTriggered;

    //caméra
    public CameraManager cam;

    // Animator
    private Animator animator;

    private void OnEnable()
    {
        moveAction.action.Enable();
        InterractAction.action.performed+= InterractStudent;
        sitAction.action.performed += SitPlayer;
        animator= GetComponent<Animator>();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        InterractAction.action.performed-= InterractStudent;
        sitAction.action.performed -= SitPlayer;
    }

    void Update()
    {
        Vector2 Direction = moveAction.action.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(Direction.x, 0, Direction.y);

        // Animation
        if(moveDirection == Vector3.zero)
        {
            animator.SetBool("Speed", false);
        }
        else
        {
            animator.SetBool("Speed", true);
        }

        // Si le personnage se déplace
        if (moveDirection.magnitude > 0.1f)
        {
            
            Camera MainCamera = cam.GetCurrentCamera();
            // En fonction de la caméra
            Vector3 cameraForward = MainCamera.transform.forward;
            Vector3 cameraRight = MainCamera.transform.right;
            cameraForward.y = 0;
            cameraRight.y = 0;

            // Calculer la direction finale
            moveDirection = cameraForward * Direction.y + cameraRight * Direction.x;

            // Calculer la rotation cible vers la direction de déplacement
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection.normalized, Vector3.up);

            // Appliquer la rotation progressivement
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 25 * Time.deltaTime);
        }

        // Déplacer le personnage
        if(tag=="Player")
            GetComponent<CharacterController>().SimpleMove(moveDirection * speed);

    }


    // Interraction (touche E)
    void InterractStudent(InputAction.CallbackContext _ctx) {
        if(TableTriggered)
            TableTriggered.GetComponent<CheatArea>().StartCopy();
        if (HidingPlaceTriggered)
        {
            tag = tag == "Player" ? "Hide" : "Player";
            // Récupére tout les enfant
            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = !renderer.enabled;
            }
        }
    }

    // Vérification des triggers
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "PlayerSeat":
                SeatTrigger = other.gameObject;
                break;
            case "HidingPlace":
                Debug.Log("entré placard");
                HidingPlaceTriggered = other.gameObject;
                break;
            default:
                TableTriggered = other.gameObject;
                break;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "PlayerSeat":
                SeatTrigger = null;
                break;
            case "HidingPlace":
                HidingPlaceTriggered = null;
                break;

            default:
                other.GetComponent<CheatArea>().StopCopy();

                TableTriggered = null;
                break;
        }
        
    }

    // Se faire attraper

    public void GetCaught()
    {
        if (TableTriggered) {  
            TableTriggered.GetComponent<CheatArea>().StopCopy();
        }
        SceneManager.LoadScene("GameOverScene");
    }

    // Le joeur s'assoit à sa place, et regarde si il a gagné
    public void SitPlayer(InputAction.CallbackContext _ctx)
    {
        if (SeatTrigger)
        {
            if (tag == "Sitting")
            {
                tag = "Player";
                animator.SetBool("IsSitting", false);
            }
                
            else
            {
                tag = "Sitting";
                animator.SetBool("IsSitting", true);
                transform.position=SeatTrigger.transform.position+new Vector3(0,0,0.2f);
                if (ScoreManager.Instance.GetScore()==3)
                {
                    ScoreManager.Instance.StopTimer();
                    SceneManager.LoadScene("WinningScene");
                }
            }
            
        }
        
    }
    
}
