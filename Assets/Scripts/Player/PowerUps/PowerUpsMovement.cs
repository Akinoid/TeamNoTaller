using UnityEngine;

public class PowerUpsMovement : MonoBehaviour
{
    [SerializeField] Vector3 direction;
    private Rigidbody rb;
    [SerializeField] private float speed = 2;
    [SerializeField] private float timer;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
        TimerLife();
    }

    private void Move()
    {
        direction = new Vector3(0, 0, 1f);
        rb.AddRelativeForce(new Vector3(direction.x, direction.y, direction.z * speed), ForceMode.Force);
    }
    private void TimerLife()
    {
        timer += Time.deltaTime;
        if (timer >= 7f)
        {
            Invoke(nameof(AutoDestroy), 0.05f);
        }
    }
    private void AutoDestroy()
    {
        Destroy(gameObject);

    }
}
