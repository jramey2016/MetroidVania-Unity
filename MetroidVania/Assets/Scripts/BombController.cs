﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{

    public float timeToExplode = .5f;
    public GameObject explosion;

    public float blastRange;
    public LayerMask whatisDestructable;

    public int damageAmount;
    public LayerMask whatisDamageable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeToExplode -= Time.deltaTime; 
        if(timeToExplode <= 0)
        {
            if (explosion != null)
            {
                Instantiate(explosion, transform.position, transform.rotation);

            }

            Destroy(gameObject);

            Collider2D [] objectsToRemove = Physics2D.OverlapCircleAll(transform.position, blastRange, whatisDestructable);

            if(objectsToRemove.Length > 0)
            {
                foreach(Collider2D col in objectsToRemove)
                {
                    Destroy(col.gameObject);
                }
            }

            Collider2D[] objectsToDamage = Physics2D.OverlapCircleAll(transform.position, blastRange, whatisDamageable);

            foreach(Collider2D col in objectsToDamage)
            {
                EnemyHealthController enemyHealth = col.GetComponent<EnemyHealthController>();
                if(enemyHealth != null)
                {
                    enemyHealth.DamageEnemy(damageAmount);
                }
            }

            AudioManager.instance.PlaySFXAdjusted(8);
        }
    }
}
