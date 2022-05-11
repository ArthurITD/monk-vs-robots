using Opsive.Shared.Events;
using Opsive.UltimateCharacterController.Character.Abilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultStartType(AbilityStartType.ButtonDown)]
[DefaultInputName("Fire1")]
public class SpearMelee : Ability
{
    private bool isAtacking = false;
    private SpearWeapon spearWeapon;

    public override void Awake()
    {
        base.Awake();
        spearWeapon = CharacterControllerHelper.Instance.charactersSpearWeapon;
    }

    public override bool CanStartAbility()
    {
        if(!base.CanStartAbility())
        {
            return false;
        }

        return !isAtacking 
            && m_CharacterLocomotion.Grounded
            && !m_CharacterLocomotion.IsAbilityTypeActive<SpearAimAbility>()
            && spearWeapon.IsInHand;
    }

    protected override void AbilityStarted()
    {
        base.AbilityStarted();
        ChangeAttackState(true);
    }

    protected override void AbilityStopped(bool force)
    {
        base.AbilityStopped(force);
        ChangeAttackState(false);
    }

    private void ChangeAttackState(bool isActive)
    {
        isAtacking = isActive;
        EventHandler.ExecuteEvent<bool>(CharacterControllerHelper.Instance.Character, "OnHitActivate", isAtacking);
    }
}
