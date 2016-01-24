using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private Transform CenterOfMass;
    [SerializeField]
    private TrailRenderer Trail;
    
    public delegate void HitDelegate(Vector3 point, Bullet bullet);
    public HitDelegate OnHit = (p, b) => {};

    [HideInInspector]
    public Rigidbody Rigidbody;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Rigidbody.centerOfMass = CenterOfMass.localPosition;
    }

    public void OnEnable()
    {
        Trail.Clear();
    }

    void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Vector3 pos = contact.point;
        OnHit(pos, this);
    }
}
