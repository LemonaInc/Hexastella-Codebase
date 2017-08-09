// Copyright © 2017 Jaxon Stevens. All rights reserved.
// Created by Jaxon Stevens and Hillary Ocando
// Created in part of our VFS game final project.
using System.Collections;
using UnityEngine;

// THIS SCRIPT DOES:
// Start the box range attack state where the box element will appear when the boss is about to use the range attack
// In this attack if the player is in the defined radius of the boss then spawn an object and if the player gets hit by that object then deal damage to the player

public class TentacleAttack : MonoBehaviour
{
    public float tentacleAttackWindup;
    public float tentacleAttackHold;
    public float tentacleMaxScale = 20f;

    public EnemyAttackController enemyAttackController;
    private PlayerController player;
    private EnemyFrontTentacle[] tentacles;
    private SkinnedMeshRenderer[] tentacleRend;
    private Animator anim;

    void Awake()
    {
        enemyAttackController = GetComponent<EnemyAttackController>();
        anim = GetComponentInChildren<Animator>();
        tentacles = GetComponentsInChildren<EnemyFrontTentacle>();
        player = FindObjectOfType<PlayerController>();

        tentacleRend = new SkinnedMeshRenderer[tentacles.Length];
        for (int i = 0; i < tentacles.Length; i++)
            tentacleRend[i] = tentacles[i].GetComponent<SkinnedMeshRenderer>();        
    }
    
    public IEnumerator ActivateTentacleAttackState()
    {
        // TENTACLE ATTACK WINDUP
        anim.SetBool("StartFrontSlam", true);

        yield return new WaitForSeconds(tentacleAttackWindup);

        anim.SetBool("StartFrontSlam", false);
        anim.SetBool("ChannelFrontSlam", true);

        enemyAttackController.nav.SetDestination(player.transform.position);

        float attackTimerCountdown = 0;
        float originalTentacleScale = 1f;

        while (attackTimerCountdown < tentacleAttackHold)
        {
            float percentageToAttackComplete = attackTimerCountdown / tentacleAttackHold;
            float currentTentacleScale = Mathf.Lerp(originalTentacleScale, tentacleMaxScale, percentageToAttackComplete);

            //Increase scale of tentacles when doing slam
            foreach (SkinnedMeshRenderer renderer in tentacleRend)
                renderer.material.SetFloat("_node_2346", currentTentacleScale);
            
            attackTimerCountdown += Time.deltaTime;
            yield return null;
        }
        
        anim.SetBool("ChannelFrontSlam", false);
        
        float windDownTimer = 0f;
        while (windDownTimer < 2f)
        {
            float currentTentacleScale = Mathf.Lerp(tentacleMaxScale, 1f, windDownTimer/2f);

            foreach (SkinnedMeshRenderer renderer in tentacleRend)
                renderer.material.SetFloat("_node_2346", currentTentacleScale);

            windDownTimer += Time.deltaTime;
            yield return null;
        }
        
        yield return null;
    }
    
} // END SCRIPT
