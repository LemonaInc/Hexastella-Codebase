using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEWaveAttackDamage : MonoBehaviour
{
    PlayerController player;
    PlayerHealth playerHealth;
    AOEWaveAttack wave;

    public int targetWaveCircleDamage;
    bool isDealingDamage;

    private void Start()
    {
        player = GlobalUtils.Player;
        playerHealth = GlobalUtils.PlayerHealth;
        wave = FindObjectOfType<AOEWaveAttack>();
    }

    private void BeginAoeDamage()
    {
        isDealingDamage = true;

        playerHealth.TakeAoeDamage(targetWaveCircleDamage);
    }

    private void EndAoeDamage()
    {
        isDealingDamage = false;

        playerHealth.StopDamageOverTime();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!isDealingDamage && col.gameObject.name == "PlayerController")
        {
            BeginAoeDamage();

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isDealingDamage && other.gameObject.name == "PlayerController")
            EndAoeDamage();
    }

    private void OnDisable()
    {
        if (isDealingDamage)
            EndAoeDamage();
    }
}