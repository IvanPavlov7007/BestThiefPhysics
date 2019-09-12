﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    public HumanoidController humanoidController;

    [SerializeField]
    private float smoothTime, rotateRatio, rotationSensitivity, gamePadRotationSensitivity, resetRotationTime, fieldOfViewFromVelocityCoefficient;
    [SerializeField]
    [Range(0f, 180f)]
    private float minFieldOfView, maxFieldOfView;

    private Vehicle vehicle;
    private Transform focus;
    private float fieldOfViewDifference, idle_t;
    private Vector3 focusCamDeltaPos, smoothVelocity;
    private Quaternion focusCamDeltaRot, mouseDeltaRot;
    private Camera cam;

    private void Awake()
    {
        fieldOfViewDifference = maxFieldOfView - minFieldOfView;
    }
    private void Start()
    {
        cam = GetComponent<Camera>();
        humanoidController.onDisembark += HandleHumanoidDisembark;
        humanoidController.onSeat += HandleHumanoidSeat;
        idle_t = 0f;
        resetFocus(humanoidController);
    }

    public void resetFocus(FollowingObject obj)
    {
        focus = obj.Focus;
        focusCamDeltaPos = Quaternion.Inverse(focus.rotation) * (obj.CameraPosition.position - focus.position);
        mouseDeltaRot = Quaternion.identity;
    }

    private void Update()
    {
        /*Vector3 direction = (Input.mousePosition - new Vector3(Screen.width / 2f, Screen.height / 2f)) * rotationSensitivity;
        mouseDeltaRot = Quaternion.Euler(-direction.y, direction.x, 0f);*/
        float x = 0, y = 0;
        if (vehicle == null || vehicle.canControlCamera)
        { 
            x = Input.GetAxis("Rotation Y GP") * gamePadRotationSensitivity;
            y = Input.GetAxis("Rotation X GP") * gamePadRotationSensitivity;
        }
        if (x == 0f && y == 0f)
        {
            x = -Input.GetAxis("Rotation Y") * rotationSensitivity;
            y = Input.GetAxis("Rotation X") * rotationSensitivity;
        }

        if (x == 0f && y == 0f)
        {
            idle_t += Time.deltaTime;
            if (idle_t >= resetRotationTime)
                resetFocus(vehicle == null ? humanoidController as FollowingObject : vehicle as FollowingObject);
        }
        else
            idle_t = 0f;

        mouseDeltaRot *= Quaternion.Euler(x, y, 0);
    }
    private void LateUpdate()
    {
        //TODO: Make the following work in LateUpdate()
        /*Vector3 targetPosition = focus.position + focus.rotation * mouseDeltaRot * focusCamDeltaPos;
        transform.SetPositionAndRotation(Vector3.SmoothDamp(transform.position, targetPosition, ref smoothVelocity, smoothTime),
                                         Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(focus.position - transform.position, focus.up), rotateRatio)
            ) ;*/

        /*Vector3 targetPosition = focus.position + Quaternion.LookRotation(MyTools.yPlaneVector(focus.forward), Vector3.up) * mouseDeltaRot * focusCamDeltaPos;
        transform.SetPositionAndRotation(targetPosition, Quaternion.LookRotation(focus.position - targetPosition, Vector3.up));*/
        Vector3 forward, up;
        Quaternion focusRotation;

        if (vehicle != null && !vehicle.StraightCameraControl)
        {
            focusRotation = focus.rotation;
            up = focus.up;
        }
        else
        {
            if (vehicle != null && vehicle.DisplayVelocity.magnitude > 5f)
                forward = vehicle.DisplayVelocity;
            else
                forward = MyTools.yPlaneVector(focus.forward);
            focusRotation = Quaternion.LookRotation(forward, Vector3.up);
            up = Vector3.up;
        }

        Vector3 targetPosition = focus.position + focusRotation * mouseDeltaRot * focusCamDeltaPos;
        transform.SetPositionAndRotation(targetPosition, Quaternion.LookRotation(focus.position - targetPosition, up));

        if (vehicle != null)
            cam.fieldOfView = minFieldOfView + vehicle.DisplayVelocity.magnitude / fieldOfViewFromVelocityCoefficient * fieldOfViewDifference;
    }

    
    private void FixedUpdate()
    {

    }

    private void HandleHumanoidSeat(Vehicle vehicle)
    {
        this.vehicle = vehicle;
        resetFocus(vehicle);
    }

    private void HandleHumanoidDisembark()
    {
        vehicle = null;
        resetFocus(humanoidController);
    }
}
