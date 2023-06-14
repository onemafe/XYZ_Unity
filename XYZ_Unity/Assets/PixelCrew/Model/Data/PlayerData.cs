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


        public InventoryData Inventory => _inventory;
        public int Coins;
        public int Hp;
        public int MaxHp;
        public bool IsArmed;
        public int Knives;
    }
}
