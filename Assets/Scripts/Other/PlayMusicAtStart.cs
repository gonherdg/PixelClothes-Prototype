using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusicAtStart : MonoBehaviour
{
    void Start()
    {
        AudioManager.instance.SetVolume(1.0f, AudioManager.AudioChannel.Music);
        AudioManager.instance.PlayMusicCrossfade(AudioManager.instance.audioClips[2], 0.1f);
    }
}
