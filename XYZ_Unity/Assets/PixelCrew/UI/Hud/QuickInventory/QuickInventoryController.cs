using System.Collections;
using System.Collections.Generic;
using PixelCrew.Model;
using UnityEngine;

public class QuickInventoryController : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private GameObject _prefab;

    private readonly CompositeDisposable _trash = new CompositeDisposable();

    private GameSession _session;
    private InventoryItemData[] _inventory;

    private void Start()
    {
        _session = FindObjectOfType<GameSession>();
        _inventory = _session.Data.Inventory.GetAll();

        Rebuild();
    }

    private void Rebuild()
    {

    }
}
