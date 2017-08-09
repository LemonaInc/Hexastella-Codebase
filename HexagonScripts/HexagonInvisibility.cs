//  HexagonDamage.CS
//  Copyright © 2017 Jaxon Stevens. All rights reserved.
// Created by Jaxon Stevens and Hillary Ocando
// Created in part of our VFS game final project.

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HexagonInvisibility : Hexagons
{
    private PlayerController player;
    private EnemyAttackController enemy;
    internal HexagonSelector hexagonSelector;

    private EnemyHealth enemyHealth;

    public Image InvisActiveImage;

    public bool playerIsInvisible;

    private Coroutine HexActivationCo;

    public void Start()
    {
        player = GlobalUtils.Player;
        enemy = GlobalUtils.Boss;
        hexagonSelector = FindObjectOfType<HexagonSelector>();
        enemyHealth = FindObjectOfType<EnemyHealth>();
        hexagonTime = 10f;
    }

    void OnTriggerStay(Collider incomingCollider)
    {
        if (incomingCollider.transform.root == player.transform.root && Input.GetButtonDown("Y") && isActive && !hasBeenActivated)
        {
            HexActivationCo = StartCoroutine(ActivateHexagon());
        }
    }

    public override IEnumerator ActivateHexagon()
    {
        // Set the playerIsInvisible bool to true
        playerIsInvisible = true;

        StartCoroutine(CheckForDamagedBoss());

        yield return base.ActivateHexagon();

        player.isInvisible = true;

        InvisActiveImage.enabled = true;

        TurnInvisible();

        enemy.target = null;

        yield return new WaitForSeconds(hexagonTime);

        EndInvisibility();
    }

    IEnumerator CheckForDamagedBoss()
    {
        while (playerIsInvisible)
        {
            if (enemyHealth.damaged)
            {
                if (HexActivationCo != null)
                {
                    StopCoroutine(HexActivationCo);
                    HexActivationCo = null;
                }

                EndInvisibility();
            }

            yield return null;
        }
    }

    void EndInvisibility()
    {
        if (player.isBoosting)
            TurnBoostRed();
        else
            TurnNormal();

        player.isInvisible = false;

        InvisActiveImage.enabled = false;

        hexagonSelector.InvisibleHexagonHasExpired();

        playerIsInvisible = false;

        hasBeenActivated = false;

        enemy.target = player.transform;
    }

}
