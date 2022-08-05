using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public string newGameScene;

    public GameObject continueButtion;

    public PlayerAbilityTracker player;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlayMainMenuMusic();
        if (PlayerPrefs.HasKey("ContinueLevel"))
        {
            continueButtion.SetActive(true);
        }
    }
    public void NewGame()
    {
        PlayerPrefs.DeleteAll();

        SceneManager.LoadScene(newGameScene);
    }


    public void Continue()
    {
        player.gameObject.SetActive(true);
        player.transform.position = new Vector3(PlayerPrefs.GetFloat("PosX"), PlayerPrefs.GetFloat("PosY"), 0);

        SceneManager.LoadScene(PlayerPrefs.GetString("ContinueLevel"));

        if (PlayerPrefs.HasKey("DoubleJumpUnlocked"))
        {
            if(PlayerPrefs.GetInt("DoubleJumpUnlocked") == 1)
            {
                player.canDoubleJump = true;
            }
        }
        if (PlayerPrefs.HasKey("DashUnlocked"))
        {
            if (PlayerPrefs.GetInt("DashUnlocked") == 1)
            {
                player.canDash = true;
            }
        }
        if (PlayerPrefs.HasKey("BallUnlocked"))
        {
            if (PlayerPrefs.GetInt("BallUnlocked") == 1)
            {
                player.canBecomeBall = true;
            }
        }
        if (PlayerPrefs.HasKey("BombUnlocked"))
        {
            if (PlayerPrefs.GetInt("BombUnlocked") == 1)
            {
                player.canDropBomb = true;
            }
        }

    }
    public void QuitGame()
    {
        Application.Quit();

        Debug.Log("Game Quit");
    }
}
