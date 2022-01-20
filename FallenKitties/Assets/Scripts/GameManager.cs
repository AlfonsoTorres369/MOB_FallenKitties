using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get;
        private set;
    }

    public Transform LeftSpawnerPoint;
    public Transform RightSpawnerPoint;
    public GameObject KittiesPrefab;
    public int InitialKittiesPoolSize = 10;
    public List<KittyLogic> Kitties;

    private int score = 0;
    private int deadKitties = 0;
    public int gameLevel = 1;
    private float elapsedSpawnTime = 0;
    private float elapsedDifficultyTime = 0;
    public float SpawnTime = 2.5f;
    public float DifficultyTime = 10;
    public float DifficultyTimeFactor = 1.3f;
    public int MaxNumberDeadKitties = 3;
    public int MaxGameLevel = 5;
    public float SubtractTimePerLevel = 0.5f;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        Kitties = new List<KittyLogic>();
    }

    private void Start()
    {
        for(int i = 0; i < InitialKittiesPoolSize; i++)
        {
            InstantiateKitty();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(gameLevel < MaxGameLevel)
        {
            if (elapsedDifficultyTime >= DifficultyTime)
            {
                UpgradeGameLevel();
            }
            else
                elapsedDifficultyTime += Time.deltaTime;
        }

        if (elapsedSpawnTime >= SpawnTime)
            ActivateKitty();
        else
            elapsedSpawnTime += Time.deltaTime;
    }

    public static float GetRandomNumber(float _min, float _max)
    {
        return Random.Range(_min, _max);
    }

    private KittyLogic InstantiateKitty()
    {
        KittyLogic newKitty = Instantiate(KittiesPrefab, LeftSpawnerPoint.position, Quaternion.identity).GetComponent<KittyLogic>();
        newKitty.gameObject.SetActive(false);
        Kitties.Add(newKitty);

        return newKitty;
    }

    private void ActivateKitty()
    {
        KittyLogic nextKitty = GetValidKitty();
        nextKitty = nextKitty == null ? InstantiateKitty() : nextKitty;

        if(nextKitty != null)
        {
            Vector3 newPosition = new Vector3(GetRandomNumber(LeftSpawnerPoint.position.x, RightSpawnerPoint.position.x), LeftSpawnerPoint.position.y, 0);
            nextKitty.Activate(newPosition);
            elapsedSpawnTime = 0;
        }
    }

    private KittyLogic GetValidKitty()
    {
        foreach(KittyLogic kitty in Kitties)
        {
            if(!kitty.gameObject.activeInHierarchy)
                return kitty;
        }

        return null;
    }

    public void AddDeadKitty()
    {
        if(++deadKitties >= MaxNumberDeadKitties)
        {
            Application.Quit();
        }
    }

    public void AddScore()
    {
        ++score;
    }
    private void UpgradeGameLevel()
    {
        ++gameLevel;
        DifficultyTime *= DifficultyTimeFactor;
        elapsedDifficultyTime = 0;
        SpawnTime -= SubtractTimePerLevel;
    }
}
