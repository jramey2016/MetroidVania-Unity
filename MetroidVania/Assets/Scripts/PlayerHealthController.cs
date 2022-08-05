using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

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

    //[HideInInspector]
    public int currentHealth;
    public int maxHealth;

    public float invincibilitylength;
    private float invincCounter;

    public float flashLength;
    private float flashCounter;

    public SpriteRenderer[] playerSprites;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;


        
    }

    // Update is called once per frame
    void Update()
    {
        if(invincCounter > 0)
        {
            invincCounter -= Time.deltaTime;

            flashCounter -= Time.deltaTime;
            if(flashCounter <= 0)
            {
                foreach(SpriteRenderer sr in playerSprites)
                {
                    sr.enabled = !sr.enabled;
                }
                flashCounter = flashLength;

            }
            if(invincCounter <= 0)
            {
                foreach (SpriteRenderer sr in playerSprites)
                {
                    sr.enabled = true;
                }
                flashCounter = 0f;
            }
        }
    }


    public void DamagePlayer(int damageAmount)
    {
        if (invincCounter <= 0)
        {


            currentHealth -= damageAmount;

            if (currentHealth <= 0)
            {
                currentHealth = 0;

                //gameObject.SetActive(false);

                RespawnController.instance.Respawn();

                AudioManager.instance.PlaySFX(21);
            }
            else
            {
                invincCounter = invincibilitylength;

                AudioManager.instance.PlaySFXAdjusted(16);
            }
            UIController.instance.UpdateHealth(currentHealth, maxHealth);
        }
    }

    public void FillHealth()
    {
        currentHealth = maxHealth;


        UIController.instance.UpdateHealth(currentHealth, maxHealth);
    }

    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UIController.instance.UpdateHealth(currentHealth, maxHealth);
    }
}
