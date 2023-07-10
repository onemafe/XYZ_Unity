﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Defs/Potions", fileName = "Potions")]
public class PotionRepository : DefRepository<PotionDef>
{

}


[Serializable]
public struct PotionDef : IHaveId
{
    [InventoryId][SerializeField] private string _id;
    [SerializeField] private float _value;
    [SerializeField] private float _time;


    public string Id => _id;
    public float Value => _value;
    public float Time => _time;
}
