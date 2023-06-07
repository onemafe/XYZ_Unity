using PixelCrew.Creatures;
using PixelCrew.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PixelCrew.Components
{
    public class ArmHeroComponent : MonoBehaviour
    {
        [SerializeField] public int knivesNumber;
        private GameSession _session;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
        }

            public void ArmHero(GameObject go)
        {
            var hero = go.GetComponent<Hero>();
            if (hero != null)
            {
                hero.ArmHero();
            }


            _session.Data.Knives += knivesNumber;
            hero._knivesNumber = _session.Data.Knives;
        }

    }
}
