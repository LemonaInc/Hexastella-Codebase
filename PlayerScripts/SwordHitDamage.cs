using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SwordHitDamage : MonoBehaviour
{
    public int swordDamage;
    internal int originalSwordDamage;
    public int increasedSwordDamage;

    internal bool enemyHitBool;
    private EnemyAttackController enemy;
    internal SmallEnemyAttackController smallEnemy;

    SmallBossHealth[] smallBossesHealth;

    private EnemyHealth enemyHealth;
    internal SmallBossHealth smallBossHealth;
    private GameObject player;
    private List<Enemy> EnemiesHitThisAttack;

    private void Start()
    {
        EnemiesHitThisAttack = new List<Enemy>();
        enemy = GlobalUtils.Boss;
        enemyHealth = GlobalUtils.BossHealth;
        enemy = FindObjectOfType<EnemyAttackController>();
        smallEnemy = FindObjectOfType<SmallEnemyAttackController>();
        smallBossHealth = FindObjectOfType<SmallBossHealth>();
        smallBossesHealth = FindObjectsOfType<SmallBossHealth>();
        player = GetComponentInParent<PlayerController>().gameObject;
        gameObject.SetActive(false);

        originalSwordDamage = swordDamage;
    }

    void OnEnable()
    {
        if (EnemiesHitThisAttack != null)
            EnemiesHitThisAttack.Clear();
    }

    void OnDisable()
    {
        EnemiesHitThisAttack.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemyHit = other.transform.root.GetComponent<Enemy>();


        if (enemyHit != null && !EnemiesHitThisAttack.Contains(enemyHit))
        {

            EnemiesHitThisAttack.Add(enemyHit);
            enemyHitBool = false;

            enemyHit.GetComponent<Health>().TakeDamage(swordDamage);
            VoiceOverSoundManager.instance.PlayPlayerVoiceMaybe("Play_G64_Inflicts_Damage", 30f);

        }
    }
}

