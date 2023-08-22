using System.Collections;
using System.Collections.Generic;
using PixelCrew.Components;
using PixelCrew.Model;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(SpawnComponent))]

public class CheckpointComponent : MonoBehaviour
{
    [SerializeField] private string _id;
    [SerializeField] private UnityEvent _setChecked;
    [SerializeField] private UnityEvent _setUnchecked;

    public string Id => _id;

    private SpawnComponent _heroSpawner;

    private GameSession _session;

    private void Start()
    {
        _heroSpawner = GetComponent<SpawnComponent>();
        _session = FindObjectOfType<GameSession>();
        if (_session.IsChecked(_id))
            _setChecked?.Invoke();
        else
            _setUnchecked?.Invoke();
    }

    public void Check()
    {
        _session.SetChecked(_id);
    }

    public void SpawnHero()
    {
        _heroSpawner.Spawn();
    }
}
