using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraFollowing : MonoBehaviour
{
    public float CamDistance, smoothTime, rotateRatio;

    public Transform focus;
    Vector3 focusCamDeltaPos;
    Quaternion focusCamDeltaRot;
    private void Awake()
    {
        
    }
    private void Start()
    {
        focusCamDeltaPos = transform.position - focus.position;
        CamDistance = focusCamDeltaPos.magnitude;
        //playerCamDeltaRot = player
    }

    public void SetTargerPosition()
    {

    }
    Vector3 smoothVelocity;
    //TODO: Make the following of the cam smooth in LateUpdate()
    private void FixedUpdate()
    {
        Vector3 targetPosition = focus.position + focus.rotation * focusCamDeltaPos;
        Debug.DrawRay(focus.position, focus.rotation * focusCamDeltaPos ,Color.red);
        transform.SetPositionAndRotation(Vector3.SmoothDamp(transform.position, targetPosition, ref smoothVelocity, smoothTime),
                                         Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(focus.position - transform.position, focus.up), rotateRatio)
            ) ;
    }
}
