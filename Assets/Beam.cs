using UnityEngine;

public class Beam : MonoBehaviour
{
    [Tooltip("Tiempo en segundos antes de destruirse")]
    public float duration = 0.5f;

    void Start()
    {
        Destroy(gameObject, duration);
    }
}
