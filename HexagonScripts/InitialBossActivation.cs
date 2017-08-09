// Copyright © 2017 Hexastella. All rights reserved.
// Created by Jaxon Stevens and Hillary Ocando
// Created in part of our VFS game final project.
using UnityEngine;

public class InitialBossActivation : MonoBehaviour
{
    internal ParticleSystem playerArenaEnterTriggerParticles;

    PlayerController player;
    EnemyAttackController[] enemyAttackControllers;
   // SmallEnemyAttackController[] smallEnemyAttackControllers;

    void Awake()
    {
        enemyAttackControllers = FindObjectsOfType<EnemyAttackController>();
      //  smallEnemyAttackControllers = FindObjectsOfType<SmallEnemyAttackController>();

        player = FindObjectOfType<PlayerController>();

        foreach (EnemyAttackController enemy in enemyAttackControllers)
            enemy.activateBossStateMachineBool = false;

       // foreach (SmallEnemyAttackController smallEnemy in smallEnemyAttackControllers)

        playerArenaEnterTriggerParticles = GetComponent<ParticleSystem>();
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "PlayerController")
        {
            // Play Particles when player enters the arena
            playerArenaEnterTriggerParticles.Play();
            
            foreach (EnemyAttackController enemy in enemyAttackControllers)
                // Set the player target to ! null
                enemy.target = player.transform;

          // foreach (SmallEnemyAttackController smallEnemy in smallEnemyAttackControllers) 
           // Set the player target to ! null
            //smallEnemy.target = player.transform;

            
            playerArenaEnterTriggerParticles.Stop();
        }

    }

}
