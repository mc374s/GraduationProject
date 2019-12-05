using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform target;

    public int total = 5;
    int count = 0;
    public float spawnTime = 5f;
    private float spawnTimer = 0;

    public bool RandomSapwn = true;
    [Header("ランダム生成範囲"),Tooltip("When RandomSapwn is true")]
    public Vector2 offset = new Vector2(1.5f, 1f);
    public Vector2 size = new Vector2(2.5f, 1f);
    [Header("指定生成位置"), Tooltip("When RandomSapwn is false")]
    public GameObject[] spawnPoints;

    private bool isSpawning = false;

    private GameObject[] enemyCollection;

    // Start is called before the first frame update
    void Start()
    {
        enemyCollection = new GameObject[total];
        count = 0;
        //Debug.Log(gameObject.name + " Start");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSpawning)
        {
            if (Global.isBattling)
            {
                count = 0;
                spawnTimer = spawnTime;
                isSpawning = true;
            }
        }
        else if (count < total)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer > spawnTime)
            {
                Vector3 spawnPosition;
                Quaternion spawnRotation;
                if (RandomSapwn)
                {
                    spawnPosition = transform.position + new Vector3(offset.x + Random.Range(-size.x / 2, size.x / 2), offset.y + Random.Range(-size.y / 2, size.y / 2), 0);
                    spawnRotation = transform.rotation;
                }
                else
                {
                    int spawnPointIndex = count < spawnPoints.Length ? count : spawnPoints.Length - 1;
                    spawnPosition = spawnPoints[spawnPointIndex].transform.position;
                    spawnRotation = spawnPoints[spawnPointIndex].transform.rotation;
                }
                enemyCollection[count] = Instantiate(enemyPrefab, spawnPosition, spawnRotation);
                enemyCollection[count].GetComponent<EnemyBehaviour>().target = target;
                ++count;
                spawnTimer = 0;
            }
        }
        if (count >= total)
        {
            if (!CheckAlive())
            {
                Global.isBattling = false;
            }
        }

    }

    public bool CheckAlive()
    {
        if (count < total)
        {
            return true;
        }
        for (int i = 0; i < total; i++)
        {
            if (enemyCollection[i] != null)
            {
                return true;
            }
        }
        return false;
    }
}
