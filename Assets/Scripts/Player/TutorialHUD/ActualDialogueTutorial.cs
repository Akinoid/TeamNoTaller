using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ActualDialogueTutorial : MonoBehaviour
{
    public TMP_Text dialogueText;
    public DialoguesController dialoguesController;
    [TextArea(4, 6)] public string[] actualLines;

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
        if (Time.timeScale == 0f || Time.timeScale == 1f)
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
            }
        }
    }

    public void StartDialogue()
    {
        didDialogueStart = true;
        if(Time.timeScale == 0f || Time.timeScale == 1f)
        {
            StartCoroutine(WriteLine());
        }
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
            if(DialoguesController.tutorial == 3 || DialoguesController.tutorial == 10 || DialoguesController.tutorial == 11)
            {
                startTimer = true;
            }
            Time.timeScale = 1f;
        }
    }

    void ChangeLines()
    {
        timer += Time.deltaTime;
        if(timer >= 3)
        {
            DialoguesController.tutorial += 1;
            dialoguesController.ChangeBools();
            Invoke("StartDialogue", 0.02f);
            tutorial = true;
            timer = 0;
            
        }
    }

    void ChangeLinesWithOutTimer()
    {
        DialoguesController.tutorial += 1;
        dialoguesController.ChangeBools();
        Invoke("StartDialogue", 0.02f);
        startChangeLines = false;
        tutorial = true;

    }
    private IEnumerator WriteLine()
    {
        if (Time.timeScale == 0f || Time.timeScale == 1f)
        {
            dialogueText.text = string.Empty;
            foreach (char letter in actualLines[index].ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSecondsRealtime(textSpeed);
            }
        }
    }
}
