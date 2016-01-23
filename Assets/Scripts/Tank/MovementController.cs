using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {

    [SerializeField]
    private float sideSpeed = 10f;
     [SerializeField]
    private float forwardSpeed = 0.2f;


     private void FixedUpdate()
     {
         float h = Input.GetAxis("Horizontal") * sideSpeed * Time.deltaTime;
         float v = Input.GetAxis("Vertical") * forwardSpeed * Time.deltaTime;
         rigidbody.AddTorque(transform.up * h);
         rigidbody.AddForce(transform.forward * v);
     }
}
