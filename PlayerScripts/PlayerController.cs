using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Unit
{
    public float speed;
    public float slowSpeed;
    public float dashForce;
    public float dodgeLength;
    public float dashTime = 0.4f;
    [HideInInspector] public float blockTime = 1f;
    public float dodgeCooldown;
    [HideInInspector] public float blockCooldown;
    public float dashCooldown;

    internal float originalSpeed;

    private float dodgeTime = 0.2f;
    private float playerDash;
    private float succesBlockBoost = 2f;

    internal bool canMove = true;
    internal bool canAttack = true;
    internal bool canParry;

    private bool dodgeActive = false;
    private bool dashActive = false;
    private bool blockActive = false;

    private bool isDodging = false;
    private bool isDashing = false;
    private bool isBlocking = false;
    [HideInInspector] public bool isBoosting = false;
    internal bool isInvisible = false;
    private bool succesfulBlock = false;
    private bool isIdle;

    internal bool inCombo = false;
    internal bool xPressed = false;

    public int ComboAttackNumber = 1;

    public Image[] skillImageCooldown;
    public Image blockPopUpSpot;
    public Image blockLightFlash;
    public Sprite[] succesfulBlockPopUp;
    public Image[] outerGlow;

    internal ParticleSystem swordHitDamageParticles;

    public Transform enemy;
    public GameObject cam;
    internal Health health;
    private BossAnimationEvents bossAnimEvents;
    protected EnemyHealth mainEnemyHealth;

    //Singleton Stuff
    public static PlayerController instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    protected override void Start()
    {
        base.Start();
        mainEnemyHealth = GlobalUtils.BossHealth;
        health = GlobalUtils.Health;
        bossAnimEvents = FindObjectOfType<BossAnimationEvents>();

        swordHitDamageParticles = GetComponentInChildren<ParticleSystem>();
        swordHitDamageParticles.time = 5;

        originalSpeed = speed;

        AkSoundEngine.PostEvent("Play_G64_Idle", gameObject);
    }

    private void FixedUpdate()
    {
        //Player can not move character if they've won or lost
        if (GameManager.instance.stopPlayer)
            return;

        if (canMove)
            PlayerMovement();
    }

    float t = 0;

    void Update()
    {
        if (GameManager.instance.gamePaused)
            return;

        if (GameManager.instance.whichAttack == AttackType.None)
            PlayerInput();

        if (canAttack)
            PlayerMainAttack();

        if (ComboAttackNumber == 1 && inCombo || ComboAttackNumber >= 2)
        {
            GameManager.instance.whichAttack = AttackType.Attack;
        }

        if (Time.time - t >= (0.4f * ComboAttackNumber))
        {
            GameManager.instance.whichAttack = AttackType.None;
            canMove = true;
            inCombo = false;
            ComboAttackNumber = 1;
            anim.SetBool("Attack01", false);
            anim.SetBool("Attack02", false);
            anim.SetBool("Attack03", false);
            anim.SetBool("Attack04", false);
            anim.SetBool("Attack05", false);
            t = Time.time;
        }

        //Random force from the heavens above down on the player
        rb.AddForce(-transform.up * 200, ForceMode.Force);
    }

    void PlayerMainAttack()
    {
        //////////////////////////////////////////////ATTACK
        if (Input.GetButtonDown("X"))
        {
            xPressed = true;
            t = Time.time;

            if (ComboAttackNumber == 1 && !inCombo)
            {
                inCombo = true;
                anim.SetBool("Attack01", true);
                xPressed = false;
            }
        }
    }

    void PlayerInput()
    {
        /////////////////////////////////////////////////DASH
        playerDash = Input.GetAxis("RightTrigger");

        if (playerDash > 0.5 && !dashActive)
        {
            StartCoroutine(Dash());
        }

        ////////////////////////////////////////////////DODGE ROLL
        if (Input.GetButtonDown("A") && !dodgeActive)
        {
            StartCoroutine(DodgeRoll());
        }
    }

    void PlayerMovement()
    {
        //MOVEMENT
        float horizontalInput = Input.GetAxis("LeftJoystickX");
        float verticalInput = Input.GetAxis("LeftJoystickY");

        //Normalize our input vector
        Vector3 input = new Vector3(horizontalInput, 0, verticalInput);

        //Use the Camera pivot's direction as the player's
        input = cam.transform.TransformDirection(input);

        //Rotate and move player
        if (input != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(input);
            targetRotation.eulerAngles = new Vector3(0f, targetRotation.eulerAngles.y, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.15f);

            isIdle = false;
            AkSoundEngine.PostEvent("Stop_Player_Idle", gameObject);
        }
        else if (!isIdle && input == Vector3.zero)
        {
            AkSoundEngine.PostEvent("Play_G64_Idle", gameObject);
            isIdle = true;
        }

        input *= speed;
        input.y = rb.velocity.y;
        if (!isDashing && !isDodging)
            rb.velocity = input;

        //RUN animation
        anim.SetFloat("VerticalSpeed", Mathf.Abs(verticalInput));
        anim.SetFloat("HorizontalSpeed", Mathf.Abs(horizontalInput));
    }

    IEnumerator Dash()
    {
        GameManager.instance.whichAttack = AttackType.Dash;

        dashActive = true;
        isDashing = true;
        canAttack = false;

        skillImageCooldown[2].fillAmount = 1;
        outerGlow[2].color = Color.black;

        anim.SetBool("Dash", true);

        //Add a force on the player
        rb.AddForce(transform.forward * dashForce, ForceMode.Impulse);
        uint wwiseEventID = AkSoundEngine.PostEvent("Play_G64_Dash", gameObject);

        yield return new WaitForSeconds(dashTime);

        //reset the player's velocity to zero to stop them
        rb.velocity = Vector3.zero;
        canAttack = true;
        isDashing = false;

        anim.SetBool("Dash", false);

        GameManager.instance.whichAttack = AttackType.None;

        while (dashActive)
        {
            skillImageCooldown[2].fillAmount -= 1 / dashCooldown * Time.deltaTime;

            if (skillImageCooldown[2].fillAmount <= 0)
                dashActive = false;

            yield return null;
        }
        outerGlow[2].color = Color.white;
        SkillActive();
    }

    IEnumerator DodgeRoll()
    {
        GameManager.instance.whichAttack = AttackType.Dodge;
        dodgeActive = true;
        isDodging = true;
        canAttack = false;

        skillImageCooldown[0].fillAmount = 1;
        outerGlow[0].color = Color.black;

        anim.SetBool("Roll", true);

        health.damageDeflect = true;

        rb.AddForce(transform.forward * dodgeLength, ForceMode.VelocityChange);
        AkSoundEngine.PostEvent("Play_G64_Dodge_Roll", gameObject);

        yield return new WaitForSeconds(dodgeTime);
        rb.velocity = Vector3.zero;
        health.damageDeflect = false;
        canAttack = true;
        isDodging = false;

        anim.SetBool("Roll", false);

        GameManager.instance.whichAttack = AttackType.None;

        while (dodgeActive)
        {
            skillImageCooldown[0].fillAmount -= 1 / dodgeCooldown * Time.deltaTime;

            if (skillImageCooldown[0].fillAmount <= 0)
                dodgeActive = false;

            yield return null;
        }

        outerGlow[0].color = Color.white;
        SkillActive();
    }


    //BLOCK HAS BEEN SCRAPED
    IEnumerator Block()
    {
        GameManager.instance.whichAttack = AttackType.Block;

        isBlocking = true;
        blockActive = true;
        canAttack = false;

        skillImageCooldown[1].fillAmount = 1;
        outerGlow[1].color = Color.black;

        while (isBlocking == true)
        {
            rb.velocity = Vector3.zero;

            anim.SetBool("IsBlocking", true);

            if (health.damaged)
            {
                anim.SetBool("BlockFailure", true);

                break;
            }

            if (canParry && bossAnimEvents.tentacleSlamActive || canParry && bossAnimEvents.isSpinning)
            {
                succesfulBlock = true;
                health.damageDeflect = true;
                health.regainHealth = true;

                anim.SetTrigger("BlockSuccess");

                int randomPopUp = Random.Range(0, 2);

                //Show succesful block with random sprite
                blockLightFlash.enabled = true;
                blockPopUpSpot.enabled = true;
                blockPopUpSpot.sprite = succesfulBlockPopUp[randomPopUp];

                break;
            }

            yield return new WaitForSeconds(blockTime);

            isBlocking = false;
            canAttack = true;
        }

        anim.SetBool("IsBlocking", false);

        GameManager.instance.whichAttack = AttackType.None;

        if (succesfulBlock)
        {
            yield return new WaitForSeconds(succesBlockBoost);
            succesfulBlock = false;
        }

        blockLightFlash.enabled = false;
        blockPopUpSpot.enabled = false;
        health.damageDeflect = false;
        health.regainHealth = false;

        while (blockActive)
        {
            skillImageCooldown[1].fillAmount -= 1 / blockCooldown * Time.deltaTime;

            if (skillImageCooldown[1].fillAmount <= 0)
                blockActive = false;

            yield return null;
        }
        outerGlow[1].color = Color.white;
        SkillActive();
    }

    public void SkillActive()
    {
        AkSoundEngine.PostEvent("Play_SFX_HEX_Invisibility", gameObject);
    }
}