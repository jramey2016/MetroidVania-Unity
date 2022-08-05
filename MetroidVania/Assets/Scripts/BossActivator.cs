using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActivator : MonoBehaviour
{
    public GameObject bossToactivate;

    public string bossRef;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if (PlayerPrefs.HasKey(bossRef))
            {
                if(PlayerPrefs.GetInt(bossRef) != 1)
                {
                    bossToactivate.SetActive(true);

                    gameObject.SetActive(false);
                }
            }
            else
            {
                bossToactivate.SetActive(true);

                gameObject.SetActive(false);
            }
            
        }
    }

}
