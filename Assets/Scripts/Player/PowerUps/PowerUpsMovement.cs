using UnityEngine;

public class PowerUpsMovement : MonoBehaviour
{
    [SerializeField] Vector3 direction;
    private Rigidbody rb;
    [SerializeField] private float speed = 3;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        direction = new Vector3(0, 0, 1f);
        rb.AddRelativeForce(new Vector3(direction.x, direction.y, direction.z * speed), ForceMode.Force);
    }
}
