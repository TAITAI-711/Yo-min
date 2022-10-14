using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    // �T�E���h�̎��
    public enum Enum_AudioType
    {
        BGM = 0,
        SE
    }

    // �T�E���h�̃N���b�v���\����
    [System.Serializable]
    public struct AudioInfo
    {
        public string audioName;
        public AudioClip audioClip;
        //public Enum_AudioType audioType;
    }

    // �T�E���h�̃N���b�v���ϐ�
    [SerializeField] private AudioInfo[] Audios;

    // �I�[�f�B�I�\�[�X
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

    // �T�E���h�̍Đ��J�n
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

    // BGM�̍Đ���~
    public void StopSoundBGM()
    {
        audioSource[(int)Enum_AudioType.BGM].Stop();
    }

    // SE�̍Đ���~
    public void StopSoundSE()
    {
        audioSource[(int)Enum_AudioType.SE].Stop();
    }

    // ���ׂĂ̍Đ���~
    public void StopSoundAll()
    {
        foreach (var obj in audioSource)
        {
            obj.Stop();
        }
    }
}
