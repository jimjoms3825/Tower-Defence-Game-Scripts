using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    private Tile nextTile;

    [SerializeField] private float speed = 5;
    [SerializeField] private int damage = 5;
    [SerializeField] private float maxLife = 5;

    private float life;
    private bool destroyed = false;

    public UnityEvent onDestroy = new UnityEvent();

    private void Awake()
    {
        life = maxLife;
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        nextTile = GameManager.GetInstance().GetCurrentLevel().GetPathHead();
        GetComponent<SpriteRenderer>().sortingOrder = 1;
    }

    private void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, nextTile.transform.position, speed);
        if(transform.position == nextTile.transform.position)
        {
            if (nextTile.GetConnectedTile() != null)
                nextTile = nextTile.GetConnectedTile();
            else
                DealDamage();
        }
    }

    private void Die()
    {
        DestroyEnemy();
    }

    private void DealDamage()
    {
        GameManager.GetInstance().DealDamage(damage);
        DestroyEnemy();
    }

    private void DestroyEnemy()
    {
        destroyed = true;
        onDestroy.Invoke();
        Destroy(gameObject);
    }

    public bool GetDestroyed()
    {
        return destroyed;
    }

    public void Damage(float damage)
    {
        life = Mathf.Clamp(life - damage, 0, maxLife);
        if(life == 0)
            Die();
    }
}
