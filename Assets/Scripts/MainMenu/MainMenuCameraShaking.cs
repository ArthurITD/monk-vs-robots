using UnityEngine;

public class MainMenuCameraShaking : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speedModifier = 10f;
    [SerializeField] private float timeToRotateOneSide = 5f;
    private Vector3 point;

    private float currentRotateTime;
    private bool allowShaking;

    private void Start()
    {
        point = target.position;
        transform.LookAt(point);
        EnableShaking(true);
    }

    private void Update()
    {
        if (allowShaking)
        {
            currentRotateTime += Time.deltaTime;
            if (currentRotateTime >= timeToRotateOneSide)
            {
                currentRotateTime = 0f;
                speedModifier = -speedModifier;
            }
            transform.RotateAround(point, Vector3.up, Time.deltaTime * speedModifier);
        }
    }

    public void EnableShaking(bool enable, Transform rotateTargetPoint = null)
    {
        if (rotateTargetPoint != null)
        {
            target = rotateTargetPoint;
            point = target.position;
            transform.LookAt(point);
        }
        allowShaking = enable;
        
    }
}
