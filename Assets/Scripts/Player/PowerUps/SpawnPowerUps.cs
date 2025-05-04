using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SpawnPowerUps : MonoBehaviour
{
    [Header("Object Features")]
    [SerializeField] private List<GameObject> powerUps;
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private Vector3 pos;
    [SerializeField] private Vector2 pos1;
    [SerializeField] private Vector2 pos2;
    [SerializeField] private Vector2 pos3;
    [SerializeField] private Vector2 pos4;
    [SerializeField] private Vector2 pos5;
    private Transform spawnTransform;
    [Header("Couroutine")]
    [SerializeField] private float timeBetweenSpawn;
    [SerializeField] private int PUpsAmount;
    [SerializeField] private bool startSpawn;
    private Coroutine coroutine;
    private bool startCoroutine;
    [Header("Random Values")]
    [SerializeField] private int  index;
    [SerializeField] private int  canSpawn;
    [SerializeField] private float changePos;
    [SerializeField] private float changePosX;
    [SerializeField] private float changePosY;
    [SerializeField] private float posX;
    [SerializeField] private float posY;
    [Header("Bools Checks")]
    [SerializeField] private bool objectSpawned;
    [SerializeField] private bool changePosition;
    [SerializeField] private bool check1;
    [SerializeField] private bool check2;
    [SerializeField] private bool check3;
    [Header("Timers")]
    [SerializeField] private float timerSpawn;
    [SerializeField] private float timer;

    void Start()
    {
        coroutine = StartCoroutine(IESpawnPowerUps(PUpsAmount, timeBetweenSpawn));
        spawnTransform = gameObject.transform;
        index = Random.Range(0, powerUps.Count);
        posX = Random.Range(-20, 21);
        posY = Random.Range(-20, 21);
        canSpawn = 2;
    }

    void Update()
    {
        if(powerUps.Count > 0)
        {
            ChooseObjectToSpawn();
            ChoosePositionToSpawn();
            TimerToSpawn();
            if (startSpawn)
            {
                SpawnTime();
            }
        }
        
    }

    private void SpawnTime()
    {
        timer += Time.deltaTime;
        if(timer >= 1f)
        {
            startSpawn = false;
            timer = 0;
        }
    }

    private void TimerToSpawn()
    {
        timerSpawn += Time.deltaTime;
        if(timerSpawn >= 8f && !check1)
        {
            canSpawn = Random.Range(0, 2);
            if(canSpawn == 0)
            {
                check1 = true;
            }
            timerSpawn = 0;
        }
        if(canSpawn == 0 && timerSpawn >= 7 && check1 && !check2)
        {
            canSpawn = Random.Range(0, 2);
            if(canSpawn == 0)
            {
                check2 = true;
            }
            timerSpawn = 0;
        }
        if(canSpawn == 0 && timerSpawn >= 6 && check2 && !check3)
        {
            canSpawn = Random.Range(0, 2);
            if (canSpawn == 0)
            {
                check3 = true;
            }
            timerSpawn = 0;
        }
        if(canSpawn == 0 && timerSpawn >= 5 && check3)
        {
            canSpawn = 1;
            timerSpawn = 0;
        }

        if(canSpawn == 1)
        {
            check1 = false;
            check2 = false;
            check3 = false;
            startSpawn = true;
            canSpawn = 0;
        }
    }

    private void ChooseObjectToSpawn()
    {
        if (objectSpawned)
        {
            index = Random.Range(0, powerUps.Count);
            objectSpawned = false;
        }
        objectToSpawn = powerUps[index];
    }
    private void ChoosePositionToSpawn()
    {
        pos1 = new Vector2(-6, 8);
        pos2 = new Vector2(6, 8);
        pos3 = new Vector2(0, 0);
        pos4 = new Vector2(6, -8);
        pos5 = new Vector2(-6, -8);
        if (changePosition)
        {
            changePos = Random.Range(1, 6);
            ChangePos();
            posX = changePosX;
            posY = changePosY;
            changePosition = false;
        }
        pos = new Vector3(posX, posY, spawnTransform.position.z);
    }
    private void ChangePos()
    {
        switch (changePos)
        {
            case 1:
                changePosX = pos1.x;
                changePosY = pos1.y;
                break;
            case 2:
                changePosX = pos2.x;
                changePosY = pos2.y;
                break;
            case 3:
                changePosX = pos3.x;
                changePosY = pos3.y;
                break;
            case 4:
                changePosX = pos4.x;
                changePosY = pos4.y;
                break;
            case 5:
                changePosX = pos5.x;
                changePosY = pos5.y;
                break;
        }
    }
    private void SpawnPUps()
    {
        Instantiate(objectToSpawn, pos, spawnTransform.rotation);
        objectSpawned = true;
        changePosition = true;
    }
    private IEnumerator IESpawnPowerUps(int PUpsAmount, float timeBetweenSpawn)
    {
        while (!startCoroutine)
        {
            if (startSpawn)
            {
                for (int i = 0; i < PUpsAmount; i++)
                {
                    SpawnPUps();
                    yield return new WaitForSeconds(timeBetweenSpawn);
                }
            }
            yield return new WaitForSeconds(timeBetweenSpawn);
        }
    }

}
