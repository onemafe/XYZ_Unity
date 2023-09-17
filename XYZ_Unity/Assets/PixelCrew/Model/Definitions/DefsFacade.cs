using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Defs/DefsFacade", fileName = "DefsFacade")]

public class DefsFacade : ScriptableObject
{

    [SerializeField] private ItemsRepository _items;
    [SerializeField] private ThrowableRepository _throwableItems;
    [SerializeField] private PlayerDef _player;
    [SerializeField] private PotionRepository _potions;
    [SerializeField] private PerkRepository _perks;




    public ItemsRepository Items => _items;
    public ThrowableRepository ThrowableItems => _throwableItems;
    public PlayerDef Player => _player;
    public PotionRepository Potions => _potions;
    public PerkRepository Perks => _perks;



    private static DefsFacade _instance;
    public static DefsFacade I => _instance == null ? LoadDefs() : _instance;

    private static DefsFacade LoadDefs()
    {
        return _instance = Resources.Load<DefsFacade>("DefsFacade");

    }

    public TDefType[] All => new List<TDefType>(_collection).ToArray();


}
