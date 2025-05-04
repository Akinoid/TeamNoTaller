using UnityEngine;

public class Invisible : MonoBehaviour
{
    [SerializeField] private GameObject centerOfGame;
    [SerializeField] private PlayerLife playerLife;
    [SerializeField] public bool isInvisible;
    [SerializeField] private float timerInvisible;
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
        if(timerInvisible >= 10f)
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
            Destroy(other.gameObject);
        }
    }
}
