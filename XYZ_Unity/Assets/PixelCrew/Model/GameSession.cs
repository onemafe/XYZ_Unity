using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

namespace PixelCrew.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;
        public PlayerData Data => _data;
        private readonly CompositeDisposable _trash = new CompositeDisposable();

        public QuickInventoryModel QuickInventory { get; private set; }

        private readonly List<string> _checkpoints = new List<string>();
        [SerializeField] private string _defaultCheckpoint;

        public List<string> showCheckpoints;

        private PlayerData _save;

        // Для отображения чекпоинтов в инспекторе
        private void Update()
        {
            showCheckpoints = _checkpoints;
        }

        private void Awake()
        {
            
            var existSession = GetExistSession();
            if (existSession != null)
            {
                existSession.StartSession(_defaultCheckpoint);
                Destroy(gameObject);
            }

            else
            {
                Save();
                InitModels();
                DontDestroyOnLoad(this);
                StartSession(_defaultCheckpoint);
            }

        }

        public void Save()
        {
            _save = _data.Clone();
        }

        public void LoadLastSave()
        {
            _data = _save.Clone();
            _trash.Dispose();
            InitModels();
        }

        private void StartSession(string defaultChecpoint)
        {
            SetChecked(defaultChecpoint);

            LoadHud();
            SpawnHero();
        }

        private void SpawnHero()
        {
            var checkpoints = FindObjectsOfType<CheckpointComponent>();
            var lastCheckpoint = _checkpoints.Last();
            foreach (var checkpoint in checkpoints)
            {
                if (checkpoint.Id == lastCheckpoint)
                {
                    checkpoint.SpawnHero();
                    break;
                }
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


        private GameSession GetExistSession()
        {
            var sessions = FindObjectsOfType<GameSession>();
            foreach (var gameSession in sessions)
            {
                if (gameSession != this)
                    return gameSession;
            }

            return null;
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }

        public bool IsChecked(string id)
        {
            return _checkpoints.Contains(id);
        }

        public void SetChecked(string id)
        {
            if (!_checkpoints.Contains(id))
            {
                //Save();
                _checkpoints.Add(id);
            }

        }



    }




}