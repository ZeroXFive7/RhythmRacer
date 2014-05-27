using UnityEngine;
using System.Collections;

public class ChaseCamera : MonoBehaviour
{
    public Transform target;
    public float DistanceBehind;
    public float DistanceAbove;
	
	// Update is called once per frame
	void Update ()
    {
        this.transform.position = target.position - target.forward * this.DistanceBehind + Vector3.up * this.DistanceAbove;
        this.transform.LookAt(target);
	}
}
