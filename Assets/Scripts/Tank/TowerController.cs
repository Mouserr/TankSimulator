using UnityEngine;

public class TowerController : MonoBehaviour {
    [SerializeField]
    private float maxHorizontalAngle;
    [SerializeField]
    private float maxVerticalAngle;

    private Quaternion originalRotation;

    private void Awake()
    {
        originalRotation = transform.localRotation;
    }

    public float LookAt(Vector3 point)
    {
        Vector3 dirToTarget = (point - transform.position);
        Vector3 originalForward = transform.parent.rotation * originalRotation * Vector3.forward;

        Vector3 dirXZ = Vector3.ProjectOnPlane(dirToTarget, Vector3.up);
        Vector3 forwardXZ = Vector3.ProjectOnPlane(originalForward, Vector3.up);
        float yAngle = Vector3.Angle(dirXZ, forwardXZ) * Mathf.Sign(Vector3.Dot(Vector3.up, Vector3.Cross(forwardXZ, dirXZ)));
        float yClamped = Mathf.Clamp(yAngle, -maxHorizontalAngle, maxHorizontalAngle);
        Quaternion yRotation = Quaternion.AngleAxis(yClamped, Vector3.up);

        //Debug.Log(string.Format("Desired Y rotation: {0}, clamped Y rotation: {1}", yAngle, yClamped), this);

       /* originalForward = yRotation * original * Vector3.forward;
        Vector3 xAxis = yRotation * original * Vector3.right; // our local x axis
        dirYZ = Vector3.ProjectOnPlane(dirToTarget, xAxis);
        forwardYZ = Vector3.ProjectOnPlane(originalForward, xAxis);
        float xAngle = Vector3.Angle(dirYZ, forwardYZ) * Mathf.Sign(Vector3.Dot(xAxis, Vector3.Cross(forwardYZ, dirYZ)));
        float xClamped = Mathf.Clamp(xAngle, 0, -maxVerticalAngle);
        Quaternion xRotation = Quaternion.AngleAxis(xClamped, Vector3.right);

        Debug.Log(string.Format("Desired X rotation: {0}, clamped X rotation: {1}", xAngle, xClamped), this);
*/
        Quaternion newRotation = yRotation /** xRotation*/ * transform.parent.rotation * originalRotation;
        transform.rotation = newRotation;
        return yAngle - yClamped;
    }
}
