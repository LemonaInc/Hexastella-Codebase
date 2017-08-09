using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BossEmerge : MonoBehaviour
{

    internal PlayerController player;

    internal SmallEnemyAttackController smallEnemyAttackController;

    SmallEnemyAttackController[] smallEnemyAttackControllers;

    //Main Small Boss GameObject Reference
    public GameObject smallBoss;

    public Transform[] spawnPoints;

    public bool miniBossSpawnerActive;

    internal NavMeshAgent nav;

    public void Awake()
    {

        //////////////// //////////////// 
        nav = FindObjectOfType<NavMeshAgent>();
        // Nav Reset Path Must be called in order to reset the path for the instantiated NavMeshAgents
        // If this is not called you will get the error "Set Destination" for the nav mesh agent
        //////////////// //////////////// 

        player = FindObjectOfType<PlayerController>();
        smallEnemyAttackController = FindObjectOfType<SmallEnemyAttackController>();
        smallEnemyAttackControllers = FindObjectsOfType<SmallEnemyAttackController>();
        foreach (SmallEnemyAttackController smallEnemy in smallEnemyAttackControllers)
            // Set the player target to ! null
            smallEnemy.target = player.transform;

    }

    // Reset the navmesh agent coroutine
    IEnumerator ResetNavMeshAgent()
    {
        nav.ResetPath();

        yield return new WaitForSeconds(1);
        yield return null;
    }


    // When this gets called spawn the first mini boss 
    public void SpawnMiniBoss()
    {
        // call the navmesh agent reset coroutine
        // Randomize all the spawn points where the mini bosses can spawn at
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        GameObject clone = Instantiate(smallBoss, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        clone.GetComponent<NavMeshAgent>().enabled = true;
        print("Spawned: " + clone.name);
        miniBossSpawnerActive = false;

    }


} // END SCRIPT
