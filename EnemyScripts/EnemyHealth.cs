//  Copyright © 2017 Hexastella. All rights reserved.
// Created by Jaxon Stevens and Hillary Ocando
// Created in part of our VFS game final project.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : Health
{
    public bool damagePOPActive;

    public Image enemyDamageImage;
    public Color flashColor = new Color(1f, 0f, 0f, 0.1f);
    public ParticleSystem bloodGalore;
    public GameObject bossHealth;
    public Image damagePopText;
    public Text damagePopDisplayNumber;

    private EnemyAttackController enemy;
    private Animator anim;

    private void Start()
    {
        enemy = GlobalUtils.Boss;
        anim = enemy.anim;

        ragdollColliders = GameObject.FindGameObjectsWithTag("BossRagdoll");
    }

    public override void Update()
    {
        base.Update();
    }

    public override void TakeDamage(int amount)
    {
        if (isDead)
            return;

        StartCoroutine(enemyHealth.DamagePOP());

        if (swordHitDamage.enemyHitBool == false)
        {
            base.TakeDamage(amount);

            bloodGalore.Play();

            AkSoundEngine.PostEvent("Play_G64_Deal_Damage", gameObject);

            VoiceOverSoundManager.instance.PlayEnemyVoiceMaybe("Play_Xenandros_Gets_Hurt", 30f);
            AkSoundEngine.PostEvent("Play_SFX_ALN_HIT", gameObject);

            FindObjectOfType<SwordHitDamage>().enemyHitBool = true;
        }
    }

    public override void Death()
    {
        base.Death();

        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<EnemyAttackController>().enabled = false;

        anim.enabled = false;

        bossHealth.SetActive(false);

        // Destroy all Mini Bosses when the main boss dies
        GameObject[] killMiniBosses = GameObject.FindGameObjectsWithTag("MiniBoss");
        foreach (GameObject killMiniBoss in killMiniBosses)
            Destroy(killMiniBoss);

        VoiceOverSoundManager.instance.PlayEnemyVoiceMaybe("Play_Xenandros_Death");
        AkSoundEngine.PostEvent("Play_Alien_Death", gameObject);
        AkSoundEngine.PostEvent("Stop_Alien_Hover", gameObject);

        Destroy(enemy);

        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;

        GameManager.instance.playerWin = true;
    }

    public IEnumerator DamagePOP()
    {
        damagePOPActive = true;

        if (damagePOPActive == true)
        {
            yield return new WaitForSeconds(0.2f);
            damagePopText.enabled = true;
            damagePopDisplayNumber.enabled = true;
        }

        yield return new WaitForSeconds(0.2f);

        damagePOPActive = false;

        if (damagePOPActive == false)
        {
            damagePopText.enabled = false;
            damagePopDisplayNumber.text = (swordHitDamage.swordDamage * 20).ToString();
            damagePopDisplayNumber.enabled = false;
            yield return null;
        }
    }

}
