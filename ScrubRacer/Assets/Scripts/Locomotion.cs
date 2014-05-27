using UnityEngine;
using System.Collections;

public class Locomotion : MonoBehaviour
{
    public float MaxSpeed;
    public float AccelerationRate;
    public float MaxTurnAngle;
    public float TurnRate;
    public float TurboRate;
    public float TurboTime;
    public float TurboCooldown;

    private Quaternion wheelRotationLS;
    private float turboTimer;
    private float turboCooldownTimer;

	void Start ()
    {
        this.wheelRotationLS = Quaternion.identity;
        this.turboTimer = 0.0f;
        this.turboCooldownTimer = 0.0f;
	}

    private Vector3 velocity = Vector3.zero;

	void FixedUpdate ()
    {
        var forward = GetWheelDirection();
        Debug.DrawRay(transform.position, this.transform.forward * 10.0f, Color.red);

        // Positive when moving forward, negative when moving backward, zero when still.
        var acceleration = forward * GetAcceleration();

        this.rigidbody.AddForce(acceleration);

        var slerpRotation = Quaternion.Slerp(Quaternion.identity, wheelRotationLS, Time.deltaTime);
        var rotation = this.transform.localRotation * slerpRotation;
        
        this.rigidbody.MoveRotation(rotation); 
    }

    private Vector3 GetWheelDirection()
    {
        var steerDirection = Input.GetAxis("Horizontal");
        var targetAngle = steerDirection * this.MaxTurnAngle;

        var targetAngleRotation = Quaternion.Euler(0.0f, targetAngle, 0.0f);
        this.wheelRotationLS = Quaternion.Slerp(this.wheelRotationLS, targetAngleRotation, Time.deltaTime * this.TurnRate);

        var targetForward = ProjectVecToSurface(targetAngleRotation * this.transform.forward);
        Debug.DrawRay(this.transform.position, 10.0f * targetForward, Color.blue);

        return ProjectVecToSurface(this.wheelRotationLS * this.transform.forward); 
    }

    private float GetAcceleration()
    {
        var accelerationInput = Input.GetAxis("Forward") - Input.GetAxis("Reverse");

        return (GetTurboAcceleration() + this.AccelerationRate) * accelerationInput;
    }

    private float GetTurboAcceleration()
    {
        this.turboCooldownTimer = Mathf.Max(0.0f, this.turboCooldownTimer - Time.deltaTime);

        var rate = 0.0f;
        if (this.turboTimer > 0.0f)
        {
            this.turboTimer = Mathf.Max(0.0f, this.turboTimer - Time.deltaTime);
            rate = this.TurboRate;
        }

        if (Input.GetButton("Turbo") && this.turboCooldownTimer <= 0.0f)
        {
            this.turboTimer = this.TurboTime;
            this.turboCooldownTimer = this.TurboCooldown;
        }

        return rate;
    }

    private Vector3 ProjectVecToSurface(Vector3 forward)
    {
        var ray = new Ray(this.transform.position, Vector3.down);
        RaycastHit hit;

        float colliderHeight = this.collider.transform.localScale.y * this.collider.bounds.size.y;
        if (!Physics.Raycast(ray, out hit, (colliderHeight / 2.0f * 1.05f)))
        {
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
