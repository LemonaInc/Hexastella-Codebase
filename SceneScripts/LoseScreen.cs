using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreen : UIScreen
{
    void OnEnable()
    {
        SceneManager.UnloadSceneAsync("GameScene");
        AkSoundEngine.StopAll();
        AkSoundEngine.PostEvent("Play_UI_HS_DEATH", gameObject);
        StartCoroutine(ConditionTime());
    }

    void OnDisable()
    {
        Time.timeScale = 1;
        GameManager.instance.stopPlayer = false;
        GameManager.instance.playerLose = false;
    }
}
