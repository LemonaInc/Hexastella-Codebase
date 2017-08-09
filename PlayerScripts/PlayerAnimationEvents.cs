using DG.Tweening;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    public float shakeDuration = 0.5f;
    public float shakeStrength = 1.5f;
    public int shakeVibrato = 5;
    public float shakeRandomness = 5;
    public bool shakeFadeOut = true;
    
    public GameObject swordHitDamage;
    public GameObject sheathedSword;
    public GameObject drawnSword;
    public ParticleSystem swordSwosh;
    public ParticleSystem swordSwosh2;

    private PlayerController player;
    private SwordHitDamage swordDamage;
    private HexagonBoost hexBoost;

    private void Awake()
    {
        player = GlobalUtils.Player;
        swordDamage = FindObjectOfType<SwordHitDamage>();
        hexBoost = FindObjectOfType<HexagonBoost>();
    }

    //Attack box activation
    public void AttackActive()
    {
        swordHitDamage.SetActive(true);
    }

    public void AttackDisabled()
    {
        swordHitDamage.SetActive(false);
    }

    //Player movement
    public void DisablePlayerMovement()
    {
        PlayerController.instance.canMove = false;
    }

    public void EnablePlayerMovement()
    {
        PlayerController.instance.canMove = true;
    }
    
    //Step sounds
    public void OnWalkEvent()
    {
        AkSoundEngine.SetSwitch("Feet_Intention", "Run", gameObject);
        AkSoundEngine.SetSwitch("Surface", "Clanky", gameObject);
        uint feetEvent = AkSoundEngine.PostEvent("Play_G64_Feet", gameObject);
    }

    public void OnSlowWalkEvent()
    {
        AkSoundEngine.SetSwitch("Feet_Intention", "Walk", gameObject);
        AkSoundEngine.SetSwitch("Surface", "Metal", gameObject);
        uint feetEvent = AkSoundEngine.PostEvent("Play_G64_Feet", gameObject);
    }

    //Slash sounds
    public void OnComboOne()
    {
        swordSwosh.Play();
        AkSoundEngine.PostEvent("Play_SFX_G64_Combo_01", gameObject);
    }

    public void OnComboTwo()
    {
        swordSwosh.Play();
        AkSoundEngine.PostEvent("Play_SFX_G64_Combo_02", gameObject);
    }

    public void OnComboThree()
    {
        swordSwosh.Play();
        AkSoundEngine.PostEvent("Play_SFX_G64_Combo_03", gameObject);
    }

    public void OnComboFour()
    {
        swordSwosh.Play();
        AkSoundEngine.PostEvent("Play_SFX_G64_Combo_04", gameObject);
    }

    public void OnComboFive()
    {
        swordSwosh2.Play();
        AkSoundEngine.PostEvent("Play_SFX_G64_Combo_05", gameObject);
    }

    public void ScreenShake()
    {
        Tweener shake = Camera.main.DOShakePosition(shakeDuration, shakeStrength, shakeVibrato, shakeRandomness, shakeFadeOut);
    }

    //Higher attack damage
    public void ExtraDamage()
    {
        swordDamage.swordDamage += swordDamage.increasedSwordDamage;
    }

    public void OriginalDamage()
    {
        swordDamage.swordDamage = swordDamage.originalSwordDamage;
    }

    //Slow movement when attacking
    public void SlowSpeed()
    {
        if (!PlayerController.instance.isBoosting)
            PlayerController.instance.speed -= PlayerController.instance.slowSpeed;
    }

    public void OriginalSpeed()
    {
        if (!PlayerController.instance.isBoosting)
            PlayerController.instance.speed = PlayerController.instance.originalSpeed;
    }

    //Draw sword
    public void SwordSheathe()
    {
        sheathedSword.SetActive(true);
        drawnSword.SetActive(false);
    }

    public void SwordDrawn()
    {
        sheathedSword.SetActive(false);
        drawnSword.SetActive(true);
        AkSoundEngine.SetSwitch("Sword", "Draw_Sword", gameObject);
        AkSoundEngine.PostEvent("Play_SFX_Sword", gameObject);
    }

    //Combo moves
    public void OnAnimationEnd()
    {
        PlayerController.instance.anim.SetBool("Attack0" + PlayerController.instance.ComboAttackNumber, false);

        if (PlayerController.instance.ComboAttackNumber == 5)
        {
            PlayerController.instance.anim.SetBool("Attack05", false);
            PlayerController.instance.xPressed = false;
            PlayerController.instance.ComboAttackNumber = 1;
        }
        else if (PlayerController.instance.xPressed)
        {
            ++PlayerController.instance.ComboAttackNumber;
            PlayerController.instance.anim.SetBool("Attack0" + PlayerController.instance.ComboAttackNumber, true);
            PlayerController.instance.xPressed = false;
        }
        else
        {
            PlayerController.instance.ComboAttackNumber = 1;
        }

        PlayerController.instance.inCombo = false;
        GameManager.instance.whichAttack = AttackType.None;
    }

}
