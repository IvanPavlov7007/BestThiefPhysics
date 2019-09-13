using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : FollowingObject
{
    public Transform SeatPlace;
    public bool StraightCameraControl, canControlCamera;
    protected Rigidbody rb;

    virtual protected void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public Vector3 DisplayVelocity
    {
        get {
                return rb == null? Vector3.zero : rb.velocity; }
        protected set { rb.velocity = value; }
    }

    virtual public void TurnOff()
    {
        enabled = false;
    }

    virtual public void TurnOn()
    {
        enabled = true;
    }
}
