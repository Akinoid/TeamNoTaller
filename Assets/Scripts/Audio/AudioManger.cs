using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private Dictionary<string, AudioSource> activeLoopSources = new Dictionary<string, AudioSource>();

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        public bool loop;
        public AudioMixerGroup outputGroup;
    }

    public List<Sound> sounds;
    public AudioSource audioSourcePrefab;

    private Dictionary<string, Sound> soundDict = new Dictionary<string, Sound>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        foreach (var s in sounds)
        {
            soundDict[s.name] = s;
        }
    }

    public void Play(string soundName)
    {
        if (!soundDict.ContainsKey(soundName))
        {
            Debug.LogWarning("AudioManager: sound not found ? " + soundName);
            return;
        }

        Sound s = soundDict[soundName];


        if (s.loop && activeLoopSources.ContainsKey(soundName))
            return;

        AudioSource source = Instantiate(audioSourcePrefab, transform);
        source.clip = s.clip;
        source.volume = s.volume;
        source.loop = s.loop;
        source.outputAudioMixerGroup = s.outputGroup;
        source.Play();

        if (s.loop)
            activeLoopSources[soundName] = source;
        else
            Destroy(source.gameObject, s.clip.length);
    }

    public void Stop(string name)
    {
        if (activeLoopSources.TryGetValue(name, out var source))
        {
            source.Stop();
            Destroy(source.gameObject);
            activeLoopSources.Remove(name);
        }
    }
    public void StopAllSounds()
    {
        foreach (var src in activeLoopSources.Values)
            if (src != null) Destroy(src.gameObject);

        activeLoopSources.Clear();
    }
}
