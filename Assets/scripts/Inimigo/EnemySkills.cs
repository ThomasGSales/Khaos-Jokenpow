using UnityEngine;
using System.Collections;

public class EnemySkills : MonoBehaviour
{
    public enum AttackType { Pedra, Papel, Tesoura }

    [Header("Ataques")]
    public Transform attackSpawnPoint;
    public GameObject pedraPrefab;
    public GameObject papelPrefab;
    public GameObject tesouraPrefab;

    [Header("Configurações")]
    public int maxCharges = 3;
    public float rechargeTime = 2f;
    public float tempoEntreAtaques = 2f;
    public float ataqueRange = 7f;

    private int currentCharges;
    private bool isRecharging = false;
    private float proximoAtaque = 0f;

    private Transform player;

    void Start()
    {
        currentCharges = maxCharges;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (Time.time >= proximoAtaque && currentCharges > 0 && distanceToPlayer <= ataqueRange)
        {
            EscolherEMandarAtaque(distanceToPlayer);
            currentCharges--;
            proximoAtaque = Time.time + tempoEntreAtaques;

            if (!isRecharging)
                StartCoroutine(RecarregarCargas());
        }
    }

    void EscolherEMandarAtaque(float distance)
    {
        GameObject prefab = null;
        AttackType chosenType;

        // Lógica básica de escolha (pode ser turbinada com IA mais tarde)
        if (distance < 3f)
            chosenType = AttackType.Pedra; // Curto alcance
        else if (distance < 6f)
            chosenType = AttackType.Tesoura; // Médio
        else
            chosenType = AttackType.Papel; // Longo

        switch (chosenType)
        {
            case AttackType.Pedra: prefab = pedraPrefab; break;
            case AttackType.Papel: prefab = papelPrefab; break;
            case AttackType.Tesoura: prefab = tesouraPrefab; break;
        }

        if (prefab != null)
        {
            GameObject ataqueGO = Instantiate(prefab, attackSpawnPoint.position, Quaternion.LookRotation(player.position - transform.position));
            EnemyAttackMovement movement = ataqueGO.GetComponent<EnemyAttackMovement>();

            if (movement != null)
            {
                Vector3 dir = (player.position - transform.position).normalized;
                movement.Configure(dir, 10f, 5f, movement.impactoVFX);
            }
        }
    }

    IEnumerator RecarregarCargas()
    {
        isRecharging = true;

        while (currentCharges < maxCharges)
        {
            yield return new WaitForSeconds(rechargeTime);
            currentCharges++;
        }

        isRecharging = false;
    }
    
}