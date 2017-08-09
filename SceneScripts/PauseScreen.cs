using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseScreen : UIScreen
{
    public GameObject soundObject;
    public EventSystem eventSystem;
    public GameObject firstButton;
    
    void OnEnable()
    {
        eventSystem.SetSelectedGameObject(firstButton);
        Time.timeScale = 0;
        
        GameManager.instance.gamePaused = true;
    }

    void OnDisable()
    {
        Time.timeScale = 1;
        GameManager.instance.gamePaused = false;
    }
    
    public void OnResumeButton()
    {
        AkSoundEngine.PostEvent("Play_UI_HS_MESELECT", gameObject);
        UIManager.instance.Show<GameScreen>();
    }

    public void OnRestartButton()
    {
        SceneManager.UnloadSceneAsync("GameScene");
        SceneManager.LoadScene("GameScene", LoadSceneMode.Additive);
        UIManager.instance.Show<GameScreen>();
    }

    public void OnOptionsButton()
    {
        AkSoundEngine.PostEvent("Play_UI_HS_MESELECT", gameObject);
        UIManager.instance.Show<OptionScreen>();
    }

    public void OnMainMenuButton()
    {
        GameManager.instance.gameStarted = false;

        AkSoundEngine.StopAll();

        AkSoundEngine.PostEvent("Play_UI_HS_MESELECT", gameObject);
        UIManager.instance.Show<MainScene>();
        SceneManager.UnloadSceneAsync("GameScene");
        GameManager.instance.stopPlayer = false;
    }
}
