//  HexagonBoost.CS
//  Copyright © 2017 Jaxon Stevens. All rights reserved.
// Created by Jaxon Stevens and Hillary Ocando
// Created in part of our VFS game final project.

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HexagonBoost : Hexagons
{
    public int boostDamage;
    public int boostSpeed = 50;

    private float originalSpeed;
    private int originalDamage;

    public SwordHitDamage playerAttack;
    public Image boostActiveImage;

    private PlayerController player;

    internal HexagonSelector hexagonSelector;

    public void Start()
    {
        player = GlobalUtils.Player;
        originalDamage = playerAttack.swordDamage;
        originalSpeed = player.speed;
        hexagonSelector = FindObjectOfType<HexagonSelector>();
        hexagonTime = 10f;
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.name == "PlayerController" && Input.GetButtonDown("Y") && isActive && !hasBeenActivated)
        {
            StartCoroutine(ActivateHexagon());
        }
    }

    public override IEnumerator ActivateHexagon()
    {
        yield return base.ActivateHexagon();

        playerAttack.swordDamage += boostDamage;
        player.speed += boostSpeed;
        player.isBoosting = true;

        boostActiveImage.enabled = true;

        if (!player.isInvisible)
            TurnBoostRed();

        yield return new WaitForSeconds(hexagonTime);

        playerAttack.swordDamage = originalDamage;
        player.speed = originalSpeed;
        player.isBoosting = false;

        boostActiveImage.enabled = false;

        if (!player.isInvisible)
            TurnNormal();

        hasBeenActivated = false;

        hexagonSelector.BoostHexagonHasExpired();
    }

}

