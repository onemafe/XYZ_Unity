using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PixelCrew.Utils;
using UnityEngine;

public class TotemTower : MonoBehaviour
{
    [SerializeField] List<ShootingTrapAI> _traps;
    [SerializeField] private Cooldown _cooldown;

    private int _currentTrap;



    
    void Start()
    {
        foreach (var shootingTrapAI in _traps)
        {
            shootingTrapAI.enabled = false;
        }
    }

    
    void Update()
    {
        //пакет linq
        var hasAnyTarget = _traps.Any(x => x._vision.isTouchingLayer);
        if (hasAnyTarget)
        {
            if (_cooldown.IsReady)
            {
                _traps[_currentTrap].Shoot();
                _cooldown.Reset();
                _currentTrap = (int) Mathf.Repeat(_currentTrap + 1, _traps.Count);
            }
        }
    }
}
