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

    [Header("Spawn Points References")]
    public Transform LeftSpawnerPoint;
    public Transform RightSpawnerPoint;

    [Header("Kitties Configuration")]
    public GameObject KittiesFolder;
    public GameObject KittiesPrefab;
    public int InitialKittiesPoolSize = 10;
    private List<KittyLogic> Kitties;

    [Header("Game Configuration")]
    [Min(1)]
    public int Lifes;
    [Min(0)]
    public float KittiesSpawnTime;
    [Min(0)]
    public float KittiesSpawnTimeFactor;

    private int score = 0;
    private int gameLevel = 1;
    private float elapsedKittiesSpawnTime = 0;
    private float elapsedDifficultyTime = 0;
    public float DifficultyTime = 10;
    public float DifficultyTimeFactor = 1.3f;

    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(this);

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
        if (elapsedDifficultyTime >= DifficultyTime)
        {
            UpgradeGameLevel();
        }
        else
            elapsedDifficultyTime += Time.deltaTime;

        if (elapsedKittiesSpawnTime >= KittiesSpawnTime)
            ActivateKitty();
        else
            elapsedKittiesSpawnTime += Time.deltaTime;
    }

    public static float GetRandomNumber(float _min, float _max)
    {
        return Random.Range(_min, _max);
    }

    private KittyLogic InstantiateKitty()
    {
        KittyLogic newKitty = Instantiate(KittiesPrefab, LeftSpawnerPoint.position, Quaternion.identity).GetComponent<KittyLogic>();

        if(newKitty)
        {
            newKitty.gameObject.SetActive(false);
            Kitties.Add(newKitty);

            if(KittiesFolder)
                newKitty.transform.SetParent(KittiesFolder.transform);
        }

        return newKitty;
    }

    private void ActivateKitty()
    {
        KittyLogic nextKitty = GetValidKitty();
        nextKitty = nextKitty == null ? InstantiateKitty() : nextKitty;

        if(nextKitty)
        {
            Vector3 newPosition = new Vector3(GetRandomNumber(LeftSpawnerPoint.position.x, RightSpawnerPoint.position.x), LeftSpawnerPoint.position.y, 0);
            nextKitty.Activate(newPosition);
            elapsedKittiesSpawnTime = 0;
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
    //
    public void AddDeadKitty()
    {
        /*if(++deadKitties >= MaxNumberDeadKitties)
        {
            Application.Quit();
        }*/
    }

    public void AddScore()
    {
        ++score;
    }

    private void UpgradeGameLevel()
    {
        ++gameLevel;
        DifficultyTime += DifficultyTimeFactor * DifficultyTime;
        elapsedDifficultyTime = 0;
        KittiesSpawnTime -= KittiesSpawnTimeFactor * KittiesSpawnTime;
    }

    public int GetGameLevel()
    {
        return gameLevel;
    }
}
