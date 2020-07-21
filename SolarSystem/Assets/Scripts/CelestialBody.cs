using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public class CelestialBody : MonoBehaviour
{
    public const float GRAV_CONST = 6.674f;
    public static List<CelestialBody> bodies;
    public Rigidbody body;

    [Header("Body Speeds")]
    [Tooltip("Negative of opposite direction")]
    public float velocityMultiplier;
    public float spinSpeed;


    private void Start()
    {
        spinSpeed = Random.Range(5f, 50f);
    }
    private void FixedUpdate()
    {
        body.transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
        Gravity();
        
    }

    private void OnEnable()
    {
        if (bodies == null)
        {
            bodies = new List<CelestialBody>();
        }
        //adds active body to list
        bodies.Add(this);
    }

    private void OnDisable()
    {
        //remove active body from life
        bodies.Remove(this);
    }

    private void OnMouseDown()
    {
        CameraControl.cameraInstance.followTarget = transform;
        CameraControl.cameraInstance.Reset();
    }

    public void Gravity()
    {
        foreach (CelestialBody attrBody in bodies)
        {
            if (attrBody != this)
            {
                //Debug.Log("This body: " + this.name + " Is attracted to " + attrBody.name);

                Rigidbody bodyToAttract = attrBody.body;

                //finds direction towards the other body and the distance between them
                Vector3 direction = bodyToAttract.position - body.position;
                float distance = direction.magnitude;

                if (distance == 0)
                {
                    continue;
                }

                //Works out newtons law of gravitaion
                float gForceMagnitude = GRAV_CONST * ((bodyToAttract.mass * body.mass) / Mathf.Pow(distance, 2));
                Vector3 gForce = direction.normalized * gForceMagnitude;
                body.AddForce(gForce, ForceMode.Force);
            }
        }
    }
}
