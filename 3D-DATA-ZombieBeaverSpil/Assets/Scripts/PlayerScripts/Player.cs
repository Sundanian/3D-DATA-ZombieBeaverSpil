﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    #region Fields
    private Ray attackRay;
    private RaycastHit hit;
    private bool dead;

    [SerializeField]
    private int health;
    public int Health
    {
        get { return health; }
    }
    [SerializeField]
    private int bæverTænder;
    public int BæverTænder
    {
        get { return bæverTænder; }
    }

    [SerializeField]
    private float rateOfFire;
    private float shootClock;


    #endregion

    // Use this for initialization
    void Start()
    {
        shootClock = 0;
    }

    // Update is called once per frame
    void Update()
    {
        shootClock += Time.deltaTime;
    }

    private void Shoot()
    {
        if (shootClock >= rateOfFire)
        {
            MakeRay();
            if (Physics.Raycast(attackRay, out hit, Mathf.Infinity, (1 << 8)))
            {
                //Debug.Log("Hit with Ray: " + hit.collider.gameObject.layer);

                if (hit.collider.tag == "Enemy")
                {
                    hit.collider.SendMessage("TakeDamageMan", 10);
                    Debug.Log("Hit");
                }

            }
            shootClock = 0;
        }
    }

    private void MakeRay()
    {
        attackRay = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward * 10, Color.blue);
    }

    private void LifeZeroEnding()
    {
        if (health <= 0 && !dead)
        {
            bæverTænder = (int)(Time.time);
            //Destroy(gameObject);
            dead = true;

            Application.LoadLevelAdditive("Done Screen");
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

}