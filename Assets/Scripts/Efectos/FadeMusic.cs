using UnityEngine;
using System.Collections;

public class FadeMusic : MonoBehaviour
{
    public AudioSource mainAudioSource;
    public float fadeTime = 0.5f;
    Coroutine fadeAudioCoroutine;
    public void CambiarCancion(AudioClip cancionSiguiente)
    {
        if(fadeAudioCoroutine != null) StopCoroutine(fadeAudioCoroutine);
        fadeAudioCoroutine = StartCoroutine(FadeIn(cancionSiguiente, fadeTime));
    }

    private IEnumerator FadeIn(AudioClip audioClip, float FadeTime)
    {
        //Fade out
        float startVolume = mainAudioSource.volume;

        while (mainAudioSource.volume > 0)
        {
            mainAudioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        mainAudioSource.Stop();

        //Fade in
        mainAudioSource.clip = audioClip;
        mainAudioSource.Play();

        while (mainAudioSource.volume < startVolume)
        {
            mainAudioSource.volume += startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }
    }

}
