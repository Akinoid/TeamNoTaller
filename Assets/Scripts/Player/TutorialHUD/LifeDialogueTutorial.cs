using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LifeDialogueTutorial : MonoBehaviour
{
    public TMP_Text dialogueText;
    public DialoguesController dialoguesController;
    [TextArea(4, 6)] public string[] actualLines;
    public GameObject dialogues1;
    public float textSpeed = 0.02f;

    public int index;

    public bool didDialogueStart;
    public static bool tutorial;
    public static bool startTimer, startChangeLines;
    public float timer;
    private void Awake()
    {
        tutorial = true;
    }
    public void Update()
    {
        if (!didDialogueStart)
        {
            StartDialogue();
        }
        else if (dialogueText.text == actualLines[index])
        {
            NextDialogueLine();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            StopAllCoroutines();
            dialogueText.text = actualLines[index];

        }

        if (!tutorial && startTimer)
        {
            ChangeLines();
        }
        if (!tutorial && startChangeLines)
        {
            ChangeLinesWithOutTimer();
        }
        if (tutorial)
        {
            startTimer = false;
            startChangeLines = false;
        }
    }

    public void StartDialogue()
    {
        didDialogueStart = true;

        StartCoroutine(WriteLine());
    }
    public void NextDialogueLine()
    {
        index++;
        if (index < actualLines.Length)
        {
            StartCoroutine(WriteLine());
        }

        if (index >= actualLines.Length)
        {
            index = 0;
            StopAllCoroutines();
            tutorial = false;
            startTimer = true;
            Time.timeScale = 1f;
        }
    }

    void ChangeLines()
    {
        timer += Time.deltaTime;
        if(timer >= 3)
        {
            dialogues1.SetActive(true);
            gameObject.SetActive(false);
            tutorial = true;
            timer = 0;
        }
    }

    void ChangeLinesWithOutTimer()
    {
        Invoke("StartDialogue", 0.02f);
        tutorial = true;        
    }
    private IEnumerator WriteLine()
    {
        dialogueText.text = string.Empty;
        foreach (char letter in actualLines[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(textSpeed);
        }
    }
}
