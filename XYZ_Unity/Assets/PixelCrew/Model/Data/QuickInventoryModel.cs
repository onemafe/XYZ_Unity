﻿using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrew.Model;
using UnityEngine;

public class QuickInventoryModel
{
    private readonly PlayerData _data;

    public InventoryItemData[] Inventory { get; private set; }

    public readonly IntProperty SelectedIndex = new IntProperty();

    public Action OnChanged;

    public InventoryItemData SelectedItem
    {
        get
        {
            if(Inventory.Length > 0 && Inventory.Length > SelectedIndex.Value)
            {
                return Inventory[SelectedIndex.Value];
            }

            return null;
        }
    }

    public ItemDef SelectedDef => DefsFacade.I.Items.Get(SelectedItem?.Id);


    public QuickInventoryModel(PlayerData data)
    {
        _data = data;

        Inventory = _data.Inventory.GetAll(ItemTag.Usable);
        _data.Inventory.OnChanged += OnChangedInventory;
    }

    public IDisposable Subscribe(Action call)
    {
        OnChanged += call;
        return new ActionDisposable(() => OnChanged -= call);
    }

    private void OnChangedInventory(string id, int value)
    {
        var indexFound = Array.FindIndex(Inventory, x => x.Id == id);
        if(indexFound != 1)
        {
            Inventory = _data.Inventory.GetAll(ItemTag.Usable);
            SelectedIndex.Value = Mathf.Clamp(SelectedIndex.Value, 0, Inventory.Length - 1);
            OnChanged?.Invoke();
        }
    }

    public void SetNextItem()
    {
        SelectedIndex.Value = (int)Mathf.Repeat(SelectedIndex.Value + 1, Inventory.Length);
    }
}
