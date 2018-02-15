using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyVector3 {
    // Add()


    // Scale()
    public static Vector3 Scale(Vector3 v3, float s) {
        return (v3 * s);
    }
    public static Vector3 Scale(Vector2 v2, float s) {
        return (v2 * s);
    }

    // Magnitude()
    public static float Magnitude(float x, float y, float z) {
        return Mathf.Sqrt( Mathf.Pow(x, 2) + Mathf.Pow(y, 2) + Mathf.Pow(z, 2) );
    }
    public static float Magnitude(Vector3 v3) {
        return Magnitude(v3.x, v3.y, v3.z);
    }

    public static float Magnitude(float x, float y) {
        return Mathf.Sqrt( Mathf.Pow(x, 2) + Mathf.Pow(y, 2) );
    }
    public static float Magnitude(Vector2 v2) {
        return Magnitude(v2.x, v2.y);
    }

    // Normalize()
    public static Vector3 Normalize(Vector3 v3) {
        return (v3 / Magnitude(v3));
    }

    public static Vector3 Normalize(Vector2 v2) {
        return (v2 / Magnitude(v2));
    }
}
