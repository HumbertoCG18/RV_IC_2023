using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXController : MonoBehaviour
{
    [SerializeField] private AudioClip _sfxAudioClip;
    [SerializeField] private float _volume = 1f;

    public void PlaySFX()
    {
        AudioManager.Instance.PlaySFX(_sfxAudioClip, _volume);
    }
}
