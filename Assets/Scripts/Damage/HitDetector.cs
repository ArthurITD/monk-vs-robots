using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DamageInfo
{
    public float damageAmount;
    public string damageDealerName;
    public float baseDamage;
    //Add refs to attack buffs\debuffs
}

public class HitDetector : MonoBehaviour
{
    public List<string> hitTags = new List<string>();
    public DamageInfo damageInfo;

    private List<GameObject> hittedTargets = new List<GameObject>();

    public void ClearHittedTargets()
    {
        hittedTargets.Clear();
    }

    private void OnTriggerEnter(Collider hitCollider)
    {
        if (hitTags.Contains(hitCollider.tag) && !hittedTargets.Contains(hitCollider.gameObject))
        {
            hittedTargets.Add(hitCollider.gameObject);
            hitCollider.GetComponent<DamageProcessor>().ProcessDamage(damageInfo);
        }
    }
}
