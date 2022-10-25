using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

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

    private float StartVolumeBGM = 1.0f;
    private float StartVolumeSE = 1.0f;

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject); // シーンが変わっても死なない
    }

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
        if (Input.GetKeyDown(KeyCode.A))
        {
            PauseSoundAll();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            PauseRestartSoundAll();
        }
    }

    // サウンドの再生開始
    public void PlaySound(string AudioName, bool isloop)
    {
        PlaySound(AudioName, isloop, 0.0f, 0.0f, 1.0f);
    }
    public void PlaySound(string AudioName, bool isloop, float StartTime)
    {
        PlaySound(AudioName, isloop, StartTime, 0.0f, 1.0f);
    }

    // BGM遅延用
    public void PlaySoundBGMDelay(string AudioName, float DelayTime)
    {
        PlaySound(AudioName, true, 0.0f, DelayTime, 1.0f);
    }
    public void PlaySoundBGMDelay(string AudioName, float DelayTime, float Volume)
    {
        PlaySound(AudioName, true, 0.0f, DelayTime, Volume);
    }

    // 本体
    public void PlaySound(string AudioName, bool isloop, float StartTime, float DelayTime, float Volume)
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
            audioSource[(int)Enum_AudioType.BGM].time = StartTime;
            audioSource[(int)Enum_AudioType.BGM].volume = Volume;

            if (DelayTime <= 0.0f)
                audioSource[(int)Enum_AudioType.BGM].Play();
            else
                audioSource[(int)Enum_AudioType.BGM].PlayDelayed(DelayTime);

            StartVolumeBGM = Volume;
        }  
        else
        {
            audioSource[(int)Enum_AudioType.SE].loop = false;
            audioSource[(int)Enum_AudioType.SE].clip = Audio.audioClip;
            audioSource[(int)Enum_AudioType.SE].time = StartTime;
            audioSource[(int)Enum_AudioType.SE].volume = Volume;
            audioSource[(int)Enum_AudioType.SE].PlayOneShot(Audio.audioClip);

            StartVolumeSE = Volume;
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

    // すべてのサウンドの再生停止
    public void StopSoundAll()
    {
        foreach (var obj in audioSource)
        {
            obj.Stop();
        }
    }

    // すべてのサウンドの再生一時停止
    public void PauseSoundAll()
    {
        foreach (var obj in audioSource)
        {
            obj.Pause();
        }
    }

    // BGMの再生一時停止
    public void PauseSoundBGM()
    {
        audioSource[(int)Enum_AudioType.BGM].Pause();
    }

    // SEの再生一時停止
    public void PauseSoundSE()
    {
        audioSource[(int)Enum_AudioType.SE].Pause();
    }

    // すべてのサウンドの再生一時停止解除
    public void PauseRestartSoundAll()
    {
        foreach (var obj in audioSource)
        {
            obj.UnPause();
        }
    }


    // 音量調節用
    public void SetVolumeBGM(float Volume)
    {
        audioSource[(int)Enum_AudioType.BGM].volume = StartVolumeBGM * Volume;
    }
    public void SetVolumeSE(float Volume)
    {
        audioSource[(int)Enum_AudioType.SE].volume = StartVolumeSE * Volume;
    }

}
