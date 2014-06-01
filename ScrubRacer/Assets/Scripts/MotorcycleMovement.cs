using UnityEngine;
using System.Collections;

public class MotorcycleMovement : MonoBehaviour
{
    public float AccelerationRate;
    public float Drag;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        var forward = this.transform.forward;
        Debug.DrawRay(this.transform.position, forward * 10.0f, Color.red);

        var acceleration = forward * GetAcceleration();
        this.rigidbody.AddForce(acceleration, ForceMode.Acceleration);
	}

    private float GetAcceleration()
    {
        var accelerationInput = Input.GetAxis("Forward") - Input.GetAxis("Reverse");

        return this.AccelerationRate * accelerationInput;
    }
}
