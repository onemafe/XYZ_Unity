using System.Collections;
using System.Collections.Generic;
using PixelCrew;
using PixelCrew.Components;
using PixelCrew.Utils;
using UnityEngine;

public class ShootingTrapAI : MonoBehaviour
{
    [SerializeField] LayerCheck _vision;
    [SerializeField] SpriteAnimationGroup _animator;
    [SerializeField] Cooldown _cooldown;


    private void Update()
    {
        if (_vision.isTouchingLayer && _cooldown.IsReady)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        _cooldown.Reset();
        _animator.SetClip("start-attack");
    }


}
