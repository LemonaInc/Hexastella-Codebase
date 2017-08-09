//  EnemyAttackController.CS
//  Copyright ©017 Jaxon Stevens. All rights reserved.
// Created by Jaxon Stevens and Hillary Ocando
// Created in part of our VFS game final project.

using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class SmallEnemyAttackController : Unit
{
    public float rangeToMeleeAttack = 30f;

    // COOLDOWNS
    public GameObject arenaPosition;

    internal PlayerHealth mainPlayerHealth;
    internal InitialBossActivation initialBossActivation;
    internal SmallBossHealth smallBossHealth;

    public GameObject spinDamage;

    internal Animator bossAnimator;

    EnemyHealth enemyHealth;

    internal HexagonInvisibility hexagonInvisibility;

    internal NavMeshAgent nav;

    internal PlayerController player;

    // Create a randomClone GameObject foreach AI RandomTracker 
    public GameObject positionTrackerOriginal;
    public GameObject randomClone;

    [Range(0, 10)]
    public int chanceTolaser = 2;

    private IEnumerator currentActiveBehaviour;
    internal Transform target;

    void Awake()
    {
        nav = FindObjectOfType<NavMeshAgent>();
        player = FindObjectOfType<PlayerController>();
        bossAnimator = GetComponentInChildren<Animator>();
        mainPlayerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        smallBossHealth = GetComponent<SmallBossHealth>();
        initialBossActivation = FindObjectOfType<InitialBossActivation>();
        hexagonInvisibility = FindObjectOfType<HexagonInvisibility>();
        target = player.transform;
        // Start the random boss location spawner on start 
        StartSpawner();
    }

    protected override void Start()
    {
        base.Start();
        SetState(OnIdleState());
    }

    public void PlayerInvisible()
    {
        SetState(OnPlayerInvisibilityState());
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

    // ENEMY IDLE STATE
    public IEnumerator OnIdleState()
    {
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

            if (nav != null)
            {
                nav.enabled = true;
                nav.SetDestination(player.transform.position);
            }
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

    private IEnumerator OnSpinAttack()
    {
        yield return new WaitForSeconds(0.5f);

        spinDamage.SetActive(true);

        float totalLerpTime = 0.5f;
        float currentLerpTime = 0.0f;
        Vector3 startRotation = transform.eulerAngles;
        Vector3 endRotation = transform.eulerAngles + 360f * Vector3.up;

        while (currentLerpTime < totalLerpTime)
        {
            bossAnimator.SetBool("180Spin", true);

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

        bossAnimator.SetBool("180Spin", false);

        spinDamage.SetActive(false);

        SetState(OnIdleState());
        yield return null;
    }
    // END REAR SPIN ATTACK STATE

    // PLAYER INVISIBILITY STATE
    public IEnumerator OnPlayerInvisibilityState()
    {
        // While the playerIsInvisible do 
        while (hexagonInvisibility.playerIsInvisible == true)
        {
            target = null;

            yield return null;
        }
    }

    void ChooseAction()
    {
        if (Random.Range(0, 10) < 2)
        {
            SetState(OnSpinAttack());
        }
        else
        {
            SetState(OnMoveToTarget());
        }
    }

    void ChooseCloseAttack()
    {
        int r = Random.Range(0, 2);
        switch (r)
        {
            case 0:
                SetState(OnMoveToTarget());
                break;
            case 1:
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

} // END SCRIPT
