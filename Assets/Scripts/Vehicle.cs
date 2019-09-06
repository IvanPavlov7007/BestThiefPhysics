using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    public Transform SeatPlace;
    protected Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public float DisplayVelocity
    {
        get;
        protected set;
    }
}
