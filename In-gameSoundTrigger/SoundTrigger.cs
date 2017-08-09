using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    protected GameObject player;
    protected GameObject enemy;

    private void Start()
    {
        player = GlobalUtils.Player.gameObject;
        enemy = GlobalUtils.Boss.gameObject;
    }
}
