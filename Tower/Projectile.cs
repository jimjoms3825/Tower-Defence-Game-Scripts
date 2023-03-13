using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float damage = 1;

    private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Projectile");
        StartCoroutine(DestroyAfterSeconds());
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.up);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>())
            collision.gameObject.GetComponent<Enemy>().Damage(damage);

        Destroy(gameObject);
    }

    private IEnumerator DestroyAfterSeconds()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
