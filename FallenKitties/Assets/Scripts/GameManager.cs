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

    private float elapsedTime = 0;
    public float GoalTime = 1;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (elapsedTime >= GoalTime)
            InstantiateKitty();
        else
            elapsedTime += Time.deltaTime;
    }

    public static float GetRandomNumber(float _min, float _max)
    {
        return Random.Range(_min, _max);
    }

    private void InstantiateKitty()
    {
        Vector3 spawnPosition = new Vector3(GetRandomNumber(LeftSpawnerPoint.position.x, RightSpawnerPoint.position.x), LeftSpawnerPoint.position.y, 0);
        GameObject.Instantiate(KittiesPrefab, spawnPosition, Quaternion.identity);
        elapsedTime = 0;
    }
}
