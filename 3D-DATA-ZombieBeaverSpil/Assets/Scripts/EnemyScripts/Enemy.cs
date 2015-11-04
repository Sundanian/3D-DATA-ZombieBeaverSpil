﻿using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    NavMeshAgent myAgent;
    GameObject playerGO;
    Object playerO;
    [SerializeField]
    float distanceToPlayerBeforeHold;
    [SerializeField]
    private int health;
    [SerializeField]
    private int damage;
    [SerializeField]
    private int attackRate;
    private float attackTime = 0;
    private Animator myAnimator;
    private float deathTimer = 0;
    private bool dead;

    // Use this for initialization
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        playerGO = GameObject.FindGameObjectWithTag("Player");
        playerO = FindObjectOfType<Player>();
        if (!dead)
        {
            Navigation();
            Attack();
        }
        if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("LayingDead"))
        {
            deathTimer += Time.deltaTime;
            dead = true;
            if (deathTimer >= 3)
            {
                Destroy(gameObject);
            }
        }
    }

    public void TakeDamageMan(int damage)
    {
        Debug.Log("Took Damage: " + damage);
        health -= damage;

        if (health <= 0)
        {
            myAnimator.SetBool("Dying", true);
        }
    }

    private void Attack()
    {
        Vector3 v = playerGO.transform.position - transform.position; //The vector between the player and the enemy
        if (Mathf.Sqrt(Mathf.Pow(v.x, 2) + Mathf.Pow(v.y, 2) + Mathf.Pow(v.z, 2)) <= distanceToPlayerBeforeHold * 2) //if the distance is less or equal the enemies stoppingposition * 2
        {
            attackTime += Time.deltaTime;
            if (attackTime >= attackRate)
            {
                (playerO as Player).TakeDamage(damage);
                attackTime = 0;
                myAnimator.SetBool("Attacking", true);
            }
        }
        else
        {
            myAnimator.SetBool("Attacking", false);
        }
    }

    private void Navigation()
    {
        Vector3 v = playerGO.transform.position - transform.position;
        if (Mathf.Sqrt(Mathf.Pow(v.x, 2) + Mathf.Pow(v.y, 2) + Mathf.Pow(v.z, 2)) > distanceToPlayerBeforeHold)
        {
            myAgent.SetDestination(playerGO.transform.position);
            myAnimator.SetBool("Walking", true);
        }
        else
        {
            myAgent.SetDestination(transform.position);
            myAnimator.SetBool("Walking", false);
        }
    }
}
