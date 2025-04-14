using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;
    public float moveRange = 5f;

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 newPosition = transform.position + new Vector3(horizontal, vertical, 0) * speed * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, -moveRange, moveRange);
        newPosition.y = Mathf.Clamp(newPosition.y, -moveRange, moveRange);

        transform.position = newPosition;
    }
}
