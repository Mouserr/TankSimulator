using UnityEngine;

public class EffectWithSound : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem effect;
    [SerializeField]
    private AudioSource audioSource;

    public float Duration
    {
        get { return effect.duration; }
    }

    public void Play()
    {
        effect.Play();
        audioSource.Play();
    }
}
