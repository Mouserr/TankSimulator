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


    private GameObjectPull<Bullet> bulletsPull; 
    private GameObjectPull<ParticleSystem> effectsPull; 

	// Use this for initialization
	void Start () {
        bulletsPull = new GameObjectPull<Bullet>(new GameObject("BulletsPull"), prefab, 30);
        effectsPull = new GameObjectPull<ParticleSystem>(new GameObject("EffectsPull"), explosionPrefab, 30);
	}
	
	// Update is called once per frame
	void Update () 
    {
          if (Input.GetMouseButtonUp(0))
          {
              Bullet bullet = CreateBullet();
              bullet.transform.localPosition = Vector3.zero;
              bullet.rigidbody.AddForce(transform.forward * power, ForceMode.Impulse);
          }
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
}
