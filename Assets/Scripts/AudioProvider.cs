using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioProvider : MonoBehaviour
{
    public List<AudioFiles> SFX_clips = new();
    public List<AudioFiles> MusicClips = new();
    public AudioClip GetAudioClipByName(string _audioName)
    {

        foreach(var clip in SFX_clips)
        {
            if(clip.audioName == _audioName)
            {
                int r = Random.Range(0,clip.audioClip.Length);
                return clip.audioClip[r];
            }
        }
        return null;
    }
    public AudioClip GetMusicByName(string _audioName)
    {

        foreach(var clip in MusicClips)
        {
            if(clip.audioName == _audioName)
            {
                int r = Random.Range(0,clip.audioClip.Length);
                return clip.audioClip[r];
            }
        }
        return null;
    }
}
[System.Serializable]
public struct AudioFiles
{
    public string audioName;
    public AudioClip[] audioClip;
}