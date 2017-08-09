using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : UIScreen
{    
    void OnEnable()
    {
        SceneManager.UnloadSceneAsync("GameScene");
        AkSoundEngine.StopAll();
        AkSoundEngine.PostEvent("Play_UI_HS_WIN", gameObject);
        StartCoroutine(ConditionTime());
    }

    void OnDisable()
    {
        Time.timeScale = 1;
        GameManager.instance.stopPlayer = false;
        GameManager.instance.playerWin = false;
    }
}
