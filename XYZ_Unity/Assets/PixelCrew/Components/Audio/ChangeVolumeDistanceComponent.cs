using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ChangeVolumeDistanceComponent : MonoBehaviour
{
    private Transform _goReverb;
    private AudioReverbZone _reverbZone;

    [SerializeField] private Transform _goListener;
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private float _minVolume;
    private float _maxVolume;
    private float distance;

    private void Awake()
    {
        _goReverb = GetComponent<Transform>();
        _reverbZone = GetComponent<AudioReverbZone>();
        _maxVolume = _audioSource.volume;

    }

    private void Update()
    {
        distance = Vector3.Distance(_goReverb.position, _goListener.position);

        if (distance < _reverbZone.maxDistance)
        {
            _audioSource.volume = Mathf.Max(_maxVolume * distance / _reverbZone.maxDistance, _minVolume);
        }
        
    }


}
