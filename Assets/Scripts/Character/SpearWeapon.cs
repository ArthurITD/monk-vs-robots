using enums;
using Opsive.Shared.Events;
using Opsive.UltimateCharacterController.Camera;
using System.Collections;
using System.Collections.Generic;
using TotemEntities;
using UnityEngine;

public class SpearWeapon : MonoBehaviour
{
    public float criticalDamageChance = 0;

    [SerializeField] private TipMaterialEnumGameObjectDictionary tipTypes;
    [SerializeField] private Material spearShaftMaterial;
    [SerializeField] private Collider spearShaftCollider;
    [SerializeField] private SpearRanged spearRangedController;
    [SerializeField] private PickupItem spearPicker;
    [SerializeField] private CameraController characterCameraController;

    private TipMaterialEnum tipMaterial;
    private ElementEnum element;

    private GameObject character;
    private HitDetector weaponHitDetector;
    private Collider spearTipCollider;
    private int nonCriticalAttacks = 0;

    private Transform parentRoot;
    private Vector3 startPosition;
    private Quaternion startRotation;

    public bool IsInHand { get; private set; } = true;

    void Awake()
    {
        character = CharacterControllerHelper.Instance.Character;
        weaponHitDetector = GetComponent<HitDetector>();
        InitializeSpear(TotemManager.Instance.currentSpear);

        EventHandler.RegisterEvent<bool>(character, "OnHitActivate", EnableHitCollider);
        EventHandler.RegisterEvent(character,"OnThrowSpear", OnSpearThrow);
        EventHandler.RegisterEvent(gameObject, "OnSpearLanded", OnSpearLanded);
        EventHandler.RegisterEvent(gameObject, "OnSpearPickedUp", OnSpearPickedUp);
        EventHandler.RegisterEvent("GameRestarted", OnGameRestarted);
    }

    public void InitializeSpear(TotemSpear spear)
    {
        SetSpearTip(spear.tipMaterial);
        weaponHitDetector.damageInfo.damageAmount = spear.damage;
        weaponHitDetector.damageInfo.baseDamage = spear.damage;
        spearRangedController.throwingForce = spear.range;

        //To Do: assign attack buf\debuf depending on element
        element = spear.element;
        spearShaftMaterial.color = spear.shaftColor;

        parentRoot = transform.parent;
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;
    }

    private void SetSpearTip(TipMaterialEnum tipMaterialToSet)
    {
        tipTypes[tipMaterial].SetActive(false);
        tipMaterial = tipMaterialToSet;

        var newTip = tipTypes[tipMaterial];
        newTip.SetActive(true);
        weaponHitDetector = newTip.GetComponent<HitDetector>();
        spearTipCollider = newTip.GetComponent<BoxCollider>();
    }

    private void EnableHitCollider(bool isEnabled)
    {
        spearTipCollider.enabled = isEnabled;

        if(isEnabled)
        {
            if(criticalDamageChance > 0 && IsCriticalHit())
            {
                weaponHitDetector.damageInfo.damageAmount =
                    weaponHitDetector.damageInfo.baseDamage * Constants.CRITICAL_DAMAGE_MULTIPLIER;
            }
            else
            {
                weaponHitDetector.damageInfo.damageAmount = weaponHitDetector.damageInfo.baseDamage;
            }
        }
        else
        {
            weaponHitDetector.ClearHittedTargets();
        }
    }

    private bool IsCriticalHit()
    {
        if (nonCriticalAttacks >= Constants.NON_CRITICAL_ATTACKS_LIMIT)
        {
            nonCriticalAttacks = 0;
            return true;
        }

        if (Random.Range(0, 100) <= criticalDamageChance)
        {
            nonCriticalAttacks = 0;
            return true;
        }

        nonCriticalAttacks++;
        return false;
    }

    private void OnSpearThrow()
    {
        EnableHitCollider(true);
        characterCameraController.CanZoom = false;
        gameObject.layer = Constants.RANGED_SPEAR_LAYER_INDEX;
        IsInHand = false;
        transform.SetParent(null);
        spearRangedController.enabled = true;
    }

    private void OnSpearLanded()
    {
        EnableHitCollider(false);
        gameObject.layer = Constants.PICKUP_LAYER_INDEX;
        spearRangedController.enabled = false;
        spearShaftCollider.isTrigger = true;
        spearPicker.enabled = true;
    }

    private void OnSpearPickedUp()
    {
        transform.SetParent(parentRoot);
        transform.localPosition = startPosition;
        transform.localRotation = startRotation;

        IsInHand = true;
        spearShaftCollider.isTrigger = false;
        spearPicker.enabled = false;
        characterCameraController.CanZoom = true;
    }

    private void OnGameRestarted()
    {
        EnableHitCollider(false);
        spearRangedController.enabled = false;
    }

    private void OnDestroy()
    {
        EventHandler.UnregisterEvent<bool>(character, "OnHitActivate", EnableHitCollider);
        EventHandler.UnregisterEvent(character, "OnThrowSpear", OnSpearThrow);
        EventHandler.UnregisterEvent(gameObject, "OnSpearLanded", OnSpearLanded);
        EventHandler.UnregisterEvent(gameObject, "OnSpearPickedUp", OnSpearPickedUp);
        EventHandler.UnregisterEvent("GameRestarted", OnGameRestarted);
    }
}
