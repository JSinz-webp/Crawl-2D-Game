using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioGuy : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip patter01;
    public AudioClip patter02;
    public AudioClip patter03;
    public AudioClip patter04;
    public AudioClip patter05;
    public AudioClip patter06;

    [Range(0.05f, 0.5f)]
    public float pitchVariance = 0.15f; // Amount up or down from 1.0f

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayPitterPatter()
    {
        float randomPitch = Random.Range(1f - pitchVariance, 1f + pitchVariance);
        audioSource.pitch = randomPitch;
        int rand = Random.Range(0, 5);
        if (rand % 0 == 0)
        {
            audioSource.PlayOneShot(patter01);
        }
        else if (rand % 1 == 0)
        {
            audioSource.PlayOneShot(patter02);
        }
        else if (rand % 2 == 0)
        {
            audioSource.PlayOneShot(patter03);
        }
        else if (rand % 3 == 0)
        {
            audioSource.PlayOneShot(patter04);
        }
        else if (rand % 4 == 0)
        {
            audioSource.PlayOneShot(patter05);
        }
        else if (rand % 5 == 0)
        {
            audioSource.PlayOneShot(patter06);
        }
    }

}
