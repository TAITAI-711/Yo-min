using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    // サウンドの種類
    public enum Enum_AudioType
    {
        BGM = 0,
        SE
    }

    // サウンドのクリップ情報構造体
    [System.Serializable]
    public struct AudioInfo
    {
        public string audioName;
        public AudioClip audioClip;
        //public Enum_AudioType audioType;
    }

    // サウンドのクリップ情報変数
    [SerializeField] private AudioInfo[] Audios;

    // オーディオソース
    private AudioSource[] audioSource;


    void Start()
    {
        audioSource = GetComponents<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlaySound("Test", false);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlaySound("Test", true);
        }
    }

    // サウンドの再生開始
    public void PlaySound(string AudioName, bool isloop)
    {
        AudioInfo Audio;
        Audio.audioClip = null;

        foreach (var obj in Audios)
        {
            if (obj.audioName == AudioName)
            {
                Audio = obj;
                break;
            }
        }

        if (Audio.audioClip == null)
            return;

        if (isloop)
        {
            audioSource[(int)Enum_AudioType.BGM].loop = true;
            audioSource[(int)Enum_AudioType.BGM].clip = Audio.audioClip;
            audioSource[(int)Enum_AudioType.BGM].Play();
        }  
        else
        {
            audioSource[(int)Enum_AudioType.SE].loop = false;
            audioSource[(int)Enum_AudioType.SE].clip = Audio.audioClip;
            audioSource[(int)Enum_AudioType.SE].PlayOneShot(Audio.audioClip);
        }
    }

    // BGMの再生停止
    public void StopSoundBGM()
    {
        audioSource[(int)Enum_AudioType.BGM].Stop();
    }

    // SEの再生停止
    public void StopSoundSE()
    {
        audioSource[(int)Enum_AudioType.SE].Stop();
    }

    // すべての再生停止
    public void StopSoundAll()
    {
        foreach (var obj in audioSource)
        {
            obj.Stop();
        }
    }
}
