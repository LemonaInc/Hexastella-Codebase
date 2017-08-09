using UnityEngine;
using DG.Tweening;

public class BridgeMove : MonoBehaviour
{
    public float endPosition;
    public float duration;

    private float wallEndPos = 0.5f;
    private float wallDuration = 1f;

    private Transform player;
    public Collider safeWall;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform == player)
        {
            Tweener dropDown = transform.DOMoveY(endPosition, duration);
            AkSoundEngine.PostEvent("Play_SFX_HEX_Boost", gameObject);

            safeWall.transform.DOLocalMoveY(wallEndPos, wallDuration);
        }
    }
}
