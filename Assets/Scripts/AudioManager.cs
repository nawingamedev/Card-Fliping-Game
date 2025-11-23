using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    private AudioProvider audioProvider;
    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        audioProvider = GetComponent<AudioProvider>();
    }
    public void ChangeMusic(string clipName)
    {
        AudioClip audio = audioProvider.GetMusicByName(clipName);
        if(audio == null) {return;}
        musicSource.clip = audio;
        musicSource.Play();
    }
    public void PlayMusic()
    {
        if(!musicSource.isPlaying) musicSource.Play();
    }
    public void StopMusic()
    {
        musicSource.Stop();
    }
    public void Play2DClip(string _audioName)
    {
        AudioClip audio = audioProvider.GetAudioClipByName(_audioName);
        if(audio == null) {return;}
        sfxSource.PlayOneShot(audio);
    }
}