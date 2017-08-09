using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScreen : UIScreen
{
    public float deathPos;
    public float winOrLossWaitTimeTillConditionScreen = 5f;
    
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        AkSoundEngine.PostEvent("Play_Battle_Music", gameObject);
        GameManager.instance.playerWin = false;
        GameManager.instance.playerLose = false;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        AkSoundEngine.PostEvent("Stop_Battle_Music", gameObject);
    }

    void Update()
    {
        if(PlayerController.instance.transform.position.y < deathPos)
        {
            SceneManager.UnloadSceneAsync("GameScene");
            SceneManager.LoadScene("GameScene", LoadSceneMode.Additive);
            UIManager.instance.Show<GameScreen>();
        }

        if (GameManager.instance.playerWin)
        {
            StartCoroutine(WinWaitTime());
        } 
        
        if (GameManager.instance.playerLose)
        {
            StartCoroutine(LossWaitTime());
        }

        if (Input.GetButtonDown("StartButton"))
        {
            AkSoundEngine.PostEvent("Play_UI_HS_MESELECT", gameObject);
            UIManager.instance.Show<PauseScreen>();
        }
    }

    public IEnumerator WinWaitTime()
    {
        yield return new WaitForSecondsRealtime(winOrLossWaitTimeTillConditionScreen);
        UIManager.instance.Show<WinScreen>();
        AkSoundEngine.PostEvent("Play_Win", gameObject);
        GameManager.instance.stopPlayer = true;
    }

    public IEnumerator LossWaitTime()
    {
        yield return new WaitForSecondsRealtime(winOrLossWaitTimeTillConditionScreen);
        UIManager.instance.Show<LoseScreen>();
        AkSoundEngine.PostEvent("Play_Lose", gameObject);
        GameManager.instance.stopPlayer = true;
    }
}