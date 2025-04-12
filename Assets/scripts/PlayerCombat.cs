using TMPro;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public float cooldownTime = 0.2f;
    private float nextAttackTime = 0f;

    public GameObject miraPrefab; 
    private GameObject miraInstance; 

    public TextMeshPro attackIndicator;

    public enum AttackType { Pedra, Papel, Tesoura}

    public ParticleSystem efeitoSoco;
    public AudioClip somPedra;
    public AudioClip somPapel;
    public AudioClip somTesoura;

    public Animator animator;

    public CargasUIController cargasUI;



    public GameObject ataquePedraPrefab;
    public GameObject ataquePapelPrefab;
    public GameObject ataqueTesouraPrefab;


    public Transform spawnPoint; 
    public float velocidadeAtaque = 10f;
    public float distanciaMaxima = 5f;

    public int maxCargas = 3;
    private int cargasAtuais;
    public float tempoRecarga = 2f;
    private float tempoUltimoAtaque;


    void Start()
    {
        cargasAtuais = maxCargas;   


        if (miraPrefab != null)
        {
            miraInstance = Instantiate(miraPrefab);
            miraInstance.SetActive(true);   
        }

        cargasUI.AtualizarCargas(cargasAtuais, maxCargas, 0f);


    }


    void Update()
    {
        AtualizarMira();

        if (Time.time >= nextAttackTime){
            if( cargasAtuais > 0){
            
                // Atacar Pedra
                if(Input.GetKeyDown(KeyCode.Mouse0) && !Input.GetKeyDown(KeyCode.Mouse1))
                    Attack(AttackType.Pedra);

                //Atacar Papel
                if(Input.GetKeyDown(KeyCode.Mouse1) && !Input.GetKeyDown(KeyCode.Mouse0))
                    Attack(AttackType.Papel);
                
                //Atacar Tesoura
                if(Input.GetKeyDown(KeyCode.Mouse0) && Input.GetKeyDown(KeyCode.Mouse1))
                    Attack(AttackType.Tesoura);

            }

        }

        if (Time.time - tempoUltimoAtaque >= tempoRecarga && cargasAtuais < maxCargas)
        {
            cargasAtuais++;
            tempoUltimoAtaque = Time.time;
        }

        float progressoRecarga = Mathf.Clamp01((Time.time - tempoUltimoAtaque) / tempoRecarga);
        cargasUI.AtualizarCargas(cargasAtuais, maxCargas, progressoRecarga);

    }


    void Attack(AttackType attack){

        if(cargasAtuais <= 0)
            return; 

        if (animator != null)
            animator.SetTrigger("atacar");

        attackIndicator.text = attack.ToString();
        attackIndicator.color = GetAttackColor(attack);

        GameObject ataquePrefab = GetAttackPrefab(attack);

        if(ataquePrefab != null){

            Vector3 attackPosition = GetMouseWorldPosition();
            attackPosition.y = spawnPoint.position.y;

            Vector3 direction = (attackPosition - spawnPoint.position).normalized;
            Vector3 lookDirection = new Vector3(direction.x, 0f, direction.z); 
            transform.rotation = Quaternion.LookRotation(lookDirection);

            GameObject ataque = Instantiate(ataquePrefab, spawnPoint.position, Quaternion.identity);
            ataque.transform.rotation = GetAttackRotation(attack, direction);

            AtaqueMovimento ataqueMovimento = ataque.AddComponent<AtaqueMovimento>();
            ataqueMovimento.ConfigurarAtaque(direction, velocidadeAtaque, distanciaMaxima, efeitoSoco);

            ataque.transform.localScale *= 1.5f;

            switch (attack)
            {
                case AttackType.Pedra:
                    AudioSource.PlayClipAtPoint(somPedra, spawnPoint.position);
                    break;
                case AttackType.Papel:
                    AudioSource.PlayClipAtPoint(somPapel, spawnPoint.position);
                    break;
                case AttackType.Tesoura:
                    AudioSource.PlayClipAtPoint(somTesoura, spawnPoint.position);
                    break;
            }

            cargasAtuais--;
            tempoUltimoAtaque = Time.time;

        } else {
            Debug.LogError("Prefab de ataque não atribuído no Inspector!");
        }

        nextAttackTime = Time.time + cooldownTime;

        // Limpa após 1 segundo
        Invoke(nameof(ClearIndicator), 1f); 

    }


    void ClearIndicator(){
        attackIndicator.text = "";
    }


    Color GetAttackColor(AttackType attack){
        switch(attack){
            case AttackType.Pedra: return Color.red;
            case AttackType.Papel: return Color.blue;
            case AttackType.Tesoura: return Color.green;
            default: return Color.white;
        }
    }


    GameObject GetAttackPrefab(AttackType attack){
        switch (attack){
            case AttackType.Pedra: return ataquePedraPrefab;
            case AttackType.Papel: return ataquePapelPrefab;
            case AttackType.Tesoura: return ataqueTesouraPrefab;
            default: return null;
        }
    }


    Quaternion GetAttackRotation(AttackType attack, Vector3 direction)
    {

        Quaternion baseRotation = Quaternion.LookRotation(direction, Vector3.up);

        switch (attack)
        {
            case AttackType.Pedra:
                return baseRotation * Quaternion.Euler(90, 0, 0); // Pedra deitada
            case AttackType.Papel:
                return baseRotation; // Papel segue a rotação normal
            case AttackType.Tesoura:
                return baseRotation * Quaternion.Euler(0, 0, 0); // Tesoura deitada
            default:
                return baseRotation;
        }
    }


    Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        Vector3 playerHeight = new Vector3(0, transform.position.y, 0);
        Plane groundPlane = new Plane(Vector3.up, playerHeight); 
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            return ray.GetPoint(rayDistance); 
        }

        return spawnPoint.position + transform.forward * distanciaMaxima;
    }


    void AtualizarMira() {

    if (miraInstance != null)
    {
        Vector3 targetPosition = GetMouseWorldPosition();
        Vector3 direction = (targetPosition - transform.position).normalized;

        Vector3 miraPosition = transform.position + direction * Mathf.Min(Vector3.Distance(transform.position, targetPosition), distanciaMaxima);

        miraPosition.y = 0.1f; 

        miraInstance.transform.position = miraPosition;
        miraInstance.transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    }
}
