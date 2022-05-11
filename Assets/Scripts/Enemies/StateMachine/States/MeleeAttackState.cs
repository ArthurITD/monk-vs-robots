using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : BaseState
{
    [SerializeField] private Collider meleeAttackCollider;
    [SerializeField] private HitDetector weaponHitDetector;
    [Min(0.1f)]
    [SerializeField] private float delayBetweenAttacks;

    private bool isLookAtTarget = false;

    private void OnEnable()
    {
        if(meleeAttackCollider == null)
        {
            Debug.LogError($"MeleeAttackCollider wasn't set on {name} enemy");
            IsCompleted = true;
            return;
        }
        StartCoroutine(AttackWithIntervals());
    }

    private IEnumerator AttackWithIntervals()
    {
        IsCompleted = false;
        isLookAtTarget = true;

        //Play prepearing animation
        //Test preparation duration, will be changed after implementing animations
        yield return new WaitForSeconds(2f);
        isLookAtTarget = false;

        meleeAttackCollider.enabled = true;
        //Play attack animation
        //Test attack duration, will be changed after implementing animations
        yield return new WaitForSeconds(2f);

        meleeAttackCollider.enabled = false;
        weaponHitDetector.ClearHittedTargets();
        IsCompleted = true;

        //Play return to normal state animation
        yield return new WaitForSeconds(delayBetweenAttacks);
        StartCoroutine(AttackWithIntervals());
    }

    protected void Update()
    {
        if (isLookAtTarget)
        {
            var modiffiedTargetPosition = new Vector3(
                stateMachine.currentTarget.position.x, 
                transform.position.y,
                stateMachine.currentTarget.position.z);

            var targetDirection = (modiffiedTargetPosition - transform.position).normalized;
            var targetRotation = Quaternion.LookRotation(targetDirection);
            if (Quaternion.Angle(transform.rotation, targetRotation) > 5)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 1f);
            }
        }
    }

    protected override void OnDisable()
    {
        StopCoroutine(AttackWithIntervals());
        base.OnDisable();
    }
}
