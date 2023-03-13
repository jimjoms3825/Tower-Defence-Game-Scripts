using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private float power = 1;
    [SerializeField] private float speed = 1;
    [SerializeField] [Range(0, 100)] private float accuracy = 100;
    [SerializeField] private float range = 5;

    [SerializeField] private bool turns = true;
    [SerializeField] private GameObject turnTarget;

    private int enemyLayer;

    private float actionTimer = 0;
    private bool enemyInRange = false;

    private void Awake()
    {
        enemyLayer = LayerMask.NameToLayer("Enemy");
        actionTimer = Random.Range(-1, 0);
    }
    private void FixedUpdate()
    {
        if (turns)
        {
            faceEnemy();
        }
        actionTimer += Time.deltaTime;
        if(actionTimer >= speed && enemyInRange)
        {
            Debug.Log(enemyInRange);
            actionTimer = 0;
            TowerAction();
        }
    }

    private void faceEnemy()
    {
        GameObject closest = null;

        foreach (Collider2D col in
            Physics2D.OverlapCircleAll(transform.position, range, ~enemyLayer))
        {
            if (col.gameObject.layer != enemyLayer) continue;
            if(closest == null || Vector2.Distance(transform.position, closest.transform.position) 
                > Vector2.Distance(transform.position, col.transform.position))
            {
                closest = col.gameObject;
            }
        }
        if (closest == null)
        {
            enemyInRange = false;
            return;
        }
        enemyInRange = true;
        turnTarget.transform.up = closest.transform.position - turnTarget.transform.position;
    }

    private void TowerAction()
    {
        Shoot();
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Bullet"));
        bullet.transform.position = transform.position;
        bullet.transform.rotation = turnTarget.transform.rotation;
    }
}
