using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ReverbEffectDistance : MonoBehaviour
{
    private Transform _goReverb;
    private AudioReverbZone _reverbZone;

    [SerializeField] private Transform _goListener;
    [SerializeField] private AudioSource _audioSource;

    private float _volume;

    private void Awake()
    {
        _goReverb = GetComponent<Transform>();
        _reverbZone = GetComponent<AudioReverbZone>();
        _volume = _audioSource.volume;

    }

    private void Update()
    {
        float distance = Vector3.Distance(_goReverb.position, _goListener.position);

        if (distance < _reverbZone.minDistance)
        {
            _audioSource.volume = 1 * distance;
        }
    }


}
