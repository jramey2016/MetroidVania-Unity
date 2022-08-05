using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D myRB;

    public float moveSpeed;
    public float jumpForce;
    
    public Transform groundPoint;
    private bool isOnGround;
    public LayerMask whatIsGround;

    public Animator anim;

    public BullerController shotToFire;
    public Transform shotPoint;

    public float dashSpeed, dashTime;
    private float dashCounter;

    private bool canDoubleJump;

    public SpriteRenderer theSR, afterImage;
    public float afterImageLifeTime, timeBetweenAfterImages;
    private float afterImageCounter;
    public Color afterImageColor;

    public float waitAfterDashing;
    private float dashRechargeCounter;

    public GameObject stading, ball;
    public float waitToBall;
    private float ballCounter;

    public Animator ballAnim;

    public Transform bombPoint;
    public GameObject bomb;

    private PlayerAbilityTracker abilities;

    public bool canMove;


    // Start is called before the first frame update
    void Start()
    {
        abilities = GetComponent<PlayerAbilityTracker>();

        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && Time.timeScale != 0)
        {

            if (dashRechargeCounter > 0)
            {
                dashRechargeCounter -= Time.deltaTime;
            }
            else
            {

                if (Input.GetButtonDown("Fire2") && stading.activeSelf && abilities.canDash)
                {
                    dashCounter = dashTime;

                    ShowAfterImage();

                    AudioManager.instance.PlaySFXAdjusted(23);

                }
            }

            if (dashCounter > 0)
            {

                dashCounter = dashCounter - Time.deltaTime;

                myRB.velocity = new Vector2(dashSpeed * transform.localScale.x, myRB.velocity.y);

                afterImageCounter -= Time.deltaTime;

                if (afterImageCounter <= 0)
                {
                    ShowAfterImage();
                }

                dashRechargeCounter = waitAfterDashing;
            }
            else
            {
                //move sideways
                myRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, myRB.velocity.y);

                //Handle direction change
                if (myRB.velocity.x < 0)
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
                else if (myRB.velocity.x > 0)
                {
                    transform.localScale = Vector3.one;
                }
            }

            //Ground check
            isOnGround = Physics2D.OverlapCircle(groundPoint.position, .2f, whatIsGround);

            //jumping
            if (Input.GetButtonDown("Jump") && (isOnGround || (canDoubleJump && abilities.canDoubleJump)))
            {
                if (isOnGround)
                {
                    canDoubleJump = true;
                    AudioManager.instance.PlaySFXAdjusted(15);
                }
                else
                {
                    canDoubleJump = false;
                    anim.SetTrigger("doubleJump");
                    AudioManager.instance.PlaySFXAdjusted(19);
                }
                myRB.velocity = new Vector2(myRB.velocity.x, jumpForce);
            }

            //Shooting
            if (Input.GetButtonDown("Fire1"))
            {
                if (stading.activeSelf)
                {
                    Instantiate(shotToFire, shotPoint.position, shotPoint.rotation).moveDir = new Vector2(transform.localScale.x, 0f);
                    anim.SetTrigger("shotFired");
                    AudioManager.instance.PlaySFXAdjusted(13);
                }
                else if (ball.activeSelf && abilities.canDropBomb)
                {
                    Instantiate(bomb, bombPoint.position, bombPoint.rotation);
                }

            }

            //ball mode
            if (!ball.activeSelf)
            {
                if (Input.GetAxisRaw("Vertical") < -.9f && abilities.canBecomeBall)
                {
                    ballCounter -= Time.deltaTime;
                    if (ballCounter <= 0)
                    {
                        ball.SetActive(true);
                        stading.SetActive(false);
                        AudioManager.instance.PlaySFX(12);
                    }
                }
                else
                {
                    ballCounter = waitToBall;
                }
            }
            else
            {
                if (Input.GetAxisRaw("Vertical") > .9f)
                {
                    ballCounter -= Time.deltaTime;
                    if (ballCounter <= 0)
                    {
                        ball.SetActive(false);
                        stading.SetActive(true);
                        AudioManager.instance.PlaySFX(17);
                    }
                }
                else
                {
                    ballCounter = waitToBall;
                }
            }
        }
        else
        {
            myRB.velocity = Vector2.zero;
        }

        if (stading.activeSelf)
        {
            anim.SetBool("isOnGround", isOnGround);
            anim.SetFloat("speed", Mathf.Abs(myRB.velocity.x));
        }

        if (ball.activeSelf)
        {
            ballAnim.SetFloat("speed2", Mathf.Abs(myRB.velocity.x));
        }
        
    }

    public void ShowAfterImage()
    {
        SpriteRenderer image = Instantiate(afterImage, transform.position, transform.rotation);
        image.sprite = theSR.sprite;
        image.transform.localScale = transform.localScale;
        image.color = afterImageColor;
        Destroy(image.gameObject, afterImageLifeTime);

        afterImageCounter = timeBetweenAfterImages;
    }
}
