using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PixelCrew.Components;
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
            var hp = shootingTrapAI.GetComponent<HealhComponent>();
            hp._onDie.AddListener(() => OnTrapDead(shootingTrapAI));
        }
    }

    private void OnTrapDead(ShootingTrapAI shootingTrapAI)
    {
        var index = _traps.IndexOf(shootingTrapAI);
        _traps.Remove(shootingTrapAI);
        if (index < _currentTrap)
        {
            _currentTrap--;
        }
    }

    
    void Update()
    {

        if (_traps.Count == 0)
        {
            enabled = false;
            Destroy(gameObject, 1f);
        }


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
