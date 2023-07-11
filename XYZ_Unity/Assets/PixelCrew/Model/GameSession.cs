using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;
        public PlayerData Data => _data;
        private readonly CompositeDisposable _trash = new CompositeDisposable();

        public QuickInventoryModel QuickInventory { get; private set; }


        private void Awake()
        {
            LoadHud();

            if (IsSessionExit())
            {
                Destroy(gameObject);
            } 
            else
            {
                InitModels();
                DontDestroyOnLoad(this);
            }

        }

        private void LoadHud()
        {
            SceneManager.LoadScene("Hud", LoadSceneMode.Additive);
        }


        private void InitModels()
        {
            QuickInventory = new QuickInventoryModel(Data);
            _trash.Retain(QuickInventory);
        }


        private bool IsSessionExit()
        {
            var sessions = FindObjectsOfType<GameSession>();
            foreach (var gameSession in sessions)
            {
                if (gameSession != this)
                    return true;
            }

            return false;
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }




}