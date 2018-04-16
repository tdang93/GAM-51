using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour {
    public List<GameObject> Points = new List<GameObject>();
    public List<GameObject> Spheres = new List<GameObject>();
    public List<GameObject> AABBs = new List<GameObject>();

    public void FixedUpdate() {
        // POINT TO SPHERE TESTING
        foreach (GameObject point in Points) {
            foreach (GameObject sphere in Spheres) {
                PointToSphere(point, sphere);
            }
        }

        // SPHERE TO SPHERE TESTING
        foreach (GameObject sphere1 in Spheres) {
            foreach (GameObject sphere2 in Spheres) {
                if(sphere1 != sphere2) { // Make sure spheres are not the same GameObject
                    SphereToSphere(sphere1, sphere2);
                }
            }
        }

        // POINT TO SPHERE TESTING
        foreach (GameObject point in Points) {
            foreach (GameObject sphere in Spheres) {
                PointToSphere(point, sphere);
            }
        }

        // AABB TO AABB TESTING
        foreach (GameObject AABB1 in AABBs) {
            foreach (GameObject AABB2 in AABBs) {
                if(AABB1 != AABB2) { // Make sure spheres are not the same GameObject
                    AABBToAABB(AABB1, AABB2);
                }
            }
        }

        // AABB TO SPHERE TESTING
        foreach(GameObject sphere in Spheres) {
            foreach(GameObject AABB in AABBs) {
                SphereToAABB(sphere, AABB);
            }
        }

        Debug.Log("Collision: FixedUpdate()");
    }

    // POINT TO SPHERE COLLISION DETECTION
    public static void PointToSphere(GameObject point, GameObject sphere) {
        Vector3 pointPosition = point.transform.position; // center of "point" GameObject
        Vector3 spherePosition = sphere.transform.position; // center of "sphere" GameObject

        float sphereRadius = sphere.transform.localScale.magnitude / 2;
        float distance = Mathf.Abs((spherePosition - pointPosition).magnitude);

        if(distance <= sphereRadius) {
            Debug.Log("Collision! (Point: " + point.name + " and Sphere: " + sphere.name);
        }
    }

    // SPHERE TO SPHERE COLLISION
    public static void SphereToSphere(GameObject sphere1, GameObject sphere2) {
        Vector3 sphere1position = sphere1.transform.position; // center of "sphere1" GameObject
        Vector3 sphere2position = sphere2.transform.position; // center of "sphere2" GameObject

        float sphere1Radius = sphere1.transform.localScale.magnitude / 2;
        float sphere2Radius = sphere2.transform.localScale.magnitude / 2;
        float distance = Mathf.Abs((sphere2position - sphere1position).magnitude);

        if(distance <= sphere1Radius || distance <= sphere2Radius) {
            Debug.Log("Collision! (Sphere: " + sphere1.name + " and Sphere: " + sphere2.name);
        }
    }

    // Point to AABB COLLISION
    public static void PointToAABB(GameObject point, GameObject AABB) {
        Vector3 pointPosition = point.transform.position;
        Vector3 AABBPosition = AABB.transform.position;

        Vector3 AABBOffset = new Vector3(AABB.transform.localScale.x, AABB.transform.localScale.y,
            AABB.transform.localScale.z);
        Vector3 AABBMin = AABBPosition - AABBOffset;
        Vector3 AABBMax = AABBPosition + AABBOffset;

        if(pointPosition.x > AABBMin.x && pointPosition.y > AABBMin.y && pointPosition.z > AABBMin.z) {
            // If within lower bounds of AABBMin vertex
            if(pointPosition.x < AABBMax.x && pointPosition.y < AABBMax.y && pointPosition.z < AABBMax.z) {
                // If within upper bounds of AABBMax vertex
                Debug.Log("Collision! Point: " + point.name + " and AABB: " + AABB.name);
            }
        }
    }

    public static void AABBToAABB(GameObject AABB1, GameObject AABB2) {
        Vector3 AABB1Position = AABB1.transform.position;
        Vector3 AABB2Position = AABB2.transform.position;

        Vector3 AABB1Offset = new Vector3(AABB1.transform.localScale.x, AABB1.transform.localScale.y,
            AABB1.transform.localScale.z);
        Vector3 AABB1Min = AABB1Position - (AABB1Offset / 2);
        Vector3 AABB1Max = AABB1Position + (AABB1Offset / 2);

        Vector3 AABB2Offset = new Vector3(AABB2.transform.localScale.x, AABB2.transform.localScale.y,
            AABB2.transform.localScale.z);
        Vector3 AABB2Min = AABB2Position - (AABB2Offset / 2);
        Vector3 AABB2Max = AABB2Position + (AABB2Offset / 2);

        if(AABB1Min.x < AABB2Max.x && AABB1Min.y < AABB2Max.y && AABB1Min.z < AABB2Max.z) { // AABB1Min < AABB2Max
            if(AABB2Min.x < AABB1Max.x && AABB2Min.y < AABB1Max.y && AABB2Min.z < AABB1Max.z) { // AABB2Min < AABB1Max
                Debug.Log("Collision! AABB: " + AABB1.name + " and AABB: " + AABB2.name);
            }
        }
    }

    public static void SphereToAABB(GameObject sphere, GameObject AABB) {
        Vector3 spherePosition = sphere.transform.position;
        Vector3 AABBPosition = AABB.transform.position;

        float sphereRadius = sphere.transform.localScale.magnitude / 2;

        float AABBOffsetX = AABB.transform.localScale.x;
        float AABBOffsetY = AABB.transform.localScale.y;
        float AABBOffsetZ = AABB.transform.localScale.z;

        Vector3 AABBCornerPosition;
        for(int i = 0; i < 1; i++) {
            AABBCornerPosition.x = AABBPosition.x - AABBOffsetX; // negative X
            if(i == 0) {
                AABBCornerPosition.x *= -1; // positive X
            } 
            for(int j = 0; j < 1; j++) {
                AABBCornerPosition.y = AABBPosition.y - AABBOffsetY; // negative Y
                if(i == 0) {
                    AABBCornerPosition.y *= -1; // positive Y
                }
                for(int k = 0; k < 1; k++) {
                    AABBCornerPosition.z = AABBPosition.z - AABBOffsetZ; // negative Z
                    if(i == 0) {
                        AABBCornerPosition.z *= -1; // positive Z
                    }

                    // Check for collision
                    float distance = (spherePosition - AABBCornerPosition).magnitude;
                    if(distance <= sphereRadius) {
                        Debug.Log("Collision! AABB: + " + AABB.name + " and Sphere: " + sphere.name);
                    }
                }
            }
        }
    }
}
