using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class BulletsEmiter : NetworkBehaviour {

    [SerializeField]
    private Bullet prefab;
    [SerializeField]
    private EffectWithSound shootEffect;
    [SerializeField]
    private EffectWithSound explosionEffect;
    [SerializeField]
    [Range(0, 60)]
    private float shootAngle = 45;

    public float MaxRange;
    public float MinRange;


    private GameObjectPull<Bullet> bulletsPull;
    private GameObjectPull<EffectWithSound> shootsPull;
    private GameObjectPull<EffectWithSound> explosionsPull; 

	void Start () {
        bulletsPull = new GameObjectPull<Bullet>(new GameObject("BulletsPull"), prefab, 30);
        shootsPull = new GameObjectPull<EffectWithSound>(new GameObject("ShootPull"), shootEffect, 30);
        explosionsPull = new GameObjectPull<EffectWithSound>(new GameObject("ExplosionsPull"), explosionEffect, 30);
	}

    [Command]
    public void CmdFire(Vector3 destination)
    {
        Vector3 startSpeed = calculateStartSpeed(transform.position,
            destination, Mathf.Deg2Rad * shootAngle);
        if (startSpeed == Vector3.zero)
            return;
        Bullet bullet = CreateBullet();
        StartCoroutine(effectCoroutine(shootsPull, transform.position, true));
        bullet.Rigidbody.velocity = startSpeed;

        NetworkServer.Spawn(bullet.gameObject);
    }

    
    private Bullet CreateBullet()
    {
        Bullet bullet = bulletsPull.GetObject();
        bullet.transform.localScale = prefab.transform.localScale;
        bullet.transform.rotation = transform.rotation;
        bullet.transform.position = transform.position;
        bullet.OnHit += onHit;
        bullet.gameObject.SetActive(true);
        
        return bullet;
    }

    private void onHit(Vector3 position, Bullet bullet)
    {
        bullet.OnHit -= onHit;
        StartCoroutine(effectCoroutine(explosionsPull, bullet.transform.position, false));
        bullet.Rigidbody.velocity = Vector3.zero;
        bulletsPull.ReleaseObject(bullet);
    }

    private IEnumerator effectCoroutine(GameObjectPull<EffectWithSound> effectsPull, Vector3 position, bool changeRotation)
    {
        EffectWithSound effect = effectsPull.GetObject();
        effect.transform.localScale = Vector3.one;
        effect.transform.position = position;
        if (changeRotation)
        {
            effect.transform.rotation = transform.rotation;
        }
        effect.gameObject.SetActive(true);
        effect.Play();
        yield return new WaitForSeconds(effect.Duration);

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
        if (length <= 0) return Vector3.zero;

        float velocity = Mathf.Sqrt(length * Physics.gravity.magnitude / Mathf.Sin(2 * radAngle));
        return velocity * lineDirection.normalized; 
    }
}
