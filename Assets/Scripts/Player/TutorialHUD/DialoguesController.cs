using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DialoguesController : MonoBehaviour
{
    [SerializeField] private ActualDialogueTutorial actualDialogue;
    [SerializeField] private DialoguesTutorial dialogues;
    public GameObject brawler, laser, sniper, ambusher, granade;
    public static bool tutorialMove, tutorialDash, tutorialFinishMovement, tutorialShoot,
        tutorialBrawler, tutorialSniper, tutorialAmbusher, tutorialLaser, tutorialGranade, tutorialObstacles;
    public static int tutorial;
    void Start()
    {
        tutorial = 4;
        ChangeBools();
        ChangeLines();
    }

    void Update()
    {
        ChangeLines();
    }

    void ChangeLines()
    {
        if (tutorialMove)
        {
            actualDialogue.actualLines = dialogues.linesMove;
        }
        if (tutorialDash)
        {
            actualDialogue.actualLines = dialogues.linesDash;
        }
        if (tutorialFinishMovement)
        {
            actualDialogue.actualLines = dialogues.linesFinishMovement;
        }
        if (tutorialShoot)
        {
            actualDialogue.actualLines = dialogues.linesShoot;
        }
        if (Time.timeScale == 0f || Time.timeScale == 1f)
        {
            if (tutorialBrawler)
            {
                actualDialogue.actualLines = dialogues.linesBrawler;
            }
            if (tutorialSniper)
            {
                actualDialogue.actualLines = dialogues.linesSniper;
            }
            if (tutorialAmbusher)
            {
                actualDialogue.actualLines = dialogues.linesAmbusher;
            }
            if (tutorialLaser)
            {
                actualDialogue.actualLines = dialogues.linesLaser;
            }
            if (tutorialGranade)
            {
                actualDialogue.actualLines = dialogues.linesGranade;
            }
        }
        
        if (tutorialObstacles)
        {
            actualDialogue.actualLines = dialogues.linesObstacles;
        }
    }

    public void ChangeBools()
    {
        Debug.Log(tutorial);
        switch (tutorial)
        {
            case 1:
                tutorialMove = true;
                break;
            case 2:
                tutorialMove = false;
                tutorialDash = true;
                break;
            case 3:
                tutorialDash = false;
                tutorialFinishMovement = true;
                break;
            case 4:
                tutorialFinishMovement = false;
                tutorialShoot = true;
                break;
            case 5:
                Instantiate(brawler);
                tutorialShoot = false;
                tutorialBrawler = true;
                break;
            case 6:
                Instantiate(sniper);
                tutorialBrawler = false;
                tutorialSniper = true;
                break;
            case 7:
                Instantiate(ambusher);
                tutorialSniper = false;
                tutorialAmbusher = true;
                break;
            case 8:
                Instantiate(laser);
                tutorialAmbusher = false;
                tutorialLaser = true;
                break;
            case 9:
                Instantiate(granade);
                tutorialLaser = false;
                tutorialGranade = true;
                break;
            case 10:
                tutorialGranade = false;
                tutorialObstacles = true;
                break;
            default:
                break;
        }
    }
}
