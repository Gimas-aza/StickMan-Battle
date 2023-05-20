using Assets.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.UI
{
    public class UIControl : MonoBehaviour
    {
        [SerializeField] private GameObject _menuGameOver;

        private GameEventsServise _gameEvents;

        [Inject]
        private void Construct(GameEventsServise gameEvents)
        {
            _gameEvents = gameEvents;

            _gameEvents.GameOverMenu.AddListener(ShowGameOverMenu);
            _gameEvents.UpdateHealthBar.AddListener(UpdateHealthBar);
            _gameEvents.UpdateJerkBar.AddListener(UpdateJerkBar);
        }

        private void ShowGameOverMenu()
        {
            _menuGameOver.SetActive(true);
        }

        private void UpdateHealthBar(PlayerData playerData, float health)
        {
            playerData.HealthBarUI.fillAmount = health / playerData.Health;
        }

        private void UpdateJerkBar(PlayerData playerData, float JerkCooldown)
        {
            playerData.JerkBarUI.fillAmount += JerkCooldown;
        }
    }
}