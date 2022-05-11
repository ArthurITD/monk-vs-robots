using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firingPoint;

    void Start()
    {
        StartCoroutine(FireProjectile());
    }

    private IEnumerator FireProjectile()
    {
        var projectileController = Instantiate(projectilePrefab, firingPoint.position, firingPoint.rotation)
            .GetComponent<ProjectileController>();
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(FireProjectile());
    }
}
