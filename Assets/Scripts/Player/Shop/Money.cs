using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
public class Money : MonoBehaviour
{
    public static float money;
    public static float score;
    public static float multiplier;
    public static float timeBetweenRest;
    private bool startCoroutine;
   [SerializeField] public static bool startRest;
   [SerializeField] private bool test;
    private Coroutine coroutine;

    public List<Money> combo = new List<Money>();
    private PlayerLife playerLife;

    void Start()
    {
        multiplier = 1;
        coroutine = StartCoroutine(IESpawnPowerUps(combo.Count, timeBetweenRest));
        playerLife = GameObject.Find("Player").GetComponent<PlayerLife>();
    }

    void Update()
    {
        ComboScore();
        if (test)
        {
            Test();
        }
    }

    private void ComboScore()
    {
        float combos = combo.Count;
        switch (combos)
        {
            case 0:
                multiplier = 1;
                break;
            case 5:
                multiplier = 2;
                break;
            case 10:
                multiplier = 3;
                break;
            case 15:
                multiplier = 4;
                break;
            case 20:
                multiplier = 5;
                break;
        }       
    }


    private IEnumerator IESpawnPowerUps(int comboAmount, float timeBetweenRest)
    {
        while (!startCoroutine)
        {
            if (startRest)
            {
                comboAmount = combo.Count;
                if (combo.Count >= 20)
                {
                    for (int i = comboAmount; i > 15; i--)
                    {
                        combo.RemoveAt(i - 1);
                        Debug.Log(combo.Count);
                        startRest = false;
                        yield return new WaitForSeconds(timeBetweenRest);
                    }
                }
                else if (combo.Count >= 15)
                {
                    for (int i = comboAmount; i > 10; i--)
                    {
                        combo.RemoveAt(i - 1);
                        Debug.Log(combo.Count);
                        startRest = false;
                        yield return new WaitForSeconds(timeBetweenRest);
                    }
                }
                else if (combo.Count >= 10)
                {
                    for (int i = comboAmount; i > 5; i--)
                    {
                        combo.RemoveAt(i - 1);
                        Debug.Log(combo.Count);
                        startRest = false;
                        yield return new WaitForSeconds(timeBetweenRest);
                    }
                }
                else if (combo.Count >= 5)
                {
                    for (int i = comboAmount; i > 0; i--)
                    {
                        combo.RemoveAt(i - 1);
                        Debug.Log(combo.Count);
                        startRest = false;
                        yield return new WaitForSeconds(timeBetweenRest);
                    }
                }
            }
            yield return new WaitForSeconds(timeBetweenRest);
        }
    }

    private void Test()
    {
        Debug.Log(multiplier);
        test = false;
    }
}
