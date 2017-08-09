using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : Health
{
    public float flashSpeed = 10f;

    public Image damageImage;
    public Color flashColor = new Color(1f, 0f, 0f, 0.1f);
    public GameObject sword;

    Coroutine DamageOverTimeCoroutine;
    Animator anim;

    bool isCriticallyWounded;

    private void Start()
    {
        ragdollColliders = GameObject.FindGameObjectsWithTag("PlayerRagdoll");

        anim = PlayerController.instance.anim;
    }

    public override void Update()
    {
        base.Update();

        if (currentHealth < 50 && !isCriticallyWounded)
        {
            VoiceOverSoundManager.instance.PlayPlayerVoiceMaybe("Play_AAAP_TUT4");
            isCriticallyWounded = true;
        }

        //Flashes a red image when player is getting damaged
        if (damaged && !damageDeflect)
        {
            damageImage.color = flashColor;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
    }

    public override void TakeDamage(int amount)
    {
        if (isDead)
            return;

        base.TakeDamage(amount);

        AkSoundEngine.PostEvent("Play_Takes_Damage", gameObject);
        VoiceOverSoundManager.instance.PlayPlayerVoiceMaybe("Play_G64_Gets_Hurt", 60f);
    }

    public void TakeAoeDamage(int amount)
    {
        if (DamageOverTimeCoroutine != null)
        {
            StopCoroutine(DamageOverTimeCoroutine);
            DamageOverTimeCoroutine = null;
        }

        DamageOverTimeCoroutine = StartCoroutine(TakeAoeDamagePerSecond(amount));
    }

    public void StopDamageOverTime()
    {
        if (DamageOverTimeCoroutine != null)
        {
            StopCoroutine(DamageOverTimeCoroutine);
            DamageOverTimeCoroutine = null;
        }
    }

    private IEnumerator TakeAoeDamagePerSecond(int amountPerTick)
    {
        while (true)
        {
            base.TakeDamage(amountPerTick);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public override void Death()
    {
        base.Death();

         // Destroy all Mini Bosses when the main boss dies
      GameObject[] killMiniBosses = GameObject.FindGameObjectsWithTag("MiniBoss");
      foreach(GameObject killMiniBoss in killMiniBosses)
      Destroy(killMiniBoss);

        anim.enabled = false;

        AkSoundEngine.PostEvent("Stop_Player_Idle", gameObject);

        AkSoundEngine.PostEvent("Play_G64_Dies", gameObject);

        sword.SetActive(false);

        PlayerController.instance.canAttack = false;
        PlayerController.instance.canMove = false;

        PlayerController.instance.rb.velocity = Vector3.zero;

        GameManager.instance.playerLose = true;

        PlayerController.instance.enabled = false;
    }

}
