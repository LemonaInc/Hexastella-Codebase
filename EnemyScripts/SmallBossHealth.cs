//  Copyright © 2017 Hexastella. All rights reserved.
// Created by Jaxon Stevens and Hillary Ocando
// Created in part of our VFS game final project.

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SmallBossHealth : Health
{
    public ParticleSystem bloodGalore;

    private SmallEnemyAttackController smallEnemy;

    private Animator smallAnim;

    private CapsuleCollider miniBossCollider;

    private BossEmerge bossEmerge;

    private void Start()
    {
        smallEnemy = FindObjectOfType<SmallEnemyAttackController>();
        smallAnim = smallEnemy.bossAnimator;
        ragdollColliders = GameObject.FindGameObjectsWithTag("MiniBossRagdoll");
        miniBossCollider = smallEnemy.GetComponent<CapsuleCollider>();
        bossEmerge = FindObjectOfType<BossEmerge>();
    }

    public override void Update()
    {
        base.Update();

        // Destroy One Mini Boss
        if (Input.GetKeyDown(KeyCode.S))
        {

            GameObject destroyOneMiniBoss = GameObject.FindGameObjectWithTag("MiniBoss");
            Destroy(destroyOneMiniBoss);

        }

        if (Input.GetKeyDown(KeyCode.D))
        {

            GameObject[] destroyBosses = GameObject.FindGameObjectsWithTag("MiniBoss");
            foreach (GameObject destroyBoss in destroyBosses)
            {
                Destroy(destroyBoss);

            }

        }

    }

    public override void TakeDamage(int amount)
    {
        if (isDead)
            return;

        base.TakeDamage(amount);

        bloodGalore.Play();
        AkSoundEngine.PostEvent("Play_SFX_ALN_HIT", gameObject);
    }

    public override void Death()
    {
        base.Death();
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        smallAnim.enabled = false;
        Destroy(smallEnemy);

        StartCoroutine(WaitForSecondsBeforeDestroying());
    }

    public IEnumerator WaitForSecondsBeforeDestroying()
    {
        yield return new WaitForSeconds(2f);

        miniBossCollider.enabled = false;

        foreach (GameObject c in ragdollColliders)
        {
            c.GetComponent<Collider>().enabled = false;
        }

        yield return new WaitForSeconds(2f);

        Destroy(bossEmerge.smallBoss.gameObject);

        
    }


}
