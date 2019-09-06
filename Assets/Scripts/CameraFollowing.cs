using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    public Vehicle vehicle;
    public Transform focus;

    [SerializeField]
    private float smoothTime, rotateRatio, rotationSensitivity, fieldOfViewFromVelocityCoefficient;
    [SerializeField]
    [Range(0f, 180f)]
    private float minFieldOfView, maxFieldOfView;

    private float fieldOfViewDifference;
    private Vector3 focusCamDeltaPos, smoothVelocity;
    private Quaternion focusCamDeltaRot, mouseDeltaRot;
    private Camera cam;

    private void Awake()
    {
        fieldOfViewDifference = maxFieldOfView - minFieldOfView;
    }
    private void Start()
    {
        focusCamDeltaPos = transform.position - focus.position;
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        Vector3 direction = (Input.mousePosition - new Vector3(Screen.width / 2f, Screen.height / 2f)) * rotationSensitivity;
        mouseDeltaRot = Quaternion.Euler(-direction.y, direction.x, 0f);
    }
    private void LateUpdate()
    {
        if(vehicle != null)
            cam.fieldOfView = minFieldOfView + vehicle.DisplayVelocity / fieldOfViewFromVelocityCoefficient * fieldOfViewDifference;
    }

    //TODO: Make the following only in LateUpdate()
    private void FixedUpdate()
    {
        Vector3 targetPosition = focus.position + focus.rotation * mouseDeltaRot * focusCamDeltaPos;
        transform.SetPositionAndRotation(Vector3.SmoothDamp(transform.position, targetPosition, ref smoothVelocity, smoothTime),
                                         Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(focus.position - transform.position, focus.up), rotateRatio)
            ) ;
    }
}
