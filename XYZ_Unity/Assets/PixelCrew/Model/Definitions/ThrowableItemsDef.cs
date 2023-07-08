using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Defs/ThrowableItems", fileName = "ThrowableItems")]
public class ThrowableItemsDef : ScriptableObject
{
    [SerializeField] private ThrowableDef[] _items;

    public ThrowableDef Get(string id)
    {
        foreach (var ThrowableDef in _items)
        {
            if (ThrowableDef.Id == id)
                return ThrowableDef;
        }

        return default;
    }


    [Serializable]
    public struct ThrowableDef
    {
        [InventoryId][SerializeField] private string _id;
        [SerializeField] private GameObject _projectile;

        public string Id => _id;
        public GameObject Projectile => _projectile;

    }

}
