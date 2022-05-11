using Opsive.Shared.Events;
using Opsive.UltimateCharacterController.Character.Abilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultStartType(AbilityStartType.ButtonDown)]
[DefaultInputName("Fire1")]
public class SpearThrowAbility : Ability
{
    private Ability modifiedAimAbility;

    public override void Awake()
    {
        base.Awake();

        modifiedAimAbility = m_CharacterLocomotion.GetAbility<SpearAimAbility>();
    }

    public override bool CanStartAbility()
    {
        if (!base.CanStartAbility())
        {
            return false;
        }

        return m_CharacterLocomotion.IsAbilityTypeActive<SpearAimAbility>();
    }

    protected override void AbilityStarted()
    {
        base.AbilityStarted();
        EventHandler.ExecuteEvent(CharacterControllerHelper.Instance.Character, "OnThrowSpear");
    }
}
