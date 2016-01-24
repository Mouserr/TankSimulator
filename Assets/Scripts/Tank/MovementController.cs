using UnityEngine;

public class MovementController : MonoBehaviour {

    [SerializeField]
    private float sideSpeed = 10f;
     [SerializeField]
    private float forwardSpeed = 0.2f;

    private Quaternion destRotation;
    private Rigidbody mRigidbody;
    private bool grounded = false;

    private void Awake()
    {
        mRigidbody = GetComponent<Rigidbody>();
    }

    public void Rotate(float angle)
    {
        angle = Mathf.Sign(angle) * sideSpeed;
        destRotation = transform.localRotation * Quaternion.AngleAxis(angle, transform.localRotation * Vector3.up);
    }

     private void FixedUpdate()
     {
         
         if (grounded) {
            float targetRotation = Input.GetAxis("Horizontal") * sideSpeed;
	        
            Vector3 targetVelocity = new Vector3(0, 0, Input.GetAxis("Vertical"));
	        targetVelocity = transform.TransformDirection(targetVelocity);
	        targetVelocity *= forwardSpeed;
 
            Vector3 velocity = mRigidbody.velocity;
	        Vector3 velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -forwardSpeed, forwardSpeed);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -forwardSpeed, forwardSpeed);
	        velocityChange.y = 0;
	        mRigidbody.AddForce(velocityChange, ForceMode.VelocityChange);

            if (Mathf.Abs(targetRotation) > 0.001f)
            {
                destRotation = transform.localRotation * Quaternion.AngleAxis(targetRotation, transform.localRotation * Vector3.up);
            }

            transform.localRotation = Quaternion.Lerp(transform.localRotation, destRotation, Time.fixedDeltaTime);
    
         }

         grounded = false;
     }

     void OnCollisionStay()
     {
         grounded = true;
     }
}
