using System;
using UnityEngine;
using System.Collections;
using Assets.Helpers;

public class BulletsEmiter : MonoBehaviour {

    [SerializeField]
    private Bullet prefab;
    [SerializeField]
    private ParticleSystem explosionPrefab;

    [SerializeField]
    private float power;

    public float MaxRange;


    private GameObjectPull<Bullet> bulletsPull; 
    private GameObjectPull<ParticleSystem> effectsPull; 

	// Use this for initialization
	void Start () {
        bulletsPull = new GameObjectPull<Bullet>(new GameObject("BulletsPull"), prefab, 30);
        effectsPull = new GameObjectPull<ParticleSystem>(new GameObject("EffectsPull"), explosionPrefab, 30);
	}

    public void Fire(Vector3 destination)
    {
        Vector3 startSpeed = calculateStartSpeed(transform.position,
            destination);
                   

        Bullet bullet = CreateBullet();
        bullet.transform.localPosition = Vector3.zero;
        bullet.rigidbody.velocity = startSpeed;
        //bullet.rigidbody.AddForce(transform.forward * power, ForceMode.Impulse);
    }

    private Bullet CreateBullet()
    {
        Bullet bullet = bulletsPull.GetObject();
        bullet.tag = transform.tag;
        bullet.transform.parent = transform;
        bullet.transform.localScale = prefab.transform.localScale;
        bullet.OnHit += onHit;
        bullet.gameObject.SetActive(true);
        return bullet;
    }

    private void onHit(Vector3 position, Bullet bullet)
    {
        StartCoroutine(effectCoroutine(effectsPull.GetObject(), position));
        bullet.OnHit -= onHit;
        bullet.rigidbody.velocity = Vector3.zero;
        bulletsPull.ReleaseObject(bullet);
    }

    private IEnumerator effectCoroutine(ParticleSystem effect, Vector3 position)
    {
        effect.transform.parent = transform;
        effect.transform.localScale = Vector3.one;
        effect.transform.position = position;
        effect.gameObject.SetActive(true);
        effect.Play();
        while (effect.isPlaying)
        {
            yield return null;
        }

        effectsPull.ReleaseObject(effect);
    }

    private Vector3 calculateStartSpeed(Vector3 startGlobalPostion, Vector3 targetGlobalPosition)
    {
        Vector3 lineDirection = targetGlobalPosition - startGlobalPostion;
        float length = lineDirection.magnitude;
        lineDirection.Normalize();

        var velocity = (float)(Math.Sin(Math.PI / 4) * Math.Sqrt(length * Physics.gravity.magnitude));
        return new Vector3(lineDirection.x * velocity, velocity, lineDirection.z * velocity);
    }

    private Vector3 calculateAngleSpeed(Vector3 startGlobalPostion, Vector3 targetGlobalPosition)
    {
        Vector3 lineDirection = targetGlobalPosition - startGlobalPostion;
        float length = lineDirection.magnitude;
        lineDirection.Normalize();
        double d = power / Math.Sqrt(length * Physics.gravity.magnitude);
        Debug.Log("d = " + d);
        double d1 = (d + 1) % 2 - 1;
        Debug.Log("d1 = " + d1);
        float angle = (float)(Mathf.Rad2Deg * Math.Asin(d1));
        Debug.Log("angle = " + angle);
        return Quaternion.AngleAxis(angle, Vector3.left) *
            new Vector3(lineDirection.x * power, power, lineDirection.z * power);
    }
}
