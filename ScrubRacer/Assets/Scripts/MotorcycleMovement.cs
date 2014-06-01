using UnityEngine;
using System.Collections;

public class MotorcycleMovement : MonoBehaviour
{
    public float AccelerationRate;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        var forward = GetForward();
        Debug.DrawRay(this.transform.position, forward * 10.0f, Color.red);

        var acceleration = forward * GetAcceleration();
        this.rigidbody.AddForce(acceleration * this.rigidbody.mass);
        Debug.Log("Velocity: " + rigidbody.velocity);
	}

    private float GetAcceleration()
    {
        var accelerationInput = Input.GetAxis("Forward") - Input.GetAxis("Reverse");

        return this.AccelerationRate * accelerationInput;
    }

    private Vector3 GetForward()
    {
        return ProjectVecToSurface(this.transform.forward);
    }

    private Vector3 ProjectVecToSurface(Vector3 forward)
    {
        var ray = new Ray(this.transform.position, Vector3.down);
        RaycastHit hit;

        float colliderHeight = this.collider.transform.localScale.y * this.collider.bounds.size.y;
        if (!Physics.Raycast(ray, out hit, (colliderHeight / 2.0f * 1.05f)))
        {
            Debug.LogWarning("Raycast failed.");
            return forward;
        }

        return ProjectVecToPlane(forward, hit.normal);
    }

    private Vector3 ProjectVecToPlane(Vector3 vector, Vector3 planeNormal)
    {
        var planeNormalNorm = planeNormal.normalized;
        var vectorNorm = vector.normalized;

        return (vectorNorm - Vector3.Dot(vectorNorm, planeNormalNorm) * planeNormalNorm).normalized;
    }
}
