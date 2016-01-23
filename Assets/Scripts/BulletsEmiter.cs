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
            destination, Mathf.PI / 4);
                   

        Bullet bullet = CreateBullet();
        bullet.transform.position = transform.position;
        bullet.rigidbody.velocity = startSpeed;
        //bullet.rigidbody.AddForce(transform.forward * power, ForceMode.Impulse);
    }

    private Bullet CreateBullet()
    {
        Bullet bullet = bulletsPull.GetObject();
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

    private Vector3 calculateStartSpeed(Vector3 startGlobalPostion, Vector3 targetGlobalPosition, float radAngle)
    {
        Vector3 lineDirection = targetGlobalPosition - startGlobalPostion;
        float heightDiff = lineDirection.y;
        lineDirection.y = 0;
        
        float length = lineDirection.magnitude; 
        lineDirection.y = length * Mathf.Tan(radAngle);
        length += heightDiff / Mathf.Tan(radAngle);

        float velocity = Mathf.Sqrt(length * Physics.gravity.magnitude / Mathf.Sin(2 * radAngle));
        return velocity * lineDirection.normalized; 
    }
}
