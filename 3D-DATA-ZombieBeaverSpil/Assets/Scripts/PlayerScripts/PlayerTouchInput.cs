﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerTouchInput : MonoBehaviour
{

    public float movementSpeed = 500f;
    public float drag = 2f;
    public float terminalRotationSpeed = 25f;
    public Vector3 MoveVector { get; set; }
    public VirtualMovementJoystick moveJoystick;
    public VirtualAimingJoystick aimJoystick;

    public float cooldownTimer;
    private Animator myAnimator;
    private Rigidbody myRigidbody;
    private float originalMovementspeed;

    Vector3 tmpAimVector = new Vector3(0, 0, 1);

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.maxAngularVelocity = terminalRotationSpeed;
        myRigidbody.drag = drag;
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        MoveVector = PoolInput();

        Move();

        //Timer for movementspeed PowerUp
        #region PowerUp Update
        if (cooldownTimerStarted == true)
        {
            if (cooldownTimer >= 0)
            {
                cooldownTimer -= Time.deltaTime;
            }
            else if (cooldownTimer <= 0)
            {
                ResetPowerUps();
                cooldownTimer = 0;
            }
        }
        #endregion

    }

    void Move()
    {
        float translate = movementSpeed * Time.deltaTime;
        myRigidbody.AddForce(MoveVector * translate);

        if (MoveVector == Vector3.zero)
        {
            myRigidbody.velocity = Vector3.zero;
        }

        Vector3 point = transform.position + aimJoystick.inputVector;

        if (aimJoystick.initialInput)
        {
            transform.LookAt(point);
            //transform.LookAt(new Vector3(aimJoystick.angle.x, 0, aimJoystick.angle.y));
        }
        WalkAnimation();
    }

    private Vector3 PoolInput()
    {
        Vector3 dir = Vector3.zero;

        dir.x = moveJoystick.Horizontal();
        dir.z = moveJoystick.Vertical();

        if (dir.magnitude > 1)
        {
            dir.Normalize();
        }

        return dir;
    }

    private void WalkAnimation()
    {
        if (!(aimJoystick.inputVector == Vector3.zero))
        {
            tmpAimVector = aimJoystick.inputVector;
        }

        float vR = Mathf.Acos((MoveVector.x * tmpAimVector.x + MoveVector.y * tmpAimVector.y + MoveVector.z * tmpAimVector.z) / (Mathf.Sqrt(Mathf.Pow(MoveVector.x, 2) + Mathf.Pow(MoveVector.y, 2) + Mathf.Pow(MoveVector.z, 2)) * Mathf.Sqrt(Mathf.Pow(tmpAimVector.x, 2) + Mathf.Pow(tmpAimVector.y, 2) + Mathf.Pow(tmpAimVector.z, 2))));


        if (myRigidbody.velocity == Vector3.zero)
        {

            myAnimator.SetFloat("ForwardMomentum", 0);
            myAnimator.SetFloat("RightMomentum", 0);


        }
        else
        {
            myAnimator.SetFloat("ForwardMomentum", Mathf.Cos(vR));
            myAnimator.SetFloat("RightMomentum", Mathf.Sin(vR));
        }
    }
    PowerUpScript tempPowerup;
    bool cooldownTimerStarted;
    public void ChangeMovementspeed(Collision collision)
    {
        originalMovementspeed = movementSpeed;
        tempPowerup = collision.gameObject.GetComponent<PowerUpScript>();
        movementSpeed += tempPowerup.movementspeedBonus;
        Debug.Log(movementSpeed);
        //Sets the coolDown timer to 5 seconds
        cooldownTimer = 5;
        cooldownTimerStarted = true;
    }

    private void ResetPowerUps()
    {
        movementSpeed = originalMovementspeed;
        cooldownTimerStarted = false;
    }
    //rateOfFire = 1; //tempPowerup.rateOfFireBonus + rateOfFire
}
