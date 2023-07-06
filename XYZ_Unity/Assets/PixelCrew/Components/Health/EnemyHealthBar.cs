using System.Collections;
using System.Collections.Generic;
using PixelCrew.Components;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    private HealhComponent _healthComponent;
    private Image _image;
    private Transform _transformParent;
    private RectTransform _rectTransform;

    private int maxHp;
    private int currentHp;
    private float progress;



    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _healthComponent = GetComponentInParent<HealhComponent>();
        _transformParent = GetComponentInParent<Transform>();
        _image = GetComponent<Image>();
        maxHp = _healthComponent._health;
    }

    private void Update()
    {
        currentHp = _healthComponent._health;
        progress = (float)currentHp / maxHp;

        _image.fillAmount = progress;
    }
}
