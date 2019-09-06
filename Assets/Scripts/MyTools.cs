using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTools
{
    public static Vector3 yPlaneVector(Vector3 vector)
    {
        return new Vector3(vector.x, 0f, vector.z);
    }

    public static Vector3 yPlaneVector(Vector3 vector, float y)
    {
        return new Vector3(vector.x, y, vector.z);
    }

    public static Vector3 zPlaneVector(Vector3 vector)
    {
        return new Vector3(vector.x, vector.y, 0f);
    }
    public static Vector3 zPlaneVector(Vector3 vector, float z)
    {
        return new Vector3(vector.x, vector.y, z);
    }
}
