using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PixelCrew.Model
{
    [Serializable]
    public class PlayerData
    {
        [SerializeField] private InventoryData _inventory;

        public PerksData Perks = new PerksData();
        public InventoryData Inventory => _inventory;

        public int MaxHp;
        public IntProperty Hp = new IntProperty();

        public PlayerData Clone()
        {
            var json = JsonUtility.ToJson(this);
            return JsonUtility.FromJson<PlayerData>(json);
        }
    }


}
