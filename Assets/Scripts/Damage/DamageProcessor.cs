using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[Serializable]
public class OnDamage : UnityEvent<float> { }

public class DamageProcessor : MonoBehaviour
{
    public bool isInvincibleAfterHit = false;
    public float hitDelay = 0.5f;
    [NonSerialized] public float dodgeChance = 0;
    [SerializeField] public OnDamage OnDamage;

    private bool isInvincible = false;
    private int notDodgedHits = 0;

    public void ProcessDamage(DamageInfo damageInfo)
    {
        if (!isInvincible)
        {
            if (dodgeChance > 0 && IsHitDodged())
            {
                return;
            }
            if (isInvincibleAfterHit)
            {
                StartCoroutine(AfterHitInvincibility());
            }
            OnDamage.Invoke(damageInfo.damageAmount);
            //Apply Debuf
        }
    }

    private bool IsHitDodged()
    {
        if(notDodgedHits >= Constants.NOT_DODGED_ATTACKS_LIMIT)
        {
            notDodgedHits = 0;
            return true;
        }
        if(Random.Range(0,101) <=dodgeChance)
        {
            notDodgedHits = 0;
            return true;
        }

        notDodgedHits++;
        return false;
    }

    private IEnumerator AfterHitInvincibility()
    {
        isInvincible = true;
        yield return new WaitForSeconds(hitDelay);
        isInvincible = false;
    }
}
