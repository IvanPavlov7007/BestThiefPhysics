using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Landscape : MonoBehaviour
{
    public GameObject CubeMountain;
    public float distanceBetweenMountains;
    public int iterations;
    private void Start()
    {
        float x0 = transform.position.x;
        float z0 = transform.position.z;
        for(int i = 1; i <= iterations; i++)
        {
            Instantiate(CubeMountain, new Vector3(x0 + i * distanceBetweenMountains, 0, z0),CubeMountain.transform.rotation);
            Instantiate(CubeMountain, new Vector3(x0 - i * distanceBetweenMountains, 0, z0), CubeMountain.transform.rotation);
            Instantiate(CubeMountain, new Vector3(x0, 0, z0 + i * distanceBetweenMountains), CubeMountain.transform.rotation);
            Instantiate(CubeMountain, new Vector3(x0, 0, z0 - i * distanceBetweenMountains), CubeMountain.transform.rotation);
            Instantiate(CubeMountain, new Vector3(x0 + i * distanceBetweenMountains, 0, z0 + i * distanceBetweenMountains), CubeMountain.transform.rotation);
            Instantiate(CubeMountain, new Vector3(x0 - i * distanceBetweenMountains, 0, z0 + i * distanceBetweenMountains), CubeMountain.transform.rotation);
            Instantiate(CubeMountain, new Vector3(x0 + i * distanceBetweenMountains, 0, z0 - i * distanceBetweenMountains), CubeMountain.transform.rotation);
            Instantiate(CubeMountain, new Vector3(x0 - i * distanceBetweenMountains, 0, z0 - i * distanceBetweenMountains), CubeMountain.transform.rotation);
        }
    }
}
