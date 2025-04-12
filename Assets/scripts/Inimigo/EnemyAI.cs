using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 1f;
    public int damage = 1;

    private float lastAttackTime = 0f;
    private Transform player;
    private NavMeshAgent agent;

    public float moveSpeed = 5f;
    public Animator animator;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            agent.SetDestination(player.position);

            if (distance <= attackRange && Time.time - lastAttackTime >= attackCooldown)
            {
                AttackPlayer();
                lastAttackTime = Time.time;
            }
        }


        Vector3 velocity = agent.velocity;
        bool isMoving = velocity.magnitude > 0.1f;
        animator.SetBool("mover", isMoving);

        if (isMoving)
        {
            Quaternion targetRotation = Quaternion.LookRotation(velocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    void AttackPlayer()
    {
        Debug.Log("Inimigo atacou o jogador!");
        // Aqui você pode chamar um método no script do jogador, tipo:
        // player.GetComponent<PlayerHealth>().TakeDamage(damage);
    }

    
}
