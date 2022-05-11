using Opsive.Shared.Events;
using Opsive.Shared.Input;
using Opsive.UltimateCharacterController.Character;
using Opsive.UltimateCharacterController.Character.Abilities.Items;
using Opsive.UltimateCharacterController.Utility;
using UnityEngine;
using Opsive.UltimateCharacterController.Character.Abilities;

[DefaultStartType(AbilityStartType.ButtonDownContinuous)]
[DefaultStopType(AbilityStopType.ButtonUp)]
[DefaultInputName("Fire2")]
public class SpearAimAbility : Ability
{
    private ILookSource m_LookSource;
    private SpearWeapon spearWeapon;

    [SerializeField] protected GameObject CrosshairObject;

    public override void Awake()
    {
        spearWeapon = CharacterControllerHelper.Instance.charactersSpearWeapon;
        base.Awake();
    }

    public override bool CanStartAbility()
    {
        if(m_LookSource==null)
        {
            m_LookSource = m_CharacterLocomotion.LookSource;
        }
        if (!base.CanStartAbility())
        {
            return false;
        }
        return spearWeapon.IsInHand;
    }

    public override void UpdateRotation()
    {
        // If the character can look independently then the character does not need to rotate to face the look direction.
        if (m_CharacterLocomotion.ActiveMovementType.UseIndependentLook(true))
        {
            return;
        }

        if (m_LookSource == null)
        {
            return;
        }

        // Determine the direction that the character should be facing.
        var transformRotation = m_Transform.rotation;
        var lookDirection = m_LookSource.LookDirection(m_LookSource.LookPosition(true), true, m_CharacterLayerManager.IgnoreInvisibleCharacterLayers, false, false);
        var rotation = transformRotation * Quaternion.Euler(m_CharacterLocomotion.DeltaRotation);
        var localLookDirection = MathUtility.InverseTransformDirection(lookDirection, rotation);
        localLookDirection.y = 0;
        lookDirection = MathUtility.TransformDirection(localLookDirection, rotation);
        var targetRotation = Quaternion.LookRotation(lookDirection, rotation * Vector3.up);
        m_CharacterLocomotion.DeltaRotation = (Quaternion.Inverse(transformRotation) * targetRotation).eulerAngles;
    }

    protected override void AbilityStarted()
    {
        base.AbilityStarted();
        CrosshairObject.SetActive(true);
        EventHandler.ExecuteEvent(m_GameObject, "OnAimAbilityStart", true, true);
        EventHandler.ExecuteEvent(m_GameObject, "OnAimAbilityAim", true);
    }

    protected override void AbilityStopped(bool force)
    {
        base.AbilityStopped(force);
        CrosshairObject.SetActive(false);
        EventHandler.ExecuteEvent(m_GameObject, "OnAimAbilityStart", false, true);
        EventHandler.ExecuteEvent(m_GameObject, "OnAimAbilityAim", false);
    }
}
