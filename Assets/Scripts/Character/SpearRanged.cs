using Opsive.Shared.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearRanged : MonoBehaviour
{
    public float throwingForce = 40;

    [SerializeField] private Rigidbody spearRigidBody;
    [SerializeField] private Transform testThrowingPosition;

    private bool isLanded = false;

    private void Awake()
    {
        EnableDisableGravityProperties(false);
    }

    private void OnEnable()
    {
        transform.position = testThrowingPosition.position;
        EnableDisableGravityProperties(true);
        spearRigidBody.AddForce(Camera.main.transform.forward * throwingForce, ForceMode.Impulse);
    }

    private void OnDisable()
    {
        EnableDisableGravityProperties(false);
    }

    void Update()
    {
        if (!isLanded)
        {
            transform.rotation = Quaternion.LookRotation(spearRigidBody.velocity);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        EnableDisableGravityProperties(false);
        EventHandler.ExecuteEvent(gameObject, "OnSpearLanded");
    }

    private void EnableDisableGravityProperties(bool isGravityEnabled)
    {
        spearRigidBody.isKinematic = !isGravityEnabled;
        spearRigidBody.useGravity = isGravityEnabled;
        spearRigidBody.constraints = isGravityEnabled ? RigidbodyConstraints.FreezeRotation : RigidbodyConstraints.FreezeAll;
        isLanded = !isGravityEnabled;
    }
}
