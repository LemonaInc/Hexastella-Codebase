using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAttackDamage : MonoBehaviour {

    PlayerController player;
    PlayerHealth playerHealth;
    public int rearSpinAttackDamage = 30;

    private void Start()
    {
        player = GlobalUtils.Player;
        playerHealth = GlobalUtils.PlayerHealth;
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.name == "PlayerController")
            playerHealth.TakeDamage(rearSpinAttackDamage);
    }
}
