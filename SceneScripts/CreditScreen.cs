using UnityEngine;
using DG.Tweening;

public class CreditScreen : UIScreen
{
    public GameObject creditsObj;

    private Tweener scroll;

    public float endPos;
    public float duration;

    void Update()
    {
        if (Input.GetButtonDown("B"))
        {
            UIManager.instance.Show<MainScene>();
        }
    }

    private void OnEnable()
    {
        scroll = creditsObj.transform.DOLocalMoveY(endPos, duration);
    }

    private void OnDisable()
    {
        scroll.Restart(false, -5);
        scroll.Flip();
    }
}
