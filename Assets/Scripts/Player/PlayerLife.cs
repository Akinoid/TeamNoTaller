using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] private float timer, timer2;
    [SerializeField] private bool getHit;
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
                timer2 = 0;
                if (getHit && timer >= 1)
                {
                    state = State.Critic;
                    getHit = false;
                }
                break;
            case State.Critic:
                meshRenderer.sharedMaterial = materialRed;
                timer = 0;
                if (getHit)
                {
                    Debug.Log("Game Over");
                    getHit = false;
                }
                if (timer2 >= 1 && !getHit)
                {
                    Debug.Log("Empieza a curarse");
                    meshRenderer.sharedMaterial = materialGreen;
                }
                if(timer2 >= 3 && !getHit)
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
            getHit = true;
        }
    }
    private void Timer()
    {
        if(state == State.Base)
        {
            timer += Time.deltaTime;

        }
        if(state == State.Critic)
        {
            timer2 += Time.deltaTime;

        }

    }
}
