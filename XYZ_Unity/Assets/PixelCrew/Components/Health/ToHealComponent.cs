using System.Collections;
using System.Collections.Generic;
using PixelCrew.Creatures;
using PixelCrew.Model;
using UnityEngine;
using UnityEngine.Events;
using static InventoryData;

namespace PixelCrew.Components
{

    public class ToHealComponent : MonoBehaviour
    {
        [SerializeField] private int _healthPoints;
        [SerializeField] private Hero _hero;
        private int BottleHealthCount => _session.Data.Inventory.Count("BottleHealth");
        private GameSession _session;



        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
        }



        
    }
}