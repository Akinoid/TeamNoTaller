using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] public float timerBase, timerCritic, timerHit;
    [SerializeField] public bool getHit, canDied, unhit, startTimerHit;
    [SerializeField] public bool canGetHit, haveBubble;
    [SerializeField] private Renderer meshRenderer;
    [SerializeField] public Material materialGreen;
    [SerializeField] public Material materialRed;
    [SerializeField] public Material InitialMaterial;
    [SerializeField] public State state;
    public enum State
    {
        Base,
        Critic,
    }
    void Start()
    {
        state = State.Base;
        getHit = false;
        meshRenderer = gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInjured();
        if(getHit && !haveBubble || state == State.Critic && !haveBubble)
        {
            Timer();
        }
    }

    private void GetInjured()
    {
        switch (state)
        {
            case State.Base:
                meshRenderer.sharedMaterial = InitialMaterial;
                timerCritic = 0;
                timerHit = 0;
                canDied = false;
                unhit = false;
                startTimerHit = true;
                if (getHit && !haveBubble)
                {
                    Money.startRest = true;
                    state = State.Critic;
                    timerBase = 0;
                }
                break;
            case State.Critic:
                if (timerHit >= 0.2f)
                {
                    Money.startRest = false;
                    unhit = true;
                }
                if (unhit)
                {
                    getHit = false;
                    timerHit = 0;
                    startTimerHit = false;
                    unhit = false;
                }
                if(getHit && timerBase >= 1)
                {
                    canDied = true;
                }
                else
                {
                    canDied = false;
                }

                if (canDied && getHit && !haveBubble)
                {
                    SceneManager.LoadScene("Player");
                    Debug.Log("Game Over");
                    getHit = false;
                }
                if (timerCritic >= 1)
                {
                    Debug.Log("Empieza a curarse");
                }
                if(timerCritic >= 3)
                {
                    timerBase = 0;
                    state = State.Base;
                }
                if (haveBubble)
                {
                    timerBase = 0;
                    state = State.Base;
                }
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (canGetHit)
            {
                getHit = true;
            }
        }
        if (other.CompareTag("Bubble"))
        {
            haveBubble = true;
            Destroy(other.gameObject);
        }
    }
    
    private void Timer()
    {
        if(state == State.Base || state == State.Critic)
        {
            timerBase += Time.deltaTime;

        }
        if(state == State.Critic)
        {
            timerCritic += Time.deltaTime;
            if (startTimerHit)
            {
                timerHit += Time.deltaTime;
            }

        }

    }
}
