using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScreen : MonoBehaviour
{
    public float winLoseTime = 3f;
    
    public IEnumerator ConditionTime()
    {
        GameManager.instance.gameStarted = false;

        yield return new WaitForSecondsRealtime(winLoseTime);

        UIManager.instance.Show<MainScene>();
    }
}
