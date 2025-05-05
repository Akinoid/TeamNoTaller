using UnityEngine;

public class ExplotionDuration : MonoBehaviour
{
    [SerializeField] private bool timeOfExplosion;
    [SerializeField] private float timer, maxTimer;


    void Start()
    {
        timeOfExplosion = true;
        timer = 0;
    }

    void Update()
    {

        if (timeOfExplosion)
        {
            TimeExplosion();
        }
    }


    private void TimeExplosion()
    {
        timer += Time.deltaTime;
        if (timer >= maxTimer)
        {
            Destroy(gameObject);
            timeOfExplosion = false;
        }
    }
}
