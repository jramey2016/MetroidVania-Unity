using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatroller : MonoBehaviour
{
    public Transform[] patrolPoints;
    private int currentPoint;

    public float moveSpeed, waitAtPoints;
    private float waitCounter;

    public float jumpForce;

    public Rigidbody2D theRb;

    public Animator anim;


    // Start is called before the first frame update

    void Start()
    {
        waitCounter = waitAtPoints;
        foreach(Transform pPoint in patrolPoints)
        {
            pPoint.SetParent(null);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(transform.position.x - patrolPoints[currentPoint].position.x) > 0.2)
        {
            //moving right
            if(transform.position.x < patrolPoints[currentPoint].position.x)
            {
                theRb.velocity = new Vector2(moveSpeed, theRb.velocity.y);
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else
            {
                theRb.velocity = new Vector2(-moveSpeed, theRb.velocity.y);
                transform.localScale = Vector3.one;
            }
            if(transform.position.y < patrolPoints[currentPoint].position.y - .5f && theRb.velocity.y <.1f)
            {
                theRb.velocity = new Vector2(theRb.velocity.x, jumpForce);
            }
        }
        else
        {
            theRb.velocity = new Vector2(0f, theRb.velocity.y);

            waitCounter -= Time.deltaTime;
            if(waitCounter <= 0)
            {
                waitCounter = waitAtPoints;

                currentPoint++;

                if(currentPoint >= patrolPoints.Length)
                {
                    currentPoint = 0;
                }
            }
        }

        anim.SetFloat("speed", Mathf.Abs(theRb.velocity.x));
    }
}
