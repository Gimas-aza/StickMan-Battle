using System;
using Assets.Player;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets
{    
    public class GameOver : MonoBehaviour
    {        
        private PlayerData _pastPlayerSurvivor;
        private int _moneyMultiplier = 1;
        private GlobalEventsSystem _gameEvents;

        [Inject]
        private void Construct(GlobalEventsSystem gameEvents)
        {
            _gameEvents = gameEvents;

            _gameEvents.onGameOverMenu.AddListener(ShowMenu);
        }

        public int GetMoneyMultiplier(PlayerData playerData)
        {
            if (playerData != _pastPlayerSurvivor && _moneyMultiplier > 0)
            {
                _moneyMultiplier = 0;
                _pastPlayerSurvivor = playerData;
            }

            return ++_moneyMultiplier;
        }

        private void ShowMenu()
        {
            gameObject.SetActive(true);
        }
    }
}
