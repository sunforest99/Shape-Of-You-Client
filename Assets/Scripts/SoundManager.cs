using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    AudioClip[] audioClip;

    [SerializeField]
    AudioSource _audio;

    void Start()
    {
        loginBGM();
    }

    public void loginBGM()
    {
        _audio.clip = audioClip[0];
        _audio.volume = 0.4f;
        _audio.Play();  // 로고 배경 음악
    }

    public void readyBGM()
    {
        StartCoroutine(changeTo(audioClip[1], 1));
    }

    public void gameBGM()
    {
        StartCoroutine(changeTo(audioClip[2], 0.4f));
    }

    IEnumerator changeTo(AudioClip clip, float maxVol)
    {
        while (_audio.volume > 0)
        {
            _audio.volume -= 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
        _audio.Stop();
        _audio.clip = clip;
        _audio.Play();
        while (_audio.volume < maxVol)
        {
            _audio.volume += 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
