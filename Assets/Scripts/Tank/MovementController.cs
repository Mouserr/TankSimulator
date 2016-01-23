using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {

    [SerializeField]
    private float sideSpeed = 10f;
     [SerializeField]
    private float forwardSpeed = 0.2f;

    private Quaternion destRotation;

    public void Rotate(float angle)
    {
        destRotation = transform.localRotation * Quaternion.AngleAxis(angle, Vector3.up);
    }

     private void FixedUpdate()
     {
         float h = Input.GetAxis("Horizontal") * sideSpeed * Time.fixedDeltaTime;
         float v = Input.GetAxis("Vertical") * forwardSpeed * Time.fixedDeltaTime;
         
         rigidbody.AddForce(transform.forward * v);
         if (Mathf.Abs(h) > 0.001f)
         {
             destRotation = transform.localRotation * Quaternion.AngleAxis(h, Vector3.up);
         }
         transform.localRotation = Quaternion.Lerp(transform.localRotation, destRotation, Time.fixedDeltaTime);
     }
}
