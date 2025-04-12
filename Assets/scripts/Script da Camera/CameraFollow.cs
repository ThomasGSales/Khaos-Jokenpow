using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform Target; // player
    [SerializeField]
    private Transform CameraTransform; 
    [SerializeField]
    private UnityEngine.Vector3 OffSet; 
    [SerializeField]
    private float smoothTime;

    private UnityEngine.Vector3 velocity = UnityEngine.Vector3.zero;

    private void LateUpdate()
    {
        UnityEngine.Vector3 targetPosition = Target.position + OffSet;
        CameraTransform.position = UnityEngine.Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        transform.LookAt(Target);
    }
}
