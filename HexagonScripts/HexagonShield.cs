using System.Collections;
using UnityEngine;

public class HexagonShield : Hexagons {

    public int healthBoost;

    internal bool isHealing;

    private PlayerHealth health;

    internal HexagonSelector hexagonSelector;

    public ParticleSystem healParticles;

    GameObject activatedRisingWall;

    public void Start()
    {
        health = GlobalUtils.PlayerHealth;
        hexagonSelector = FindObjectOfType<HexagonSelector>();
        hexagonTime = 3f;
    }

    void OnTriggerStay (Collider other)
    {
        if (other.gameObject.name == "PlayerController" && Input.GetButtonDown("Y") && isActive && !hasBeenActivated)
        {
            health.damageDeflect = true;
            StartCoroutine(ActivateHexagon());
        }
    }
    
    public override IEnumerator ActivateHexagon()
    {
        yield return base.ActivateHexagon();

        healParticles.Play();

        if (health.currentHealth < 100)
        {
            health.currentHealth += healthBoost;

            if (health.currentHealth > health.startingHealth)
                health.currentHealth = health.startingHealth;
        }
        
        ShieldWall wall = GetComponentInChildren<ShieldWall>(true);

        wall.RiseUp();
        AkSoundEngine.PostEvent("Play_SFX_HEX_SHD_Initiate", gameObject);

        isHealing = true;

        yield return new WaitForSeconds(hexagonTime);

        health.damageDeflect = false;

        healParticles.Stop();

        wall.DropDown();
        AkSoundEngine.PostEvent("Play_SFX_HEX_SHD_Disengage", gameObject);

        hasBeenActivated = false;

        // Call The Hexagon Has Expired Function
        hexagonSelector.ShieldHexagonHasExpired();     
    } 
    
}
