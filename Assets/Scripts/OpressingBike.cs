using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OpressingBike : MonoBehaviour
{
    private Rigidbody rb;
    private float hor_inclination, vert_inclination, speedRatio;
    public float VerticalTorqueMax, HorizontalTorqueMax, TractionMax;
    void Áwake()
    {
        hor_inclination = 0; vert_inclination = 0; speedRatio = 0;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        hor_inclination = Input.GetAxis("Horizontal");
        vert_inclination = Input.GetAxis("Vertical");
        speedRatio = Input.GetAxis("Accelerate") - Input.GetAxis("Brake");
    }

    //[Range(0,1f)]
    public float k;
    private void FixedUpdate()
    {
        rb.AddRelativeTorque(Vector3.back * hor_inclination * HorizontalTorqueMax);
        rb.AddRelativeTorque(Vector3.right * vert_inclination * VerticalTorqueMax * rb.velocity.magnitude);
        rb.AddForce( transform.forward * speedRatio * TractionMax);
        //rb.AddRelativeTorque(Vector3.left * rb.velocity.magnitude * Vector3.Angle(transform.forward, Vector3.up) * k);
    }
}
