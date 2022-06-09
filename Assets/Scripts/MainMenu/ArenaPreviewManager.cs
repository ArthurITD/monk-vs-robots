using DG.Tweening;
using UnityEngine;
using System;
using System.Collections;

public class ArenaPreviewManager : MonoBehaviour
{ 
    [SerializeField] private MainMenuCameraShaking cameraShaking;
    [SerializeField] private Transform shrineCameraPoint;
    [SerializeField] private Transform avatarChooserCameraPoint;
    [SerializeField] private Transform shrineCameraRotatePoint;
    [SerializeField] private Transform avatarChooserCameraRotatePoint;
    [SerializeField] private float cameraTransitionTime = 1f;

    private Transform cameraTransform;

    private void Start()
    {
        cameraTransform = cameraShaking.transform;
    }

    public Tween SwitchCameraToAvatarChooser()
    {
        return SwitchCameraToPoint(avatarChooserCameraPoint, avatarChooserCameraRotatePoint);
    }

    public Tween SwitchCameraToShrine()
    {
        return SwitchCameraToPoint(shrineCameraPoint, shrineCameraRotatePoint);
    }

    private Tween SwitchCameraToPoint(Transform pointToMove, Transform pointToRotateAround)
    {
        cameraShaking.EnableShaking(false);
        cameraTransform.SetParent(pointToMove);
        cameraTransform.DODynamicLookAt(pointToRotateAround.position, cameraTransitionTime);
        StartCoroutine(RunEnableShakingWithDelay(cameraTransitionTime, true, pointToRotateAround));
        return cameraTransform.DOLocalMove(Vector2.zero, cameraTransitionTime);
    }

    private IEnumerator RunEnableShakingWithDelay(float delay, bool enableShaking, Transform pointToRotateAround)
    {
        yield return new WaitForSeconds(delay);
        cameraShaking.EnableShaking(enableShaking, pointToRotateAround);
    }

}
