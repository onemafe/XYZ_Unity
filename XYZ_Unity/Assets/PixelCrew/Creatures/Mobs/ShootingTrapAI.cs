using System.Collections;
using System.Collections.Generic;
using PixelCrew;
using PixelCrew.Components;
using PixelCrew.Utils;
using UnityEngine;

public class ShootingTrapAI : MonoBehaviour
{
    [SerializeField] public LayerCheck _vision;
    [SerializeField] private SpriteAnimationGroup _animator;
    [SerializeField] private Cooldown _cooldown;


    private void Update()
    {
        if (_vision.isTouchingLayer && _cooldown.IsReady)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        _cooldown.Reset();
        _animator.SetClip("start-attack");
    }


}
