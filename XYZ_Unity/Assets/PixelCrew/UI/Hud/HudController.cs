using System.Collections;
using System.Collections.Generic;
using PixelCrew.Model;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    [SerializeField] private ProgressBarWidget _healthBar;

    private GameSession _session;

    private void Start()
    {
        _session = FindObjectOfType<GameSession>();
        _session.Data.Hp.OnChanged += OnHealthChanged;

        OnHealthChanged(_session.Data.Hp.Value, 0);
    }

    private void OnHealthChanged(int newValue, int oldValue)
    {
        var maxHealth = _session.Data.MaxHp;
        var value = (float) newValue / maxHealth;
        _healthBar.SetProgress(value);
    }

    private void OnDestroy()
    {
        _session.Data.Hp.OnChanged -= OnHealthChanged;
    }

}
