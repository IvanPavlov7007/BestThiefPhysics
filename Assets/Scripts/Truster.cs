using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truster : Vehicle
{
    public Transform LeftMotor;
    public Transform RightMotor;

    [SerializeField]
    private float maxRotation, maxForce, torqueKoefficient, bop_duration, bop_length;
    private Quaternion lm_rotation, rm_rotation;
    private Vector3 init_lm_position, init_rm_position;

    private float lm_force, rm_force;

    private void Awake()
    {
        lm_rotation = LeftMotor.localRotation;
        rm_rotation = RightMotor.localRotation;
        init_lm_position = LeftMotor.localPosition;
        init_rm_position = RightMotor.localPosition;
    }

    private Coroutine lbopCoroutine, rbopCoroutine;
    private void Update()
    {
        float xrot = Input.GetAxis("LM_Horizontal");
        float yrot = Input.GetAxis("LM_Vertical");
        //LeftMotor.rotation = Quaternion.LookRotation(Vector3.up, Vector3.Cross(Vector3.up, transform.right));
        //LeftMotor.localRotation *= Quaternion.Euler(-yrot * maxRotation, xrot * maxRotation, 0f);
        LeftMotor.localRotation = lm_rotation * Quaternion.Euler(-yrot * maxRotation, xrot * maxRotation, 0f);
        xrot = Input.GetAxis("RM_Horizontal");
        yrot = Input.GetAxis("RM_Vertical");
        //RightMotor.rotation = Quaternion.LookRotation(Vector3.up, -Vector3.Cross(Vector3.up, transform.right));
        //RightMotor.localRotation *= Quaternion.Euler(yrot * maxRotation, -xrot * maxRotation, 0f);
        RightMotor.localRotation = rm_rotation * Quaternion.Euler(yrot * maxRotation, -xrot * maxRotation, 0f);

        if (Input.GetButtonDown("LM_Bop"))
        {
            if (lbopCoroutine != null)
                StopCoroutine(lbopCoroutine);
            lbopCoroutine = StartCoroutine(bop(LeftMotor, init_lm_position));
        }
        if (Input.GetButtonDown("RM_Bop"))
        {
            if (rbopCoroutine != null)
                StopCoroutine(rbopCoroutine);
            rbopCoroutine = StartCoroutine(bop(RightMotor, init_rm_position));
        }

        lm_force = Input.GetAxis("LM_Thrust");
        rm_force = Input.GetAxis("RM_Thrust");
    }

    IEnumerator bop(Transform motor, Vector3 init_position)
    {
        float t = 0f;
        while (t < bop_duration)
        {
            motor.localPosition = init_position + Mathf.Sin(Mathf.PI * t / bop_duration) * Vector3.down * bop_length;
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime;
        }
        motor.localPosition = init_position;
    }
    private void FixedUpdate()
    {
        rb.AddTorque(Vector3.Cross(-transform.up, Vector3.down) * torqueKoefficient);
        rb.AddForceAtPosition(LeftMotor.forward * maxForce * lm_force, LeftMotor.position);
        rb.AddForceAtPosition(RightMotor.forward * maxForce * rm_force, RightMotor.position);
    }
}
