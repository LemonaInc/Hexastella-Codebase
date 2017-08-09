using UnityEngine;

public class BossBackExtraDamage : MonoBehaviour
{
    public int extraBackDamage;

    private int originalDamage;

    private PlayerController player;
    private SwordHitDamage swordHit;

    void Start()
    {
        player = GlobalUtils.Player;
        swordHit = player.GetComponentInChildren<SwordHitDamage>();

        //originalDamage = swordHit.swordDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player.gameObject)
        {
            //swordHit.swordDamage += extraBackDamage;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //        swordHit.swordDamage = originalDamage;
    }
}
