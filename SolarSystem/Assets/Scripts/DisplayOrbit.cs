using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DisplayOrbit : MonoBehaviour
{
    public int numSteps;

    public static List<VirtualBody> virtBodies;

    public bool relativeToBody;
    public CelestialBody centralBody;
    // Start is called before the first frame update
    private void Start()
    {
        DrawOrbits();
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        DrawOrbits();
    }

    private void DrawOrbits()
    {
        Vector3 referenceBodyInitPos = Vector3.zero;
        virtBodies = new List<VirtualBody>();
        Vector3[][] drawPoints = new Vector3[CelestialBody.bodies.Count][];

        //Checks if body is the central body and applies the transformation to orbits
        for(int i = 0; i < CelestialBody.bodies.Count; i++)
        {
            CelestialBody body = CelestialBody.bodies.ElementAt(i);
            virtBodies.Add(new VirtualBody(body));
            drawPoints[i] = new Vector3[numSteps];
            if(body == centralBody && relativeToBody)
            {
                referenceBodyInitPos = body.transform.position;
            }
        }

        

        //simulates movement of the virtual body
        for (int i = 0; i < numSteps; i++)
        {
            Vector3 referenceBodyPos = relativeToBody ? centralBody.transform.position : Vector3.zero;
            //run gravity function for each virtual body
            for (int j = 0; j < virtBodies.Count; j++)
            {
                VirtualBody currentBod = virtBodies.ElementAt(j);
                currentBod.velocity = PredictGravity(j);
            }
            for(int j = 0; j < virtBodies.Count; j++)
            {
                VirtualBody currentBod = virtBodies.ElementAt(j);

                //Equivilent of adding force but also saves the points in drawpoints
                Vector3 newPos = currentBod.position + currentBod.velocity * Time.deltaTime;
                currentBod.position = newPos;
                //Adjusts for relative body
                if (relativeToBody)
                {
                    Vector3 offset = referenceBodyPos - referenceBodyInitPos;
                    newPos -= offset;
                }
                if(relativeToBody && CelestialBody.bodies.ElementAt(j) == centralBody)
                {
                    newPos = referenceBodyInitPos;
                }
                drawPoints[j][i] = newPos;
            }

            //draw paths
            for(int j = 0; j < virtBodies.Count; j++)
            {
                
                LineRenderer line = CelestialBody.bodies.ElementAt(j).gameObject.GetComponentInChildren<LineRenderer>();
                line.positionCount = drawPoints[j].Length;
                line.SetPositions(drawPoints[j]);
                Debug.Log(drawPoints[j]);
                line.enabled = true;
            }

        }

    }

    private Vector3 PredictGravity(int pos)
    {
        Vector3 gForce = Vector3.zero;
        for (int i = 0; i < virtBodies.Count ; i++)
        {
            if (pos == i)
            {
                continue;
            }
            Vector3 direction = virtBodies.ElementAt(i).position - virtBodies.ElementAt(pos).position;
            float distance = direction.magnitude;

            if (distance == 0)
            {
                continue;
            }

            //Works out newtons law of gravitaion
            float gForceMagnitude = CelestialBody.GRAV_CONST * ((virtBodies.ElementAt(i).mass * virtBodies.ElementAt(pos).mass) / Mathf.Pow(distance, 2));
            gForce = direction.normalized * gForceMagnitude;
        }
        return gForce;
    }
}
