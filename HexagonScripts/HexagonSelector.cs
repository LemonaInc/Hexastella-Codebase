using UnityEngine;

public class HexagonSelector : MonoBehaviour
{
    internal Hexagons hexagons;
    public GameObject[] invisibleHexOptions;
    public GameObject[] boostHexOptions;
    public GameObject[] shieldHexOptions;
    // Used for the shield walls
    public GameObject[] shieldWallOptions;

    // Reference to the original Hexagon Positions to be changed
    public GameObject activeBoostHex;
    public GameObject activeInvisibleHex;
    public GameObject activeShieldHex;
    
    public void Start()
    {
        hexagons = FindObjectOfType<Hexagons>();

        RandomizeShieldHexes();
        RandomizeBoostHexes();
        RandomizeInvisibleHexes();
    }

    // Call this function from the Hexagon scripts and when this function is called then call the randomize hexagon function to randomize 
    public void ShieldHexagonHasExpired()
    {
        RandomizeShieldHexes();
    }

    public void BoostHexagonHasExpired()
    {
        RandomizeBoostHexes();
    }

    public void InvisibleHexagonHasExpired()
    {
        RandomizeInvisibleHexes();
    }
    
    public GameObject NextShield()
    {
        return shieldHexOptions[Random.Range(0, shieldHexOptions.Length - 1)];
    }

    public GameObject NextBoost()
    {
        return boostHexOptions[Random.Range(0, boostHexOptions.Length - 1)];
    }

    public GameObject NextInvisible()
    {
        return invisibleHexOptions[Random.Range(0, invisibleHexOptions.Length - 1)];
    }

    public GameObject NextShieldWall()
    {
        return shieldWallOptions[Random.Range(0, shieldWallOptions.Length - 1)];
    }
    
    public void RandomizeShieldHexes()
    {
        activeShieldHex = NextShield();
        activeShieldHex.GetComponent<Hexagons>().Activate();

        // If the active shield hex does not equal the Nextshield then set actvive
        if (activeShieldHex == null)
        {
            print("NULL");
        }
    }

    public void RandomizeBoostHexes()
    {
        activeBoostHex = NextBoost();
        activeBoostHex.GetComponent<Hexagons>().Activate();

        if (activeBoostHex == null)
        {
            print("NULL");
        }
    }

    public void RandomizeInvisibleHexes()
    {
        activeInvisibleHex = NextInvisible();
        activeInvisibleHex.GetComponent<Hexagons>().Activate();

        if (activeInvisibleHex == null)
        {
            print("NULL");
        }
    }

}
