using UnityEngine;

public class OptionScreen : UIScreen
{
    void Update()
    {
        if (Input.GetButtonDown("B"))
        {
            if (!GameManager.instance.gameStarted)
            {
                UIManager.instance.Show<MainScene>();
            }
            else if (GameManager.instance.gameStarted)
            {
                UIManager.instance.Show<GameScreen>();
            }
        }
    }
}
