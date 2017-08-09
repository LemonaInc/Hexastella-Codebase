using System.Collections;
using UnityEngine;

public class Hexagons : MonoBehaviour
{
    public float hexagonTime = 10f;
    public float cooldownTime = 10f;
    public GameObject godRay;

    public GameObject[] icons;

    public GameObject activeVisual;

    public SkinnedMeshRenderer playerRend;
    public SkinnedMeshRenderer swordRend;

    public Material[] playerMaterial;
    public Material[] swordMaterial;

    private HexagonShield hexagonShield;

    private PlayerHealth health;

    protected bool isActive = false;

    protected bool hasBeenActivated = false;

    public float activatedHexagonGlow = 9f;

    public float deactivatedHexagonGlow = 0.5f;
    
    public void Activate()
    {
        isActive = true;
        // Set the godray to glow when only the activatable hexagon can be activated
        godRay.SetActive(true);

        if (isActive == true)
            GetComponent<MeshRenderer>().material.SetFloat("_EmissionIntensity", activatedHexagonGlow);

        if (activeVisual != null)
            activeVisual.SetActive(true);
    }

    public void Deactivate()
    {
        isActive = false;

        if (activeVisual != null)

            activeVisual.SetActive(false);
    }

    void Awake()
    {
        godRay.SetActive(false);
        GetComponent<MeshRenderer>().material.SetFloat("_EmissionIntensity", deactivatedHexagonGlow);

        health = GlobalUtils.PlayerHealth;
        hexagonShield = FindObjectOfType<HexagonShield>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isActive)
        {
            foreach (GameObject i in icons)
                i.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (GameObject i in icons)
            i.SetActive(false);
    }

    public void SelectHexagon()
    {
        isActive = true;
        GetComponent<MeshRenderer>().material.SetFloat("_EmissionIntensity", 1f);

        godRay.SetActive(true);
    }

    public virtual IEnumerator ActivateHexagon()
    {
        AkSoundEngine.PostEvent("Play_Hexagon_Activation", gameObject);

        GetComponentInParent<MeshRenderer>().material.SetFloat("_EmissionIntensity", 0.5f);
        GetComponent<MeshRenderer>().material.SetFloat("_EmissionIntensity", 0.5f);

        godRay.SetActive(false);
        hasBeenActivated = true;
        yield return null;

        Deactivate();
    }

    public void TurnInvisible()
    {
        playerRend.material = playerMaterial[1];
        swordRend.material = swordMaterial[1];
    }

    public void TurnBoostRed()
    {
        playerRend.material = playerMaterial[2];
        swordRend.material = swordMaterial[2];
    }

    public void TurnNormal()
    {
        playerRend.material = playerMaterial[0];
        swordRend.material = swordMaterial[0];
    }

    // ADD DEACTIVATE HEXAGON
}
