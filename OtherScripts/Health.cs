//  Copyright ©017 Jaxon Stevens. All rights reserved.
// Created by Jaxon Stevens and Hillary Ocando
// Created in part of our VFS game final project.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int startingHealth;
    public int currentHealth;

    internal bool isDead;
    internal bool damaged;
    internal bool damageDeflect = false;
    internal bool regainHealth = false;
    public Slider healthSlider;

    internal SwordHitDamage swordHitDamage;

    internal EnemyHealth enemyHealth;

    internal GameObject[] ragdollColliders;

    void Awake()
    {
        swordHitDamage = FindObjectOfType<SwordHitDamage>();
        enemyHealth = FindObjectOfType<EnemyHealth>();
    }

    private void Start()
    {
        currentHealth = startingHealth;
    }

    public virtual void Update()
    {
        if (healthSlider != null)
            healthSlider.value = currentHealth;
    }

    IEnumerator TakeDamageCo()
    {
        damaged = true;
        yield return new WaitForEndOfFrame();
        damaged = false;
    }

    public virtual void TakeDamage(int amountGive)
    {
        StartCoroutine(TakeDamageCo());

        if (!damageDeflect)
            currentHealth -= amountGive;

        if (regainHealth)
        {
            currentHealth += amountGive;
            AkSoundEngine.PostEvent("Play_SFX_ALN_MEC7", gameObject);
        }

        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    public virtual void Death()
    {
        isDead = true;

        foreach (GameObject c in ragdollColliders)
        {
            c.GetComponent<Collider>().enabled = true;
            c.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
