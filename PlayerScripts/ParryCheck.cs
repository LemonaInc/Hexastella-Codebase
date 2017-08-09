using UnityEngine;

public class ParryCheck : MonoBehaviour
{
    private EnemyAttackController enemy;

    private void Start()
    {
        enemy = FindObjectOfType<EnemyAttackController>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == enemy.gameObject)
        {
            PlayerController.instance.canParry = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController.instance.canParry = false;
    }

}
