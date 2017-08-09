using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTriggerCollider : MonoBehaviour
{
    public int laserAttackDamage = 20;
    
    internal LaserV2Attack laserV2Attack;

    private PlayerHealth mainPlayerHealth;

    void Start()
    {
        laserV2Attack = FindObjectOfType<LaserV2Attack>();
        mainPlayerHealth = FindObjectOfType<PlayerHealth>();
    }

    public void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.name == "PlayerController")
        {
            mainPlayerHealth.TakeDamage(laserAttackDamage);
        }
    }

}
