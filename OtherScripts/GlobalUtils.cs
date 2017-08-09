using UnityEngine;
using UnityEngine.AI;

public class GlobalUtils : MonoBehaviour {

    public static PlayerController Player;
    public static EnemyAttackController Boss;
    public static GameManager GameManager;
    public static Health Health;
    public static PlayerHealth PlayerHealth;
    public static EnemyHealth BossHealth;
    public static NavMeshAgent Nav;
    
    void Awake ()
    {
        Player = FindObjectOfType<PlayerController>();
        Boss = FindObjectOfType<EnemyAttackController>();
        Health = Player.GetComponent<Health>();
        PlayerHealth = Player.GetComponent<PlayerHealth>();
        BossHealth = Boss.GetComponent<EnemyHealth>();
        Nav = Boss.GetComponent<NavMeshAgent>();
	}
	
}
