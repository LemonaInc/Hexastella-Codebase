using UnityEngine;

public enum AttackType : byte
{
    None,
    Attack,
    Dodge,
    Dash,
    Block
};

public class GameManager : MonoBehaviour {

    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }
    private static GameManager _instance;

    public bool playerWin;
    public bool playerLose;
    public bool gamePaused;
    public bool gameStarted = false;
    internal bool stopPlayer = false;
    public AttackType whichAttack = AttackType.None; 
}
