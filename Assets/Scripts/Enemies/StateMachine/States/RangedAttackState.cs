using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackState : BaseState
{
    [SerializeField] private RobotAim projectileCannon;
    [SerializeField] private RobotAim projectileTurret;
    [SerializeField] private RobotProjectileCannon rangedCannon;
    [SerializeField] private float reloadTime;

    private Coroutine cannonAimCoroutine;
    private Coroutine turretAimCoroutine;

    protected virtual void OnEnable()
    {
        IsCompleted = true;
        cannonAimCoroutine = StartCoroutine(projectileCannon.Aim(stateMachine.currentTarget));
        turretAimCoroutine = StartCoroutine(projectileTurret.Aim(stateMachine.currentTarget));
    }

    protected virtual void Update()
    {
        if(projectileCannon.IsAimed && rangedCannon.IsReloaded)
        {
            StartCoroutine(RangedAttack());
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        StopCoroutine(cannonAimCoroutine);
        StopCoroutine(turretAimCoroutine);
    }

    private IEnumerator RangedAttack()
    {
        IsCompleted = false;
        animator.SetTrigger(Constants.SHOOT_ANIMATION_TRIGGER);
        rangedCannon.Shoot(reloadTime);
        yield return new WaitForSeconds(GetCurentAnimatonLength());

        IsCompleted = true;
    }
}
