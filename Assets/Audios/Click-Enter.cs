using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource soundEffectSource;

    public void PlaySoundEffect(AudioClip soundEffect)
    {
        soundEffectSource.PlayOneShot(soundEffect);
    }
}
