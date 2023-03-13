using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Level : MonoBehaviour
{
    private Tile pathHead;
    private List<Enemy> enemies;

    private bool inWave = false;
    private Tile[,] tileArray;


    private int wave = 0;
    private int enemiesPerWave = 10;
    private int enemiesThisWave;

    private void Awake()
    {
        enemies = new List<Enemy>();
        StartCoroutine(GenerateCoins());
    }


    private void CheckForLevelEnd()
    {
        List<Enemy> newList = new List<Enemy>();
        foreach(Enemy enemy in enemies)
        {
            if (enemy.GetDestroyed() == false)
                newList.Add(enemy);
        }
        enemies = newList;
        if (enemies.Count == 0) EndRound();
    }

    public void StartRound()
    {
        wave++;
        enemiesThisWave = enemiesPerWave * wave;
        inWave = true;
        StartCoroutine(EnemySpawnCoroutine());
    }

    public void EndRound()
    {
        inWave = false;
    }


    public Tile GetPathHead()
    {
        return pathHead;
    }

    public void SetPathHead(Tile t)
    {
        pathHead = t;
    }

    public bool GetInWave()
    {
        return inWave;
    }

    public void SetTileArray(Tile[,] newArr)
    {
        tileArray = newArr;
    }

    public Tile[,] GetTileArray()
    {
        return tileArray;
    }

    private IEnumerator EnemySpawnCoroutine()
    {
        while (enemiesThisWave-- > 0)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void SpawnEnemy()
    {
        Enemy enemy = Instantiate(Resources.Load<GameObject>("Prefabs/Enemy")).GetComponent<Enemy>();
        enemy.transform.position = pathHead.transform.position + Vector3.right;
        enemies.Add(enemy);
        enemies[enemies.Count - 1].onDestroy.AddListener(CheckForLevelEnd);
    }

    private IEnumerator GenerateCoins()
    {
        if (inWave)
            GameManager.GetInstance().AddCoins(1);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(GenerateCoins());
    }
}
