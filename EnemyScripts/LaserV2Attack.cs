// Copyright © 2017 Hexastella. All rights reserved.
// Created by Jaxon Stevens and Hillary Ocando
// Created in part of our VFS game final project.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class LaserV2Attack : MonoBehaviour
{
    private float laserAttackWindup = 3f;

    public float laserChannelTime;

    internal int playerDamageZone;

    EnemyAttackController enemyAttackController;

    internal PlayerController playerController;

    // Location of the eye used for reference 
    public GameObject shootFromEye;

    internal PlayerHealth mainPlayerHealth;

    private Animator anim;

    internal NavMeshAgent nav;

    void Awake()
    {
        nav = FindObjectOfType<NavMeshAgent>();
        playerController = FindObjectOfType<PlayerController>();
        playerDamageZone = LayerMask.GetMask("Shootable");

        anim = GetComponentInChildren<Animator>();

        enemyAttackController = GetComponent<EnemyAttackController>();
        mainPlayerHealth = playerController.GetComponent<PlayerHealth>();
    }

    public IEnumerator EyeLaserAttackEnabledState()
    {
        nav.speed = 0;

        anim.SetBool("StartLaser", true);

        yield return new WaitForSeconds(laserAttackWindup);

        anim.SetBool("StartLaser", false);
        anim.SetBool("ChannelLaser", true);

        yield return new WaitForSeconds(laserChannelTime);

        nav.speed = 10;
        nav.SetDestination(playerController.transform.position);

        yield return null;
    }

    public IEnumerator EyeLaserAttackDisabledState()
    {

        anim.SetBool("ChannelLaser", false);
        yield return null;
    }

}

// END SCRIPT
