using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerActions : MonoBehaviour
{
    [Header("Input Variables")]
    [SerializeField] private InputActionMap playerInputMap;
    [SerializeField] private InputActionAsset playerInputAsset;
    [Header("Input References")]
    [SerializeField] private InputActionReference moveX;
    [SerializeField] private InputActionReference moveY;
    private InputAction moveXAction, moveYAction;
    [Header("Input Values")]
    [SerializeField] public float moveXfloat;
    [SerializeField] public float moveYfloat;

    private Rigidbody rb;
    [Header("Movement Variables")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxSpeed;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        playerInputMap = playerInputAsset.FindActionMap("Player");
        moveXAction = moveX.ToInputAction();
        moveYAction = moveY.ToInputAction();
    }

    void Update()
    {
        GetInput();
        Movement();
    }
    private void Movement()
    {
        rb.AddRelativeForce(new Vector3(moveXfloat * moveSpeed, moveYfloat * moveSpeed, 0), ForceMode.Impulse);
        //rb.AddForceAtPosition(new Vector3(moveXfloat * moveSpeed, moveYfloat * moveSpeed, 0) ,
            //new Vector3(transform.position.x, transform.position.y, transform.position.z), ForceMode.Impulse);
        //rb.angularVelocity = new Vector3(moveXfloat * moveSpeed, moveYfloat * moveSpeed, 0);
        //rb.linearVelocity = new Vector3(moveXfloat * moveSpeed, moveYfloat * moveSpeed, 0);
        /*if(rb.angularVelocity.magnitude >= maxSpeed)
        {
            rb.angularVelocity = Vector3.ClampMagnitude(rb.angularVelocity, maxSpeed);
        }*/
        if(rb.linearVelocity.magnitude >= maxSpeed)
        {
            rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxSpeed);
        }
    }
    private void GetInput()
    {
        moveXfloat = moveXAction.ReadValue<float>();
        moveYfloat = moveYAction.ReadValue<float>();
    }
    private void OnEnable()
    {
        moveXAction = moveX.ToInputAction();
        moveXAction.Enable();

        moveYAction = moveY.ToInputAction();
        moveYAction.Enable();
    }

    private void OnDisable()
    {
        moveXAction.Disable();
        moveYAction.Disable();
    }
}
