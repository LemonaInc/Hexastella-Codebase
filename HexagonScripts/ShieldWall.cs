using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShieldWall : MonoBehaviour {

    public float upEndPosition;
    public float downEndPosition;
    public float duration;

    public void RiseUp()
    {
        Tweener moveWallUp = transform.DOLocalMoveY(upEndPosition, duration);
        AkSoundEngine.PostEvent("Play_G64_ShieldUp", gameObject);
    }

    public void DropDown()
    {
        Tweener moveWallDown = transform.DOLocalMoveY(-downEndPosition, duration);
    }
}
