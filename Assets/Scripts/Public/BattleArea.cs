using UnityEngine;

public class BattleArea : Trigger
{
    new Collider2D collider = null;

    public Event onBattleFinish;

    public Collider2D[] wallCollection;

    public EnemySpawner[] spawnerCollection;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider2D>();
        collider.enabled = true;
        if (spawnerCollection.Length > 0)
        {
            foreach (EnemySpawner spawner in spawnerCollection)
            {
                spawner.enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnerCollection.Length > 0 && Global.isBattling)
        {
            bool noEnemyAlive = true;
            foreach (EnemySpawner spawner in spawnerCollection)
            {
                noEnemyAlive = noEnemyAlive && !spawner.CheckAlive();
            }
            if (noEnemyAlive)
            {
                onBattleFinish.Invoke(gameObject);
            }
        }
    }

    public void StartBattle()
    {
        Debug.Log("Start Battle!");
        Global.isBattling = true;
        collider.enabled = false;
        if (spawnerCollection.Length > 0)
        {
            foreach (EnemySpawner spawner in spawnerCollection)
            {
                spawner.enabled = true;
            }
        }
        Global.borderLeft = wallCollection[0].transform.position.x/* + wallCollection[0].GetComponentInParent<Transform>().position.x + transform.position.x*/;
        Global.borderRight = wallCollection[1].transform.position.x/* + wallCollection[1].GetComponentInParent<Transform>().position.x + transform.position.x*/;

        //foreach (var item in wallCollection)
        //{
        //    if (item != null)
        //    {
        //        item.isTrigger = !Global.isBattling;
        //    }
        //}
        wallCollection[0].isTrigger = !Global.isBattling;
        wallCollection[1].isTrigger = !Global.isBattling;
    }

    public void FinishBattle()
    {
        Debug.Log("Finish Battle!");
        gameObject.SetActive(false);
        if (spawnerCollection.Length > 0)
        {
            foreach (EnemySpawner spawner in spawnerCollection)
            {
                spawner.enabled = false;
            }
        }
        Global.isBattling = false;
    }
}
