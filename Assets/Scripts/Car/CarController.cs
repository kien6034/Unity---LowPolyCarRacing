using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
  
    public Transform centerOfMass;
    private Transform handAndVolang;

    public float motorTorque = 1500f;
    public float maxSteer = 20f;
    public float maxBreak = 1500f;
    private Rigidbody _rigidbody;
    private Wheel[] wheels;
    private float speed;
    public float MAXSPEED = 1500;

    public float Steer { get; set; }
    public float Throttle { get; set; }

    public bool isBraking { get; set; }
    private void Start()
    {
        wheels = GetComponentsInChildren<Wheel>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = centerOfMass.localPosition;

        handAndVolang = transform.Find("handAndVolang ");
        Debug.Log(handAndVolang);
    }

   

    // Update is called once per frame
   

    private void Update()
    {
        Move();
        RotateVolang();
       
    }

    private void RotateVolang()
    {
        if (handAndVolang)
        {
            handAndVolang.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, -Steer * maxSteer + 5);
        }
       
    }

    private void Move()
    {
        speed = _rigidbody.velocity.sqrMagnitude;

        foreach (var wheel in wheels)
        {

            if (speed > MAXSPEED)
            {
                wheel.Torque = 0;
            }
            else
            {
                wheel.Torque = Throttle * motorTorque;
            }
            wheel.SteerAngle = Steer * maxSteer;
            


            if (isBraking)
            {
                wheel.BreakTorque = maxBreak;
            }
            else
            {
                wheel.BreakTorque = 0;
               
            }
        }


    }
}
