﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OpressingBike : Vehicle
{
    [SerializeField]
    private ParticleSystem trail;
    [SerializeField]
    private float VerticalTorqueMax, HorizontalTorqueMax, TractionMax, liftingCoefficient, maxLiftingForce, torqueToVelocityCoefficient;
    [SerializeField][Range(0f, 1f)]
    private float accelerationStep;
    [SerializeField][Range(0f, 1f)]
    private float accelerationBrakeStep;
    [SerializeField][Range(0f, 1f)]
    private float accelerationLoss;
    [SerializeField][Range(0f,1f)]
    private float initialLiftingRatio;

    [SerializeField]
    private GUIStyle customGizmosGUIStyle;

    private float hor_inclination, vert_inclination, speedRatio;

    

    public float SpeedRatio
    {
        get { return speedRatio; }
        private set { }
    }

    void Awake()
    {
        hor_inclination = 0; vert_inclination = 0; speedRatio = 0;
    }
    
    void Update()
    {
        hor_inclination = Input.GetAxis("Horizontal");
        vert_inclination = Input.GetAxis("Vertical");
        if (Input.GetAxis("Accelerate") != 0)
            speedRatio += accelerationStep * Time.deltaTime;
        else if (Input.GetAxis("Brake") != 0)
            speedRatio -= accelerationBrakeStep * Time.deltaTime;
        else
            speedRatio += accelerationLoss * Time.deltaTime * -Mathf.Sign(SpeedRatio);
        speedRatio = Mathf.Clamp(speedRatio, -1f,1f);
        var main = trail.main;
        main.startSpeed = Mathf.Clamp01(speedRatio) * 2.1f;
    }

    private void FixedUpdate()
    {
        Vector3 forward = transform.forward;
        Vector3 velocity = rb.velocity;

        //TODO: Make ALL the things below MASS dependent( so that some of the constants can go away)
        
        //TODO: Make velocity dependent
        //control the rotation
        rb.AddRelativeTorque(Vector3.right * vert_inclination * VerticalTorqueMax);
        rb.AddRelativeTorque(Vector3.back * hor_inclination * HorizontalTorqueMax);

        //TODO: Make velocity dependent
        //head the nose alnong the velocity vector
        float velocityMagnitude = velocity.magnitude;
        rb.AddTorque(Vector3.Cross(forward, velocity) * torqueToVelocityCoefficient);// * Mathf.Sin(Vector3.Angle(forward, velocity) / 360f * Mathf.PI)

        //the lifting force 
        float dot = Vector3.Dot(forward, velocity);
        if (velocityMagnitude != 0)
            rb.AddForce((forward * dot - velocity) * dot / velocityMagnitude * (AngleSmaller(forward, velocity, 90f) ? 1f : 0f) * liftingCoefficient * Mathf.Clamp01(initialLiftingRatio + speedRatio));
        
        //TODO: Make the negative(braking) force velocity dependent, to prevent fixing in the midair
        //the traction force
        if ( speedRatio > 0 || AngleSmaller(forward, velocity, 90f))
            rb.AddRelativeForce(Vector3.forward * speedRatio * TractionMax);

    }

    private bool AngleSmaller(Vector3 a, Vector3 b, float angle)
    {
        return Vector3.Angle(a, b) < angle;
    }
    private void OnDrawGizmos()
    {
        if (rb != null && rb.velocity.magnitude > 0.5f)
        {
            UnityEditor.Handles.Label(transform.position - (transform.right - transform.up) * 2f, "Velocity: " + DisplayVelocity.magnitude.ToString(), customGizmosGUIStyle);
            UnityEditor.Handles.ArrowHandleCap(0, transform.position, Quaternion.FromToRotation(Vector3.forward, rb.velocity), 3f, EventType.Repaint);
        }
    }

    public override void TurnOn()
    {
        trail.Play();
        base.TurnOn();
    }

    public override void TurnOff()
    {
        trail.Stop();
        base.TurnOff();
    }
}
