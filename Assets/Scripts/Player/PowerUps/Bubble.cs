using UnityEngine;

public class Bubble : MonoBehaviour
{
    private PlayerLife playerLife;
    [SerializeField] public float timerBase, timerCritic, timerHit, timerGetHit;
    [SerializeField] public bool canDied, unhit, startTimerHit;
    [SerializeField] public bool getHitBubble, haveElectricBuff;
    [SerializeField] public State state;
    public enum State
    {
        Base,
        Critic,
        Electric,
    }
    void Start()
    {
        state = State.Base;
        getHitBubble = false;
        playerLife = gameObject.GetComponent<PlayerLife>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInjured();
        if (getHitBubble || state == State.Critic || state == State.Electric)
        {
            Timer();
        }
    }

    private void GetInjured()
    {
        switch (state)
        {
            case State.Base:
                timerCritic = 0;
                timerHit = 0;
                timerGetHit = 0;
                unhit = false;
                canDied = false;
                startTimerHit = true;
                if (getHitBubble && !haveElectricBuff)
                {
                    state = State.Critic;
                    timerBase = 0;
                }
                if(getHitBubble && haveElectricBuff)
                {
                    state = State.Electric;
                }
                break;
            case State.Critic:
                if (timerHit >= 0.1f)
                {
                    unhit = true;
                }
                if (unhit)
                {
                    getHitBubble = false;
                    timerHit = 0;
                    startTimerHit = false;
                    unhit = false;
                }
                if (getHitBubble && timerBase >= 1 && timerGetHit >= 1)
                {
                    timerCritic += 3f;
                    timerGetHit = 0;
                    startTimerHit = true;
                }
                if (timerCritic >= 10)
                {
                    timerBase = 0;
                    playerLife.haveBubble = false;
                    state = State.Base;
                }
                break;
            case State.Electric:
                if (timerCritic >= 15)
                {
                    playerLife.haveBubble = false;
                    state = State.Base;
                }
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (playerLife.canGetHit)
            {
                getHitBubble = true;
            }
        }
    }

    private void Timer()
    {
        if (state == State.Base || state == State.Critic)
        {
            timerBase += Time.deltaTime;

        }
        if (state == State.Critic)
        {
            timerCritic += Time.deltaTime;
            timerGetHit += Time.deltaTime;
            if (startTimerHit)
            {
                timerHit += Time.deltaTime;
            }

        }
        if(state == State.Electric)
        {
            timerCritic += Time.deltaTime;
        }
    }
}
