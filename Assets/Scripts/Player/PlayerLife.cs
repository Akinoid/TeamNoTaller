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
    [SerializeField] public Money money;
    [SerializeField] private LoadManager load;
    [SerializeField] public State state;
    bool tutorialLife, tutorialLife2, tutorialDead;
    public GameObject dialogues1, dialogues2;
    [SerializeField] private LifeDialogueTutorial lifeDialogue;
    [SerializeField] private DialoguesTutorial dialogues;
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
        money = GameObject.Find("MoneyManager").GetComponent<Money>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(StartMenuManager.tutorial);
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
                if (StartMenuManager.tutorial == 0 && !tutorialLife2 && DialoguesController.tutorial == 10)
                {
                    lifeDialogue.actualLines = dialogues.linesNoHit;
                    dialogues1.SetActive(false);
                    dialogues2.SetActive(true);
                    tutorialLife2 = true;
                }
                if (getHit && !haveBubble)
                {
                    Money.startRest = true;
                    state = State.Critic;
                    timerBase = 0;
                }
                break;
            case State.Critic:
                if(StartMenuManager.tutorial == 0 && !tutorialLife)
                {
                    lifeDialogue.actualLines = dialogues.linesCriticState;
                    dialogues1.SetActive(false);
                    dialogues2.SetActive(true);
                    tutorialLife = true;
                }
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
                    if(StartMenuManager.tutorial == 1)
                    {
                        money.money += Money.score;
                        Money.score = 0;
                        load.Save();
                        Money.combo.Clear();
                        SceneManager.LoadScene("StartMenu");
                        Debug.Log("Game Over");
                        getHit = false;
                    }
                    else if(StartMenuManager.tutorial == 0 && !tutorialDead) 
                    {
                        dialogues1.SetActive(false);
                        dialogues2.SetActive(true);
                        lifeDialogue.StopAllCoroutines();
                        lifeDialogue.actualLines = dialogues.linesDeath;
                        lifeDialogue.StartDialogue();
                        getHit = false;
                        tutorialDead = true;
                    }
                }
                if (timerCritic >= 1)
                {
                    //Debug.Log("Empieza a curarse");
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
            Money.score += 150 * Money.multiplier;
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
