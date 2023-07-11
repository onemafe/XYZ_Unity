﻿using System.Collections;
using System.Collections.Generic;
using PixelCrew.Model;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemWidget : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private GameObject _selection;
    [SerializeField] private Text _value;

    private readonly CompositeDisposable _trash = new CompositeDisposable();

    private int _index;

    private void Start()
    {
        var session = FindObjectOfType<GameSession>();
        var index = session.QuickInventory.SelectedItem;
        _trash.Retain(session.QuickInventory.SelectedIndex.SubscribeAndInvoke(OnIndexChanged));
    }

    private void OnIndexChanged(int newValue, int _)
    {
        _selection.SetActive(_index == newValue);
    }

    public void SetData(InventoryItemData item, int index)
    {
        _index = index;
        var def = DefsFacade.I.Items.Get(item.Id);
        _icon.sprite = def.Icon;
        _value.text = def.HasTag(ItemTag.Stackable) ? item.Value.ToString() : string.Empty;
    }

    private void OnDestroy()
    {
        _trash.Dispose();
    }
}
