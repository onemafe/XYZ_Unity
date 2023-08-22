﻿using System.Collections;
using System.Collections.Generic;
using PixelCrew.Components;
using PixelCrew.Model;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(SpawnComponent))]

public class CheckpointComponent : MonoBehaviour
{
    [SerializeField] private string _id;
    [SerializeField] private SpawnComponent _heroSpawner;
    [SerializeField] private UnityEvent _setChecked;
    [SerializeField] private UnityEvent _setUnchecked;

    public string Id => _id;



    private GameSession _session;


    private void Start()
    {
        _session = FindObjectOfType<GameSession>();
        if (_session.IsChecked(_id))
            _setChecked?.Invoke();
        else
            _setUnchecked?.Invoke();
    }

    public void Check()
    {
        _session.SetChecked(_id);
        _setChecked?.Invoke();
    }

    public void SpawnHero()
    {
        _heroSpawner.Spawn();
    }
}
