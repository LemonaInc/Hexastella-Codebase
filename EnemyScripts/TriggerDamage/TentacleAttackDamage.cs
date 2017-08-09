using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// TENTACLE ATTACK DAMAGE SCRIPT
public class TentacleAttackDamage : MonoBehaviour
{
    public float shakeDuration = 1f;
    public float shakeStrength = 2f;
    public int shakeVibrato = 10;
    public float shakeRandomness = 25;
    public bool shakeFadeOut = true;

    PlayerController player;
    PlayerHealth playerHealth;

    public int tentacleAttackDamage = 50;

    private void Start()
    {
        player = GlobalUtils.Player;
        playerHealth = GlobalUtils.PlayerHealth;
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.name == "PlayerController")
        {
            playerHealth.TakeDamage(tentacleAttackDamage);

            //Camera shake
            Tweener shake = Camera.main.DOShakePosition(shakeDuration, shakeStrength, shakeVibrato, shakeRandomness, shakeFadeOut);
        }
    }
}
