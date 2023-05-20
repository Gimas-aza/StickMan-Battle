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

            _gameEvents.UpdateStatisticGame.AddListener(UpdateStatisticPlayer);
        }

        public void RestartGame() 
        {
            
        }

        public void UpdateStatisticPlayer(PlayerData playerData, int money)
        {
            int countKill = playerData.CountKill + 1;

            playerData.UpdateStatistic(money);

            playerData.StatisticGameMenu.Render(countKill, money);
        }
    }
}
