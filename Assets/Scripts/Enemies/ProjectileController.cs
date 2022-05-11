using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private float speed = 2f;
    private float destructionTime = 5f;

    void Start()
    {
        StartCoroutine(DestroyProjectile());
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(destructionTime);
        Destroy(gameObject);
    }
}
