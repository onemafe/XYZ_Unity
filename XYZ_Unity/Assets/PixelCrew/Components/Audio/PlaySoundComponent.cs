using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundComponent : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioData[] _sounds;

    public void Play(string id)
    {
        foreach (var audioData in _sounds)
        {
            if (audioData.Id != id) continue;

            if (_source == null)
                _source = GameObject.FindWithTag("SfxAudioSource").GetComponent<AudioSource>();

            _source.PlayOneShot(audioData.Clip);
            break;
        }
    }

    //foreach берём каждый элемент в _sounds и он является переменной audioData


    [Serializable]
    public class AudioData
    {
        [SerializeField] private string _id;
        [SerializeField] private AudioClip _clip;

        public string Id => _id;
        public AudioClip Clip => _clip;
    }
}
