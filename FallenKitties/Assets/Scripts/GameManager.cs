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

    [Header("Player Configuration")]
    public Player player;
    public Transform PlayerSpawnPosition;

    [Header("Game Configuration")]
    [Min(1)]
    public int HealthPoints;
    [Min(0)]
    public float InitiaKittiesSpawnTime;
    [Min(0)]
    public float KittiesSpawnTimeFactor;
    [Min(0)]
    public float InitialDifficultyTime = 10;
    [Min(0)]
    public float DifficultyTimeFactor = 1.3f;

    [Header("Game Data")]
    public string ScoreDataId;

    //Kitties references
    private List<KittyLogic> Kitties;

    //Game Logic
    private int score = 0;
    private int gameLevel = 1;
    private int maxScore;
    private bool playing = false;
    private int currentHealthPoints;

    private float elapsedKittiesSpawnTime = 0;
    private float currentKittiesSpawnTime;
    private float elapsedDifficultyTime = 0;
    private float currentDifficultyTime;

    //Delegates
    public delegate void OnPropertyUpdated(int _value);
    public event OnPropertyUpdated ScoreUpdated;
    public event OnPropertyUpdated HealthPointsUpdated;
    public event OnPropertyUpdated MaxScoreUpdated;

    public delegate void OnGameplayMessage();
    public event OnGameplayMessage OnStopGame;

    public delegate void OnPauseGame(bool _status);
    public event OnPauseGame OnPause;


    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(this);

        Kitties = new List<KittyLogic>();
        GenerateData();
    }

    private void Start()
    {
        LoadData();
        HealthPointsUpdated(HealthPoints);
    }

    // Update is called once per frame
    void Update()
    {
        if(playing)
        {
            if (elapsedDifficultyTime >= currentDifficultyTime)
            {
                UpgradeGameLevel();
            }
            else
                elapsedDifficultyTime += Time.deltaTime;

            if (elapsedKittiesSpawnTime >= currentKittiesSpawnTime)
                ActivateKitty();
            else
                elapsedKittiesSpawnTime += Time.deltaTime;
        }
    }

    // --------- Game flow & logic ---------
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

    private KittyLogic GetValidKitty()
    {
        foreach (KittyLogic kitty in Kitties)
        {
            if (!kitty.gameObject.activeInHierarchy)
                return kitty;
        }

        return null;
    }

    private void ActivatePlayer()
    {
        if(player)
        {
            player.transform.position = PlayerSpawnPosition.position;
            player.gameObject.SetActive(true);
            player.SetInput(true);
        }
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
    
    public void SubstractLife()
    {
        SetCurrentHealthPoints(--currentHealthPoints);

        if(currentHealthPoints <= 0)
        {
            StopGame();

            if(UIManager.Instance)
            {
                UIManager.Instance.BackToMainMenu();
            }
        }
    }

    public void AddScore()
    {
        SetScore(++score);

        if(score > maxScore)
        {
            SetMaxScore(score);
            SaveData();
        }
    }

    private void UpgradeGameLevel()
    {
        ++gameLevel;
        currentDifficultyTime += DifficultyTimeFactor * currentDifficultyTime;
        elapsedDifficultyTime = 0;
        currentKittiesSpawnTime -= KittiesSpawnTimeFactor * currentKittiesSpawnTime;
    }

    public void StartGame()
    {
        SetScore(0);
        SetCurrentHealthPoints(HealthPoints);
        playing = true;
        currentKittiesSpawnTime = InitiaKittiesSpawnTime;
        currentDifficultyTime = InitialDifficultyTime;
        gameLevel = 1;

        if (Kitties.Count == 0)
        {
            for (int i = 0; i < InitialKittiesPoolSize; i++)
            {
                InstantiateKitty();
            }
        }

        ActivatePlayer();
    }

    public void StopGame()
    {
        playing = false;

        if(OnStopGame != null)
            OnStopGame();
    }

    public void RestartGame()
    {
        StopGame();
        StartGame();
    }

    public void PauseGame(bool _status)
    {
        playing = !_status;

        if(OnPause != null)
            OnPause(_status);
    }
    // ------------------------------------

    // --------- Game Data Functions ---------
    private void LoadData()
    {
        SetMaxScore(PlayerPrefs.GetInt(ScoreDataId));
    }

    private void GenerateData()
    {
        if (!PlayerPrefs.HasKey(ScoreDataId))
            PlayerPrefs.SetInt(ScoreDataId, 0);
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt(ScoreDataId, maxScore);
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    // ------------------------------------

    // --------- Setters / Getters ---------
    public void SetScore(int _value)
    {
        score = _value;

        if(ScoreUpdated != null)
            ScoreUpdated(score);
    }

    public void SetMaxScore(int _value)
    {
        maxScore = _value;

        if(MaxScoreUpdated != null)
            MaxScoreUpdated(maxScore);
    }

    public void SetCurrentHealthPoints(int _value)
    {
        currentHealthPoints =_value;

        if(HealthPointsUpdated != null)
            HealthPointsUpdated(currentHealthPoints);
    }

    public int GetGameLevel()
    {
        return gameLevel;
    }
    // ------------------------------------

    // --------- Static methods (utilities) ---------
    public static float GetRandomNumber(float _min, float _max)
    {
        return Random.Range(_min, _max);
    }
    // ------------------------------------
}
