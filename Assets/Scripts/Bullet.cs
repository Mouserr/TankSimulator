using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private Transform CenterOfMass;
    public delegate void HitDelegate(Vector3 point, Bullet bullet);

    public HitDelegate OnHit = (p, b) => {};

    private void Awake()
    {
        rigidbody.centerOfMass = CenterOfMass.localPosition;
    }

    void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Vector3 pos = contact.point;
        OnHit(pos, this);
    }
}
