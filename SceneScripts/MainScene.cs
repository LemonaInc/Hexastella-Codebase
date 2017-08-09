using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainScene : UIScreen
{
    public GameObject soundObject;
    public EventSystem eventSystem;
    public GameObject firstButton;
    private bool fromGame = false;

    private void Start()
    {
        PlayMainMenuMusic();
    }

    private void OnEnable()
    {
        if (fromGame)
        {
            eventSystem.SetSelectedGameObject(firstButton);

            PlayMainMenuMusic();

            fromGame = false;
        }
    }
    
    public void OnStartGame()
    {
        fromGame = true;

        if (fromGame)
        {
            AkSoundEngine.PostEvent("Stop_Hexastella_Battle_Theme", soundObject);
        }

        GameManager.instance.gameStarted = true;
        AkSoundEngine.PostEvent("Play_UI_HS_MESELECT", soundObject);
        SceneManager.LoadScene("GameScene", LoadSceneMode.Additive);
        UIManager.instance.Show<GameScreen>();
    }

    public void OnOptionScreen()
    {
        AkSoundEngine.PostEvent("Play_UI_HS_MESELECT", soundObject);
        UIManager.instance.Show<OptionScreen>();
    }

    public void OnCreditPage()
    {
        AkSoundEngine.PostEvent("Play_UI_HS_MESELECT", soundObject);
        UIManager.instance.Show<CreditScreen>();
    }

    public void OnQuitGame()
    {
        AkSoundEngine.PostEvent("Play_UI_HS_EXIT", soundObject);
        Application.Quit();
    }

    public void PlayMainMenuMusic()
    {
        AkSoundEngine.SetState("Battle_Theme_States", "Low_Intensity");
        AkSoundEngine.PostEvent("Play_Hexastella_Battle_Theme", soundObject);
    }
}
