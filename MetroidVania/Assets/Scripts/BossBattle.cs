using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattle : MonoBehaviour
{
    private CameraController theCam;

    public Transform camPosistion;
    public float camSpeed;

    public int thershold1, thershold2;

    public float activeTime, fadeoutTime, inactiveTime;
    private float activeCounter, fadeCounter, inactiveCounter;

    public Transform[] spawnPoints;
    private Transform targetPoint;
    public float moveSpeed;

    public Animator anim;

    public Transform theBoss;

    public float timeBetweenShots1, timeBetweenShots2;
    private float shotCounter;
    public GameObject bullet;
    public Transform shotPoint;

    public GameObject winObjects;

    private bool battleEnded;

    public string bossRef;


    // Start is called before the first frame update
    void Start()
    {
        theCam = FindObjectOfType<CameraController>();
        theCam.enabled = false;

        activeCounter = activeTime;

        shotCounter = timeBetweenShots1;

        AudioManager.instance.PlayBossMusic();

    }

    // Update is called once per frame
    void Update()
    {
        theCam.transform.position = Vector3.MoveTowards(theCam.transform.position, camPosistion.position, camSpeed * Time.deltaTime);
        theBoss.transform.position = new Vector3(theBoss.position.x, theBoss.position.y, 1);
        if (!battleEnded)
        {

            winObjects.SetActive(false);
            if (BossHealthController.instance.currentHealth > thershold1)
            {
                if (activeCounter > 0)
                {
                    activeCounter -= Time.deltaTime;
                    if (activeCounter <= 0)
                    {
                        fadeCounter = fadeoutTime;
                        anim.SetTrigger("Vanish");

                    }

                    shotCounter -= Time.deltaTime;
                    if (shotCounter <= 0)
                    {
                        shotCounter = timeBetweenShots1;
                        Instantiate(bullet, shotPoint.position, Quaternion.identity);
                    }
                }
                else if (fadeCounter > 0)
                {
                    fadeCounter -= Time.deltaTime;
                    if (fadeCounter <= 0)
                    {
                        theBoss.gameObject.SetActive(false);
                        inactiveCounter = inactiveTime;
                    }
                }
                else if (inactiveCounter > 0)
                {
                    inactiveCounter -= Time.deltaTime;
                    if (inactiveCounter <= 0)
                    {
                        theBoss.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                        theBoss.gameObject.SetActive(true);

                        theBoss.transform.position = new Vector3(theBoss.position.x, theBoss.position.y, 1);
                        activeCounter = activeTime;
                        shotCounter = timeBetweenShots1;
                    }
                }
            }
            else
            {

                if (targetPoint == null)
                {
                    targetPoint = theBoss;
                    fadeCounter = fadeoutTime;
                    anim.SetTrigger("Vanish");
                }
                else
                {
                    if (Vector3.Distance(theBoss.position, targetPoint.position) > .02f)
                    {
                        theBoss.position = Vector3.MoveTowards(theBoss.position, targetPoint.position, moveSpeed * Time.deltaTime);
                        Debug.Log("The boss z pos: " + theBoss.position);
                        Debug.Log("Target z pos: " + targetPoint.position);
                        if (Vector3.Distance(theBoss.position, targetPoint.position) <= .2f)
                        {
                            Debug.Log("Within target posistion");
                            fadeCounter = fadeoutTime;
                            anim.SetTrigger("Vanish");
                        }
                        shotCounter -= Time.deltaTime;
                        if (shotCounter <= 0)
                        {
                            if (PlayerHealthController.instance.currentHealth > thershold2)
                            {
                                shotCounter = timeBetweenShots1;
                            }
                            else
                            {
                                shotCounter = timeBetweenShots2;
                            }
                            Instantiate(bullet, shotPoint.position, Quaternion.identity);
                        }
                    }
                    else if (fadeCounter > 0)
                    {
                        fadeCounter -= Time.deltaTime;
                        if (fadeCounter <= 0)
                        {
                            theBoss.gameObject.SetActive(false);
                            inactiveCounter = inactiveTime;
                        }
                    }
                    else if (inactiveCounter > 0)
                    {
                        inactiveCounter -= Time.deltaTime;
                        if (inactiveCounter <= 0)
                        {
                            theBoss.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                            theBoss.gameObject.SetActive(true);

                            targetPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];


                            while (targetPoint.position == theBoss.position)
                            {
                                targetPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                            }

                            theBoss.transform.position = new Vector3(theBoss.position.x, theBoss.position.y, 1);
                            targetPoint.transform.position = new Vector3(targetPoint.position.x, targetPoint.position.y, 1);

                            if (PlayerHealthController.instance.currentHealth > thershold2)
                            {
                                shotCounter = timeBetweenShots1;
                            }
                            else
                            {
                                shotCounter = timeBetweenShots2;
                            }

                        }
                    }
                }
            }
        }
        else
        {
            fadeCounter -= Time.deltaTime;
            if(fadeCounter < 0)
            {
                if(winObjects != null)
                {
                    winObjects.SetActive(true);
                    winObjects.transform.SetParent(null);
                }

                theCam = FindObjectOfType<CameraController>();
                theCam.enabled = true;
                
                gameObject.SetActive(false);

                AudioManager.instance.PlayLevelMusic();

                PlayerPrefs.SetInt(bossRef, 1);
            }
        }

    }

    public void EndBattle()
    {
        battleEnded = true;

        fadeCounter = fadeoutTime;
        anim.SetTrigger("Vanish");
        theCam.transform.position = Vector3.MoveTowards(theCam.transform.position, camPosistion.position, camSpeed * Time.deltaTime);

        theBoss.GetComponent<Collider2D>().enabled = false;

        BossBullet[] bullets = FindObjectsOfType<BossBullet>();
        if(bullets.Length > 0)
        {
            foreach(BossBullet bb in bullets) 
            { 
                Destroy(bb.gameObject);
            }
        }
    }
}
