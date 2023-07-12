using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager
{
    public readonly static LocalizationManager I;

    private StringPersistentProperty _localeKey = new StringPersistentProperty("en", "localization/current");
    private Dictionary<string, string> _localization;

    public event Action OnLocaleChanged;

    static LocalizationManager()
    {
        I = new LocalizationManager();
    }

    public LocalizationManager()
    {
        LoadLocale(_localeKey.Value);
    }

    public void LoadLocale(string localeToLoad)
    {
        var def = Resources.Load<LocaleDef>($"Locales/{localeToLoad}");
        _localization = def.GetData();
        OnLocaleChanged?.Invoke();
    }

    public string Localize(string key)
    {
        return _localization.TryGetValue(key, out var value) ? value : $"%%%{key}%%%";
    }
}
