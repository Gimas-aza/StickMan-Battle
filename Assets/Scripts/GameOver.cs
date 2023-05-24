using System;
using Assets.Player;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets
{    
    public class GameOver : MonoBehaviour
    {        
        private GameEventsServise _gameEvents;

        [Inject]
        private void Construct(GameEventsServise gameEvents)
        {
            _gameEvents = gameEvents;

            _gameEvents.GameOverMenu.AddListener(ShowMenu);
            _gameEvents.UpdateStatisticGame.AddListener(UpdateStatisticUI);
        }

        private void ShowMenu()
        {
            gameObject.SetActive(true);
        }

        private void RestartGame() 
        {
            // todo дописать
        }

        private void UpdateStatisticUI(PlayerData playerData, int money)
        {
            playerData.UpdateStatistic(money);

            int countKill = playerData.CountKill;
            playerData.StatisticGameMenu.Render(countKill, money);
        }
    }
}
