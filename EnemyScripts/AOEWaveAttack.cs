//  Copyright © 2017 Hexastella. All rights reserved.
// Created by Jaxon Stevens and Hillary Ocando
// Created in part of our VFS game final project.
using System.Collections;
using UnityEngine;

public class AOEWaveAttack : MonoBehaviour
{
    public float aOEWindUp = 2f;
    public float aOEHold = 3f;

    public GameObject enemy;
    internal EnemyAttackController enemyAttackController;
    internal EnemyHealth bossHealth;

    private PlayerController player;
    private MeshRenderer glyphMeshRend;
    internal Animator anim;

    public GameObject waveAttackCircle;
    public Collider waveDamageTriggerCollider;
    public ParticleSystem[] aoePuffs;

    public float damageDistance = 120f;

    public bool waveActive;

    void Awake()
    {
        player = FindObjectOfType<PlayerController>();

        enemyAttackController = GetComponent<EnemyAttackController>();

        anim = GetComponentInChildren<Animator>();

        bossHealth = GetComponent<EnemyHealth>();

        glyphMeshRend = waveAttackCircle.GetComponent<MeshRenderer>();
    }

    public int[] aoeBossHealthValues = new int[3] { 790, 320, 120 };
    private int lastAttackHealthValueIndex = 0;

    public void checkAoeAttack()
    {

        for (int i = 0; i < aoeBossHealthValues.Length; ++i)

            if (bossHealth.currentHealth < aoeBossHealthValues[i])
            {
                StartCoroutine(AOEWaveAttackState());

            }


    }

    public IEnumerator AOEWaveAttackState()
    {
        if (bossHealth.currentHealth < aoeBossHealthValues[lastAttackHealthValueIndex]
            && Vector2.Distance(waveAttackCircle.transform.position, player.transform.position) <= damageDistance)
        {
            ++lastAttackHealthValueIndex;
        }
        else
        {
            yield break;
        }
        // create a for loop the AoeBossHealth values defined above in the array  

        // Check the distance between the boss and the player and if the distance is less or equal to the damage distance and the current boss health is equal or less then one of the values then trigger the aoe attack
        //if (Vector2.Distance(waveAttackCircle.transform.position, player.transform.position) <= damageDistance)
        {
            waveActive = true;

            float waitForSecondsTime = 1f;
            enemyAttackController.anim.SetBool("StartAOE", true);

            yield return new WaitForSeconds(waitForSecondsTime);

            enemyAttackController.anim.SetBool("StartAOE", false);
            enemyAttackController.anim.SetBool("ChannelAOE", true);

            waveAttackCircle.SetActive(true);

            yield return new WaitForSeconds(aOEWindUp);

            enemyAttackController.anim.SetBool("ChannelAOE", false);

            enemyAttackController.anim.SetBool("HoldAOE", true);

            foreach (ParticleSystem puffs in aoePuffs)
                puffs.Play();

            //Slerp the distance of the shader's glowing arrows _Duration
            float aoeAttackStartup = 0f;

            while (aoeAttackStartup < aOEHold)
            {
                float percToComplete = aoeAttackStartup / aOEHold;
                float currentArrowDuration = Mathf.Lerp(1f, 0f, percToComplete);

                glyphMeshRend.material.SetFloat("_Duration", currentArrowDuration);

                aoeAttackStartup += Time.deltaTime;
                yield return null;
            }

            waveDamageTriggerCollider.enabled = true;

            yield return new WaitForSeconds(3f);

            waveDamageTriggerCollider.enabled = false;

            foreach (ParticleSystem puffs in aoePuffs)
                puffs.Stop();

            //Slerp back to nothing
            float aoeComeBack = 0f;
            while (aoeComeBack < 2f)
            {
                float currentAOEDurationVal = Mathf.Lerp(0f, 1f, aoeComeBack / 2f);

                glyphMeshRend.material.SetFloat("_Duration", currentAOEDurationVal);

                aoeComeBack += Time.deltaTime;
                yield return null;
            }

            waveAttackCircle.SetActive(false);

            enemyAttackController.anim.SetBool("HoldAOE", false);

            waveActive = false;

            yield return null;
        }
    }

}

