using UnityEngine;
using System.Collections;
using static UnityEditor.Experimental.GraphView.GraphView;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    public bool switchMusic = true;
    public bool switchSound = true;

    public enum AudioChannel { Master, Sfx, Music };

    public float masterVolumePercent { get; private set; }
    public float sfxVolumePercent { get; private set; }
    public float musicVolumePercent;

    //AudioSource sfx2DSource;
    AudioSource[] musicSources;
    int activeMusicSourceIndex;


    //Transform audioListener;
    //Transform playerT;

    private Transform spawnedContainer;

    // copies audiosource properties to temp audiosource for playing at a position
    public static AudioSource PlayClipAtPoint(AudioSource audioSource, Vector3 pos)
    {
        GameObject tempGO = new GameObject("TempAudio"); // create the temp object
        tempGO.transform.position = pos; // set its position
        AudioSource tempASource = tempGO.AddComponent<AudioSource>(); // add an audio source
        tempASource.clip = audioSource.clip;
        tempASource.outputAudioMixerGroup = audioSource.outputAudioMixerGroup;
        tempASource.mute = audioSource.mute;
        tempASource.bypassEffects = audioSource.bypassEffects;
        tempASource.bypassListenerEffects = audioSource.bypassListenerEffects;
        tempASource.bypassReverbZones = audioSource.bypassReverbZones;
        tempASource.playOnAwake = audioSource.playOnAwake;
        tempASource.loop = audioSource.loop;
        tempASource.priority = audioSource.priority;
        tempASource.volume = audioSource.volume;
        tempASource.pitch = audioSource.pitch;
        tempASource.panStereo = audioSource.panStereo;
        tempASource.spatialBlend = audioSource.spatialBlend;
        tempASource.reverbZoneMix = audioSource.reverbZoneMix;
        tempASource.dopplerLevel = audioSource.dopplerLevel;
        tempASource.rolloffMode = audioSource.rolloffMode;
        tempASource.minDistance = audioSource.minDistance;
        tempASource.spread = audioSource.spread;
        tempASource.maxDistance = audioSource.maxDistance;
        // set other aSource properties here, if desired
        tempASource.Play(); // start the sound
        MonoBehaviour.Destroy(tempGO, tempASource.clip.length); // destroy object after clip duration (this will not account for whether it is set to loop)
        return tempASource; // return the AudioSource reference
    }

    void Awake()
    {

        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {

            instance = this;
            DontDestroyOnLoad(gameObject);

            musicSources = new AudioSource[2];
            for (int i = 0; i < 2; i++)
            {
                GameObject newMusicSource = new GameObject("Music source " + (i + 1));
                musicSources[i] = newMusicSource.AddComponent<AudioSource>();
                newMusicSource.transform.parent = transform;
            }

            masterVolumePercent = PlayerPrefs.GetFloat("master volume", 0.2f);
            sfxVolumePercent = PlayerPrefs.GetFloat("sfx volume", 1);
            musicVolumePercent = PlayerPrefs.GetFloat("music volume", 1f);//0.15f);

        }


    }


    public void SetVolume(float volumePercent, AudioChannel channel)
    {
        switch (channel)
        {
            case AudioChannel.Master:
                masterVolumePercent = volumePercent;
                break;
            case AudioChannel.Sfx:
                sfxVolumePercent = volumePercent;
                break;
            case AudioChannel.Music:
                musicVolumePercent = volumePercent;
                break;
        }


        PlayerPrefs.SetFloat("master vol", masterVolumePercent);
        PlayerPrefs.SetFloat("sfx vol", sfxVolumePercent);
        PlayerPrefs.SetFloat("music vol", musicVolumePercent);
        PlayerPrefs.Save();

    }

    public void PlayMusic(AudioClip clip)
    { //Not tested
        musicSources[activeMusicSourceIndex].clip = clip;
        musicSources[activeMusicSourceIndex].volume = musicVolumePercent * masterVolumePercent;
        //musicSources [activeMusicSourceIndex].Play ();
    }

    public void PlayMusicCrossfade(AudioClip clip, float fadeDuration = 1)
    {
        activeMusicSourceIndex = 1 - activeMusicSourceIndex;
        musicSources[activeMusicSourceIndex].clip = clip;
        musicSources[activeMusicSourceIndex].Play();

        StartCoroutine(AnimateMusicCrossfade(fadeDuration));
    }

    public void FadeOutMusic(float duration)
    {
        StartCoroutine(AnimateFadeOutMusic(duration));
    }
    IEnumerator AnimateFadeOutMusic(float duration)
    {
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / duration;
            musicSources[activeMusicSourceIndex].volume = Mathf.Lerp(musicVolumePercent * masterVolumePercent, 0, percent);
            yield return null;
        }
    }

    public void StopMusic()
    {
        musicSources[activeMusicSourceIndex].volume = 0;
    }

    public void PlaySound(AudioClip clip, Vector3 pos)
    {
        if (clip != null)
        {
            PlayClipAtPos(clip, new Vector3(pos.x, Camera.main.transform.position.y, pos.z - 3f), sfxVolumePercent * masterVolumePercent);
        }

    }

    public void PlayFlatSound(AudioClip clip)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, sfxVolumePercent * masterVolumePercent);
        }
    }

    IEnumerator AnimateMusicCrossfade(float duration)
    {
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / duration;
            musicSources[activeMusicSourceIndex].volume = Mathf.Lerp(0, musicVolumePercent * masterVolumePercent, percent);
            musicSources[1 - activeMusicSourceIndex].volume = Mathf.Lerp(musicVolumePercent * masterVolumePercent, 0, percent);
            yield return null;
        }
    }

    public void changeMusicVolume(float volume)
    {
        musicSources[activeMusicSourceIndex].volume *= volume;
    }

    private AudioSource PlayClipAtPos(AudioClip clip, Vector3 pos, float volume)
    {
        if (spawnedContainer == null)
            spawnedContainer = GameObject.Find("SpawnedContainer").transform;
        GameObject q = new GameObject();
        q.name = "_";
        q.transform.SetParent(spawnedContainer);
        GameObject tempGO = Instantiate(q);
        tempGO.transform.SetParent(spawnedContainer);
        tempGO.name = "__";
        tempGO.transform.position = pos;
        AudioSource aSource = tempGO.AddComponent<AudioSource>();
        aSource.clip = clip;
        aSource.volume = volume;
        aSource.Play();
        Destroy(tempGO, clip.length);
        Destroy(q, 0.5f);
        return aSource;
    }

}
