using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] public float timerBase, timerCritic, timerHit;
    [SerializeField] public bool getHit, canDied, unhit;
    [SerializeField] public bool canGetHit;
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
        if(getHit || state == State.Critic)
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
                if (getHit)
                {
                    state = State.Critic;
                    timerBase = 0;
                }
                break;
            case State.Critic:
                if (timerHit >= 0.5f)
                {
                    unhit = true;
                }
                if (unhit)
                {
                    getHit = false;
                    timerHit = 0;
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

                if (canDied && getHit)
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
            timerHit += Time.deltaTime;
        }

    }
}
