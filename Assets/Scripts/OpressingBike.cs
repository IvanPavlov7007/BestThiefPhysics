using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OpressingBike : MonoBehaviour
{
    private Rigidbody rb;
    private float hor_inclination, vert_inclination, speedRatio;
    public float VerticalTorqueMax, HorizontalTorqueMax, TractionMax, displayVelocity;

    public GUIStyle customGizmosGUIStyle;

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
        Debug.Log(Vector3.Dot(rb.velocity, transform.forward));
        //Debug.DrawRay(transform.position, rb.velocity, Color.green);
        displayVelocity = rb.velocity.magnitude;
    }

    //[Range(0,1f)]
    public float k, maxLiftingForce;
    private void FixedUpdate()
    {
        rb.MoveRotation( Quaternion.AngleAxis(hor_inclination * HorizontalTorqueMax, -transform.forward)
            * Quaternion.AngleAxis(vert_inclination * VerticalTorqueMax, transform.right) * rb.rotation);
        /*rb.AddRelativeForce(Vector3.forward * speedRatio * TractionMax);
        float velocityModule = rb.velocity.magnitude;
        rb.AddRelativeForce(Vector3.up * (k - vert_inclination) * speedRatio * maxLiftingForce);*/
        float dot = Vector3.Dot(transform.forward, rb.velocity);
        float velocityMagnitude = rb.velocity.magnitude;
        if (velocityMagnitude != 0)
            rb.AddForce((transform.forward * dot - rb.velocity) * k * dot / velocityMagnitude);
        //rb.velocity = Vector3.Lerp(rb.velocity, transform.forward.normalized * velocityModule, speedRatio);
        /*rb.AddForce( transform.forward * speedRatio * TractionMax);
        //rb.AddRelativeTorque(Vector3.left * k * rb.velocity.magnitude);
        */

    }
    private void OnDrawGizmos()
    {
        
        UnityEditor.Handles.Label(transform.position - (transform.right - transform.up) * 2f, "Velocity: " + displayVelocity.ToString(), customGizmosGUIStyle);
        if(rb != null)
            UnityEditor.Handles.ArrowHandleCap(0, transform.position, Quaternion.FromToRotation(Vector3.forward, rb.velocity), 3f, EventType.Repaint);
    }
}
