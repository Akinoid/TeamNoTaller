using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] private float timerBase, timerCritic;
    [SerializeField] private bool getHit;
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
                if (getHit && timerBase >= 1)
                {
                    state = State.Critic;
                    getHit = false;
                }
                break;
            case State.Critic:
                meshRenderer.sharedMaterial = materialRed;
                timerBase = 0;
                if (getHit)
                {
                    Debug.Log("Game Over");
                    getHit = false;
                }
                if (timerCritic >= 1 && !getHit)
                {
                    Debug.Log("Empieza a curarse");
                    meshRenderer.sharedMaterial = materialGreen;
                }
                if(timerCritic >= 3 && !getHit)
                {
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
        if(state == State.Base)
        {
            timerBase += Time.deltaTime;

        }
        if(state == State.Critic)
        {
            timerCritic += Time.deltaTime;

        }

    }
}
