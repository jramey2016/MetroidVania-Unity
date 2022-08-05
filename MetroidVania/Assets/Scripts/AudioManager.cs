using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public void Awake()
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

    public AudioSource mainMenuMusic, levelMusic, bossMusic;

    public AudioSource[] sfx;

    public void PlayMainMenuMusic()
    {
        levelMusic.Stop();
        bossMusic.Stop();

        mainMenuMusic.Play();
    }

    public void PlayLevelMusic()
    {

        if (!levelMusic.isPlaying)
        {
            bossMusic.Stop();
            mainMenuMusic.Stop();

            levelMusic.Play();
        }
    
    }

    public void PlayBossMusic()
    {
        mainMenuMusic.Stop();
        levelMusic.Stop();

        bossMusic.Play();
    }

    public void PlaySFX(int value)
    {
        sfx[value].Stop();
        sfx[value].Play();
    }

    public void PlaySFXAdjusted(int sfxToAdjust)
    {
        sfx[sfxToAdjust].pitch = Random.Range(.8f, 1.2f);
        PlaySFX(sfxToAdjust);
    }

}
