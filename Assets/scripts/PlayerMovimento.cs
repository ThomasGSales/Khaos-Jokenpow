using UnityEngine;

public class Jogador : MonoBehaviour
{

    public float moveSpeed = 5f;
    public Rigidbody rb;
    public Animator animator;
    
    UnityEngine.Vector3 movement;

    
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        movement = new UnityEngine.Vector3(moveX, 0f, moveZ).normalized;

        bool isMoving = movement.magnitude > 0.1f;
        animator.SetBool("mover", isMoving);

        if(isMoving){
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);  
    }
}
