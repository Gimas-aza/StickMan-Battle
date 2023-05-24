using Assets.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.UI
{
    public class UIControl : MonoBehaviour
    {
        private GameEventsServise _gameEvents;

        [Inject]
        private void Construct(GameEventsServise gameEvents)
        {
            _gameEvents = gameEvents;

            _gameEvents.UpdateHealthBar.AddListener(UpdateHealthBar);
        }

        private void UpdateHealthBar(PlayerData playerData, float health)
        {
            playerData.HealthBarUI.fillAmount = health / playerData.Health;
        }
    }
}