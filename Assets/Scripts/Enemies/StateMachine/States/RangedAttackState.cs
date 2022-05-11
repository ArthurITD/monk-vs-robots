using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackState : BaseState
{
    [SerializeField] private List<Transform> projectileCannons;
    [SerializeField] private List<RobotProjectileCannon> rangedCannons;
    [SerializeField] private float reloadTime;

    //Needed to be reworked in future to grab actual player height
    private float playerHeight = 1.8f;

    private void Update()
    {
        for (int i = 0; i < projectileCannons.Count; i++)
        {
            if (AimCannon(projectileCannons[i]) && rangedCannons[i].IsReloaded)
            {
                IsCompleted = false;
                rangedCannons[i].Shoot(reloadTime);
                IsCompleted = true;
            }
        }
    }

    private bool AimCannon(Transform cannonTransform)
    {
        var modiffiedTargetPosition = new Vector3(
            stateMachine.currentTarget.position.x,
            stateMachine.currentTarget.position.y + playerHeight,
            stateMachine.currentTarget.position.z);
        var targetDirection = (modiffiedTargetPosition - cannonTransform.position).normalized;
        var targetRotation = Quaternion.LookRotation(targetDirection);

        if (Quaternion.Angle(cannonTransform.rotation, targetRotation) > 1)
        {
            cannonTransform.rotation = Quaternion.Slerp(cannonTransform.rotation, targetRotation, Time.deltaTime * 2f);
            return false;
        }

        return true;
    }
}
