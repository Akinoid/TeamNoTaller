using UnityEngine;

public class Invisible : MonoBehaviour
{
    private PlayerLife playerLife;
    [SerializeField] private GameObject centerOfGame;
    [SerializeField] public bool isInvisible;
    [SerializeField] private float timerInvisible, maxTimerInvisible;
    void Start()
    {
        isInvisible = false;
        centerOfGame = GameObject.Find("CenterOfGame");
        playerLife = gameObject.GetComponent<PlayerLife>();
    }

    void Update()
    {
        if (isInvisible)
        {
            InvisibleState();
            TimeInvisible();
        }
        else
        {
            NormalState();
        }
    }

    private void InvisibleState()
    {
        centerOfGame.tag = "Player";
        gameObject.tag = "InvisibleState";
    }

    private void NormalState()
    {
        timerInvisible = 0;
        centerOfGame.tag = "InvisibleState";
        gameObject.tag = "Player";
    }

    private void TimeInvisible()
    {
        timerInvisible += Time.deltaTime;
        if(timerInvisible >= maxTimerInvisible)
        {
            isInvisible = false;
        }

        if (playerLife.getHit)
        {
            isInvisible = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Invisible"))
        {
            timerInvisible = 0;
            isInvisible = true;
            Money.score += 150 * Money.multiplier;
            Destroy(other.gameObject);
        }
    }
}
