﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Defs/InventoryItems", fileName = "InventoryItems")]

public class InventoryItemsDef : ScriptableObject
{
    [SerializeField] private ItemDef[] _items;

    public ItemDef Get(string id)
    {
        foreach (var itemDef in _items)
        {
            if (itemDef.Id == id)
                return itemDef;
        }

        return default;
    }

#if UNITY_EDITOR
    public ItemDef[] ItemsForEditor => _items;
#endif
}

[Serializable]

public struct ItemDef
{
    [SerializeField] private string _id;
    [SerializeField] private bool _isStackable;
    [SerializeField] private Sprite _icon;

    public string Id => _id;
    public bool IsStackable => _isStackable;

    public bool IsVoid => string.IsNullOrEmpty(_id);

    public Sprite Icon => _icon;
}
