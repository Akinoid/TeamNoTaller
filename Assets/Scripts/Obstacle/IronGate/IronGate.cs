using UnityEngine;

public class IronGate : MonoBehaviour
{
    public Transform leftGate;   
    public Transform rightGate;  
    public Vector3 leftTargetOffset;  
    public Vector3 rightTargetOffset; 
    public float closeSpeed = 5f;
    public GameObject leftWarningIcon;
    public GameObject rightWarningIcon;

    [SerializeField]private bool closing = false;
    private Vector3 leftStartPos;
    private Vector3 rightStartPos;
    private Vector3 leftTargetPos;
    private Vector3 rightTargetPos;

    private void Start()
    {
        leftStartPos = leftGate.position;
        rightStartPos = rightGate.position;
        leftTargetPos = leftStartPos + leftTargetOffset;
        rightTargetPos = rightStartPos + rightTargetOffset;

        leftWarningIcon.SetActive(false);
        rightWarningIcon.SetActive(false);
    }

    private void Update()
    {
        if (closing)
        {
            leftGate.position = Vector3.MoveTowards(leftGate.position, leftTargetPos, closeSpeed * Time.deltaTime);
            rightGate.position = Vector3.MoveTowards(rightGate.position, rightTargetPos, closeSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !closing)
        {
            leftWarningIcon.SetActive(true);
            rightWarningIcon.SetActive(true);

            Invoke("StartClosing", 1.5f); 
        }
    }

    private void StartClosing()
    {
        leftWarningIcon.SetActive(false);
        rightWarningIcon.SetActive(false);
        closing = true;
    }
}
