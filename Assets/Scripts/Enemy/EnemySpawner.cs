using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform target;

    public Vector2 offset = new Vector2(1.5f, 1f);
    public Vector2 size = new Vector2(2.5f, 1f);
    public int total = 5;
    int count = 0;
    public float spawnTime = 5f;
    private float spawnTimer = 0;

    private bool isSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        count = total;
        Debug.Log(gameObject.name + " Start");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSpawning)
        {
            if (Global.isBattling)
            {
                count = total;
                spawnTimer = spawnTime;
                isSpawning = true;
            }
        }
        else if (count > 0)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer > spawnTime)
            {
                Vector3 spaenPosition = transform.position + new Vector3(offset.x + Random.Range(-size.x / 2, size.x / 2), offset.y + Random.Range(-size.y / 2, size.y / 2), 0);
                GameObject clone = Instantiate(enemyPrefab, spaenPosition, transform.rotation);
                clone.GetComponent<EnemyBehaviour>().target = target;
                --count;
                spawnTimer = 0;
            }
        }

    }
}
