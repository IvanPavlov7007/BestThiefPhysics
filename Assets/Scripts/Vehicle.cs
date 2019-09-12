using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : FollowingObject
{
    public Transform SeatPlace;
    public bool StraightCameraControl, canControlCamera;
    protected Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public Vector3 DisplayVelocity
    {
        get {
                return rb == null? Vector3.zero : rb.velocity; }
        protected set { rb.velocity = value; }
    }
}
