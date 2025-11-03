using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    [Header("Input Actions")]
    public InputActionReference moveAction;
    public InputActionReference rotateAction;

    [Header("Action")]
    public int speed;
    public int rotationSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void OnEnable()
    {
        moveAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
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
    void Move(InputAction.CallbackContext _ctx)
    {
        
    }

    void Look(InputAction.CallbackContext _ctx)
    {
        
    }
}
