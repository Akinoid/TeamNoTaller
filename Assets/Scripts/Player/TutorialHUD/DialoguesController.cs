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
        tutorialBrawler, tutorialSniper, tutorialAmbusher, tutorialLaser, tutorialGranade;
    public static int tutorial;
    void Start()
    {
        StartMenuManager.tutorial = 2;
        tutorial = 1;
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

    public void ChangeBools()
    {
        Debug.Log(tutorial);
        switch (tutorial)
        {
            case 1:
                tutorialMove = true;
                tutorialDash = false;
                tutorialFinishMovement = false;
                tutorialShoot = false;
                tutorialBrawler = false;
                tutorialSniper = false;
                tutorialAmbusher = false;
                tutorialLaser = false;
                tutorialGranade = false;
                break;
            case 2:
                tutorialMove = false;
                tutorialDash = true;
                tutorialFinishMovement = false;
                tutorialShoot = false;
                tutorialBrawler = false;
                tutorialSniper = false;
                tutorialAmbusher = false;
                tutorialLaser = false;
                tutorialGranade = false;
                break;
            case 3:
                tutorialMove = false;
                tutorialDash = false;
                tutorialFinishMovement = true;
                tutorialShoot = false;
                tutorialBrawler = false;
                tutorialSniper = false;
                tutorialAmbusher = false;
                tutorialLaser = false;
                tutorialGranade = false;
                ActualDialogueTutorial.startTimer = true;
                break;
            case 4:
                tutorialMove = false;
                tutorialDash = false;
                tutorialFinishMovement = false;
                tutorialShoot = true;
                tutorialBrawler = false;
                tutorialSniper = false;
                tutorialAmbusher = false;
                tutorialLaser = false;
                tutorialGranade = false;
                break;
            case 5:
                Instantiate(brawler);
                StartMenuManager.tutorial = 0;
                tutorialMove = false;
                tutorialDash = false;
                tutorialFinishMovement = false;
                tutorialShoot = false;
                tutorialBrawler = true;
                tutorialSniper = false;
                tutorialAmbusher = false;
                tutorialLaser = false;
                tutorialGranade = false;
                break;
            case 6:
                Instantiate(sniper);
                tutorialMove = false;
                tutorialDash = false;
                tutorialFinishMovement = false;
                tutorialShoot = false;
                tutorialBrawler = false;
                tutorialSniper = true;
                tutorialAmbusher = false;
                tutorialLaser = false;
                tutorialGranade = false;
                break;
            case 7:
                Instantiate(ambusher);
                tutorialMove = false;
                tutorialDash = false;
                tutorialFinishMovement = false;
                tutorialShoot = false;
                tutorialBrawler = false;
                tutorialSniper = false;
                tutorialAmbusher = true;
                tutorialLaser = false;
                tutorialGranade = false;
                break;
            case 8:
                Instantiate(laser);
                tutorialMove = false;
                tutorialDash = false;
                tutorialFinishMovement = false;
                tutorialShoot = false;
                tutorialBrawler = false;
                tutorialSniper = false;
                tutorialAmbusher = false;
                tutorialLaser = true;
                tutorialGranade = false;
                break;
            case 9:
                Instantiate(granade);
                tutorialMove = false;
                tutorialDash = false;
                tutorialFinishMovement = false;
                tutorialShoot = false;
                tutorialBrawler = false;
                tutorialSniper = false;
                tutorialAmbusher = false;
                tutorialLaser = false;
                tutorialGranade = true;
                break;
            case 10:
                tutorialMove = false;
                tutorialDash = false;
                tutorialFinishMovement = false;
                tutorialShoot = false;
                tutorialBrawler = false;
                tutorialSniper = false;
                tutorialAmbusher = false;
                tutorialLaser = false;
                tutorialGranade = false;
                break;
            default:
                break;
        }
    }
}
