using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class HumanoidController : MonoBehaviour
{
    [SerializeField]
    private float legPushingTime, legPushingforce;
    private Animator animator;
    private Rigidbody rb;
    private int horzFloat = Animator.StringToHash("Horz");
    private int vertFloat = Animator.StringToHash("Vert");
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat(horzFloat, Input.GetAxis("Horizontal"), 0.2f, Time.deltaTime);
        animator.SetFloat(vertFloat, Input.GetAxis("Vertical"), 0.2f, Time.deltaTime);
    }
}
