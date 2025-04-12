using UnityEngine;

public class AtaqueMovimento : MonoBehaviour
{
    public GameObject efeitoDesaparecerPrefab;

    private Vector3 direction;
    private float velocidade;
    private float distanciaMaxima;
    private Vector3 startPosition;
    public ParticleSystem efeitoImpacto;

    void Start(){
        // Punch Scale estilo "El Primo"
        LeanTween.scale(gameObject, transform.localScale * 1.3f, 0.1f).setEasePunch();
    }

    public void ConfigurarAtaque(Vector3 dir, float vel, float distancia, ParticleSystem efeito)
    {
        direction = dir;
        velocidade = vel;
        distanciaMaxima = distancia;
        startPosition = transform.position;
        efeitoImpacto = efeito;

    }

    void Update()
    {
        
        transform.position += direction * velocidade * Time.deltaTime;


        if (Vector3.Distance(startPosition, transform.position) >= distanciaMaxima)
        {
            DestruirComEfeito();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Inimigo"))
        {
            Debug.Log("Inimigo atingido!");
        }
    }

    void DestruirComEfeito() {

    if (efeitoImpacto != null){
        Instantiate(efeitoImpacto, transform.position, Quaternion.identity);
    }

    enabled = false;

    LeanTween.scale(gameObject, Vector3.zero, 0.3f).setEaseInBack().setOnComplete(() => {
        Destroy(gameObject);
    });

    }

    
}
