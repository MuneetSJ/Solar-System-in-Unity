using UnityEngine;

public class VirtualBody
{
    public string name;
    public Vector3 position;
    public Vector3 velocity;
    public float mass;

    public VirtualBody(CelestialBody celBody)
    {
        name = celBody.name;
        position = celBody.transform.position;
        velocity = celBody.body.transform.forward * celBody.velocityMultiplier;
        mass = celBody.body.mass;
    }
}
