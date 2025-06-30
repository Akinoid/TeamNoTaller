using UnityEngine;

public class ExplotionDuration : MonoBehaviour
{
    [SerializeField] private bool timeOfExplosion;
    [SerializeField] private float timer, maxTimer;
    [SerializeField] private GameObject colli;

    void Start()
    {
        
        timeOfExplosion = true;
        timer = 0;
        colli.SetActive(false);
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
        if(timer >= 1.5f)
        {
            colli.SetActive(true);
        }
        if (timer >= maxTimer)
        {
            Destroy(gameObject);
            timeOfExplosion = false;
        }
    }
}
