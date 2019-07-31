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
        rb.MoveRotation( Quaternion.AngleAxis(hor_inclination * HorizontalTorqueMax, -transform.forward)
            * Quaternion.AngleAxis(vert_inclination * VerticalTorqueMax, transform.right) * rb.rotation);
        rb.AddForce( transform.forward * speedRatio * TractionMax);
        rb.AddForce(Quaternion.AngleAxis(180f, transform.forward) * rb.velocity * k);
        //rb.AddRelativeTorque(Vector3.left * k * rb.velocity.magnitude);
    }
}
