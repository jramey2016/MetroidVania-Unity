using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UIController : MonoBehaviour
{
    public static UIController instance;
    // Start is called before the first frame update
    public string mainMenuScene;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Slider healthSlider;

    public Image fadeScreen;
    public float fadeSpeed = 2f;

    private bool fadingToBlack, fadingfromBlack;

    public GameObject pauseScreen;

    void Start()
    {
       // UpdateHealth(PlayerHealthController.instance.currentHealth, PlayerHealthController.instance.maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (fadingToBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            if(fadeScreen.color.a == 1f)
            {
                fadingToBlack = false;
            }
        }
        else if (fadingfromBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            if (fadeScreen.color.a == 1f)
            {
                fadingfromBlack = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    public void StartFadeToBlack()
    {
        fadingToBlack = true;
        fadingfromBlack = false;
    }

    public void StartFadeFromBlack()
    {
        fadingfromBlack = true;
        fadingToBlack=false;
    }

    public void PauseUnpause()
    {
        if (!pauseScreen.activeSelf)
        {
            pauseScreen.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            
            pauseScreen.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void GoToMainMenu()
    {
        
        Destroy(PlayerHealthController.instance.gameObject);
        PlayerHealthController.instance = null;

        Destroy(RespawnController.instance.gameObject);
        RespawnController.instance = null;

        instance = null;
        Destroy(gameObject);

        SceneManager.LoadScene(mainMenuScene);
        Time.timeScale = 1f;
    }
}
