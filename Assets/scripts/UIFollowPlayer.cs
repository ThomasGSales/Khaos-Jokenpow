using UnityEngine;

public class UIFollowPlayer : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public bool faceCamera = true;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Segue o anchor com offset
        transform.position = target.position + offset;

        // Mantém o HUD sempre de frente pra câmera
        if (faceCamera && cam != null)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
        }
    }
}