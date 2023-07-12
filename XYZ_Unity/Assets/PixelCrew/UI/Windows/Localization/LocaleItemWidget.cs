using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LocaleItemWidget : MonoBehaviour, IItemRenderer<LocaleInfo>
{

    [SerializeField] private Text _text;
    [SerializeField] private GameObject _selector;
    [SerializeField] private SelectLocale _onSelected;

    private LocaleInfo _data;

    private void Start()
    {
        LocalizationManager.I.OnLocaleChanged += UpdateCelection;
    }

    public void SetData(LocaleInfo localeInfo, int index)
    {
        _data = localeInfo;
        UpdateCelection();
        _text.text = localeInfo.LocaleId.ToUpper();
    }


    private void UpdateCelection()
    {
        var isSelected = LocalizationManager.I.LocaleKey == _data.LocaleId;
        _selector.SetActive(isSelected);
    }


    public void OnSelected()
    {
        _onSelected?.Invoke(_data.LocaleId);
    }

    private void OnDestroy()
    {
        LocalizationManager.I.OnLocaleChanged -= UpdateCelection;
    }
}

[Serializable]
public class SelectLocale : UnityEvent<string>
{
}


public class LocaleInfo
{
    public string LocaleId;
}
