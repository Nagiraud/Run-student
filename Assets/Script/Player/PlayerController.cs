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
    public InputActionReference copyAction;
    public InputActionReference sitAction;

    [Header("Action")]
    public int speed;
    public int rotationSpeed;

    private GameObject TableTriggered;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        copyAction.action.performed+= CopyStudent;
        sitAction.action.performed += SitPlayer;
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        copyAction.action.performed-= CopyStudent;
        sitAction.action.performed -= SitPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 Direction = moveAction.action.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(Direction.x, 0, Direction.y);

        // Si le personnage se déplace
        if (moveDirection.magnitude > 0.1f)
        {
            // Calculer la rotation cible vers la direction de déplacement
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection.normalized, Vector3.up);

            // Appliquer la rotation progressivement
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 25 * Time.deltaTime);
        }

        // Déplacer le personnage
        GetComponent<CharacterController>().SimpleMove(moveDirection * speed);
 
    }


    // copier
    void CopyStudent(InputAction.CallbackContext _ctx) {
        if(TableTriggered)
            TableTriggered.GetComponent<TableCopy>().StartCopy();
    }

    private void OnTriggerEnter(Collider other)
    {
        TableTriggered = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        other.GetComponent<TableCopy>().StopCopy();
        TableTriggered = null;
    }

    // se faire attraper

    public void GetCaught()
    {
        if (TableTriggered) {  
            TableTriggered.GetComponent<TableCopy>().StopCopy();
        }
        SceneManager.LoadScene("EndingScene");
    }

    public void SitPlayer(InputAction.CallbackContext _ctx)
    {
        if (tag == "Sitting")
            tag = "Player";
        else
            tag = "Sitting";
    }
    
}
