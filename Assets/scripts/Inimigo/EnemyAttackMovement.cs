using UnityEngine;

public class EnemyAttackMovement : MonoBehaviour
{
    private Vector3 direction;
    private float speed;
    private float maxDistance;
    private Vector3 startPosition;

    public ParticleSystem impactoVFX;

    void Start()
    {
        // Animação de punch style
        LeanTween.scale(gameObject, transform.localScale * 1.2f, 0.1f).setEasePunch();
    }

    public void Configure(Vector3 dir, float spd, float distancia, ParticleSystem efeito)
    {
        direction = dir.normalized;
        speed = spd;
        maxDistance = distancia;
        impactoVFX = efeito;
        startPosition = transform.position;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
        {
            DestroyWithEffect();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jogador atingido pelo inimigo!");
            DestroyWithEffect();
        }
    }

    void DestroyWithEffect()
    {
        if (impactoVFX != null)
            Instantiate(impactoVFX, transform.position, Quaternion.identity);

        LeanTween.scale(gameObject, Vector3.zero, 0.3f).setEaseInBack().setOnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}