//  EnemyAttackController.CS
//  Copyright ©017 Jaxon Stevens. All rights reserved.
// Created by Jaxon Stevens and Hillary Ocando
// Created in part of our VFS game final project.

using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyAttackController : Unit
{
    public float rangeToMeleeAttack = 30f;

    public bool activateBossStateMachineBool;

    public GameObject arenaPosition;

    internal AOEWaveAttack aoeWaveAttack;
    internal TentacleAttack tentacleAttack;
    internal LaserV2Attack laserV2Attack;

    public ParticleSystem bossSpawnParticles;
    internal BossEmerge bossEmerge;
    internal PlayerHealth mainPlayerHealth;

    EnemyHealth enemyHealth;

    internal HexagonInvisibility hexagonInvisibility;

    internal NavMeshAgent nav;

    public PlayerController player;

    // Create a randomClone GameObject foreach AI RandomTracker 
    public GameObject positionTrackerOriginal;
    public GameObject randomClone;
    public GameObject randomClone2;

    [Range(0, 10)]
    public int chanceToLaser = 3;

    private IEnumerator currentActiveBehaviour;
    internal Transform target;

    public bool RotateBossTowardsPlayerIsActive;

    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();

        player = FindObjectOfType<PlayerController>();
        tentacleAttack = GetComponent<TentacleAttack>();
        aoeWaveAttack = GetComponent<AOEWaveAttack>();
        laserV2Attack = GetComponent<LaserV2Attack>();
        bossEmerge = FindObjectOfType<BossEmerge>();
        mainPlayerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        hexagonInvisibility = FindObjectOfType<HexagonInvisibility>();
        // Start the random boss location spawner on start 
        StartSpawner();
    }

    protected override void Start()
    {
        base.Start();
        target = null;

        SetState(OnIdleState());

        AkSoundEngine.PostEvent("Play_Alien_Hover", gameObject);
    }

    private int lastMiniBossSpawned = 0;
    private int maxMiniBosses = 6;
    // Spawn the mini Bosses at these points in the First Bosses Health
    internal int[] miniBossSpawnHealth = new int[10] { 750, 700, 650, 600, 550, 500, 450, 400, 350, 300 };
    private bool[] didSpawnMiniBoss = new bool[10] { false, false, false, false, false, false, false, false, false, false };

    void Update()
    {
        if (mainPlayerHealth.currentHealth < 0)
        {
            SetState(OnPlayerDiesState());
        }

        // SPAWN THE MINI BOSSES 
        for (int i = 0; i < miniBossSpawnHealth.Length; ++i)
        {
            if (enemyHealth.currentHealth < miniBossSpawnHealth[i] && !didSpawnMiniBoss[i])
            {
                bossSpawnParticles.Play();

                bossEmerge.SpawnMiniBoss();
                didSpawnMiniBoss[i] = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            bossSpawnParticles.Play();
            bossEmerge.SpawnMiniBoss();

        }
    }

    // Start method for defining the first state
    void StartSpawner()
    {
        StartCoroutine(Spawner());
    }

    public IEnumerator Spawner()
    {
        // Set spawn Random Trackers to true
        bool spawnRandomTrackers = true;

        while (spawnRandomTrackers)
        {
            // Here we Instantiate the random player tracker (randomClone) at a random location and delete the clones. 
            // This is used for a more improved and random version of the AI for the enemy
            // We then destroy  the generated clone at the end of the method before it starts over again. This create a dynamic AI that can not be predicted by the player.

            Vector3 random1 = new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100));
            // Random Clone GameObject STARTS AT 0
            randomClone = Instantiate(positionTrackerOriginal, arenaPosition.transform.position + random1, Quaternion.identity);
            randomClone.transform.SetParent(transform);
            // We set the new name of the cloned object
            randomClone.name = "randomClone";
            yield return new WaitForSeconds(0.1f);
            Destroy(randomClone.gameObject, 1f);

            Vector3 random2 = new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100));
            randomClone2 = Instantiate(positionTrackerOriginal, arenaPosition.transform.position + random2, Quaternion.identity);
            randomClone2.transform.SetParent(transform);
            // We set the new name of the cloned object
            randomClone2.name = "randomClone2";
            yield return new WaitForSeconds(0.1f);

            //positionTrackerOriginal.SetActive(true);
            randomClone2.SetActive(true);

            // Destroy the randomClone 
            Destroy(randomClone2.gameObject, 1f);
            yield return null;
        }
    }

    // Create and deine the states for states in the first state controller
    private void SetState(IEnumerator newState)
    {
        if (currentActiveBehaviour != null)
            StopCoroutine(currentActiveBehaviour);

        currentActiveBehaviour = newState;
        StartCoroutine(newState);
    }

    // Call this function when the boss should rotate towards the player
    public IEnumerator RotateBossTowardsPlayer()
    {
        RotateBossTowardsPlayerIsActive = true;
        while (RotateBossTowardsPlayerIsActive == true)
        {
            transform.LookAt(target);

            yield return null;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////
    // ENEMY IDLE STATE
    public IEnumerator OnIdleState()
    {
        VoiceOverSoundManager.instance.PlayEnemyVoiceMaybe("Play_Xenandros_Taunts", 30f);

        nav.speed = 80;
        if (target == null)
            SetState(OnMoveToRandomPosition());
        else
            ChooseAction();

        yield return null;
    }

    IEnumerator OnMoveToTarget()
    {
        //Move towards target here
        //Checks for distance to player, and when distacne is close enough the change state to
        anim.SetBool("Moving", true);

        float distSqr = (transform.position - player.transform.position).sqrMagnitude;

        while (distSqr > rangeToMeleeAttack * rangeToMeleeAttack)
        {
            // move towards target
            nav.SetDestination(player.transform.position);
            distSqr = (transform.position - player.transform.position).sqrMagnitude;

            yield return null;
        }

        anim.SetBool("Moving", false);
        ChooseCloseAttack();

        yield return null;
    }

    IEnumerator OnMoveToRandomPosition()
    {
        //Also check if i see the target, the set target to transfor of player
        //Check when we reach the position
        //if we reach it then set state to Idle;
        while (target == null)
        {
            yield return StartCoroutine(RandomizeBossLocation(2f, randomClone.transform.position));

            yield return null;
        }

        SetState(OnIdleState());
    }

    /////////////////////////////////////// SHOOT LASER STATE ///////////////////////////////////////////
    IEnumerator OnLaserAttack()
    {
        StartCoroutine(CheckForTargetNull());

        StartCoroutine(RotateBossTowardsPlayer());
        // Play AAAP warning
        VoiceOverSoundManager.instance.PlayPlayerVoiceMaybe("Play_AAAP_TUT2", 10f);

        yield return laserV2Attack.EyeLaserAttackEnabledState();
        yield return laserV2Attack.EyeLaserAttackDisabledState();
        RotateBossTowardsPlayerIsActive = false;
        SetState(OnIdleState());
        yield return null;
    }

    private IEnumerator OnTentacleAttack()
    {
        nav.speed = 0;
        StartCoroutine(CheckForTargetNull());

        yield return tentacleAttack.ActivateTentacleAttackState();

        SetState(OnIdleState());
        yield return null;
    }

    /////////////////////////////////////// AOE WAVE ATTACK STATE ///////////////////////////////////////////
    IEnumerator OnAOEAttack()
    {
        nav.speed = 0;
        StartCoroutine(CheckForTargetNull());

        yield return aoeWaveAttack.AOEWaveAttackState();

        SetState(OnIdleState());
        yield return null;
    }

    private IEnumerator OnSpinAttack()
    {
        nav.speed = 0;
        StartCoroutine(CheckForTargetNull());

        yield return new WaitForSeconds(0.5f);

        float totalLerpTime = 0.5f;
        float currentLerpTime = 0.0f;
        Vector3 startRotation = transform.eulerAngles;
        Vector3 endRotation = transform.eulerAngles + 360f * Vector3.up;

        anim.SetBool("180Spin", true);

        while (currentLerpTime < totalLerpTime)
        {
            //Iterate the current lerp time
            currentLerpTime += Time.deltaTime;
            //Cap the time at our total lerp time
            if (currentLerpTime > totalLerpTime)
            {
                currentLerpTime = totalLerpTime;
            }

            float lerpPercentage = currentLerpTime / totalLerpTime;
            transform.eulerAngles = Vector3.Slerp(startRotation, endRotation, lerpPercentage);

            yield return null;
        }

        anim.SetBool("180Spin", false);

        SetState(OnIdleState());
        yield return null;
    }
    // END REAR SPIN ATTACK STATE

    // PLAYER DIE STATE
    IEnumerator OnPlayerDiesState()
    {
        target = null;

        yield return null;
    }

    void ChooseAction()
    {
        if (Random.Range(0, 10) < chanceToLaser)
        {
            SetState(OnLaserAttack());
        }
        else
        {
            SetState(OnMoveToTarget());
        }
    }

    void ChooseCloseAttack()
    {
        int r = Random.Range(0, 3);
        switch (r)
        {
            case 0:
                SetState(OnTentacleAttack());
                break;
            case 1:
                SetState(OnAOEAttack());
                break;
            default:
                SetState(OnSpinAttack());
                break;
        }
    }

    IEnumerator RandomizeBossLocation(float waitTime, Vector3 position)
    {
        bool randomBossLocationBool;
        randomBossLocationBool = true;

        if (randomBossLocationBool == true)
        {
            anim.SetBool("Moving", true);
            yield return new WaitForSeconds(waitTime);
            nav.SetDestination(position);
        }

        anim.SetBool("Moving", false);
        randomBossLocationBool = false;
        yield return null;
    }

    IEnumerator CheckForTargetNull()
    {
        while (target == player.transform)
        {
            if (hexagonInvisibility.playerIsInvisible)
            {
                target = null;
            }
            yield return null;
        }
    }

} // END SCRIPT
