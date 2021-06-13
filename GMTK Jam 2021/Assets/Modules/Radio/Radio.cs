using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Radio : MonoBehaviour
{
    public AudioClip transitionClip;
    private AudioSource transitionSource;

    public AudioMixerGroup mixerGroup;

    public AudioClip[] stationClips;
    private AudioSource[] stationSources;
    private int activeStationIndex;
    private int stationCount;
    private bool transitioning;

    // Start is called before the first frame update
    void Start()
    {
        transitioning = false;
        transitionSource = gameObject.AddComponent<AudioSource>();
        InitSource(transitionSource, transitionClip, playOnAwake: false, loop: false);
        transitionSource.volume = 1.0f;

        stationCount = stationClips.Length;
        stationSources = new AudioSource[stationCount];
        for (int i = 0; i < stationCount; i++)
        {
            stationSources[i] = gameObject.AddComponent<AudioSource>();
            InitSource(stationSources[i], stationClips[i], playOnAwake: true, loop: true);
        }
        activeStationIndex = Random.Range(0, stationCount);
        stationSources[activeStationIndex].volume = 1.0f;
    }

    void InitSource(AudioSource source, AudioClip clip, bool playOnAwake, bool loop)
    {
        source.clip = clip;
        source.volume = 0.0f;
        source.spatialBlend = 0.0f;
        source.playOnAwake = playOnAwake;
        source.loop = loop;
        source.outputAudioMixerGroup = mixerGroup;
        if (playOnAwake) source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.inputString != "")
        {
            if (int.TryParse(Input.inputString, out int stationIndex))
            {
                SetStation(stationIndex - 1);
            }
        }
    }

    void SetStation(int index)
    {
        if (index < 0 || index >= stationCount || transitioning) return;
        transitioning = true;
        StartCoroutine(SetStationCR(index));
    }

    IEnumerator SetStationCR(int index)
    {
        AudioSource station1 = stationSources[activeStationIndex];
        AudioSource station2 = stationSources[index];
        float t = 0.0f;
        transitionSource.Play();
        while (t < 1.0f)
        {
            station1.volume = 1 - t;
            station2.volume = t;
            t += Time.deltaTime * 4f;
            yield return new WaitForEndOfFrame();
        }
        station1.volume = 0f;
        station2.volume = 1f;
        activeStationIndex = index;
        yield return new WaitForSeconds(2f);
        transitioning = false;
    }
}
