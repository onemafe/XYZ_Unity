using PixelCrew.Creatures;
using PixelCrew.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{
    public class AddScoreComponent : MonoBehaviour
    {
        public int _score;
        [SerializeField] GameObject _coin;
        [SerializeField] Hero _hero;

        private GameSession _session;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
        }

        public void AddScores()
        {
            _session.Data.Coins += _score;
            _hero.ShowScores();
        }  
    }
}


/*public void AddScores()
        {
            if (_coin.name == "SilverCoin")
            {
                _hero.scores += 1;
            }
            else if (_coin.name == "GoldCoin")
            {
                _hero.scores += 10;
            }
            _hero.ShowScores();
        }*/