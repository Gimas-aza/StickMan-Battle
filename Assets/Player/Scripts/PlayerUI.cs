using UnityEngine;
using UnityEngine.UI;
using Assets.UI;
using TMPro;
using Zenject;

namespace Assets.Player
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private Image _healthBarUI;
        [SerializeField] private TextMeshProUGUI _countKillUI;
        [SerializeField] private TextMeshProUGUI _countMoneyUI;

        private GameEventsServise _gameEvents;

        [Inject]
        private void Construct(GameEventsServise gameEvents)
        {
            _gameEvents = gameEvents;

            // _gameEvents.UpdateHealthBar.AddListener(UpdateHealthBar);
        }

        public void UpdateHealthBar(float currentHealth, float maxHealth)
        {
            _healthBarUI.fillAmount = currentHealth / maxHealth;
        }

        public void RenderStatisticMenu(int countKill, int countMoney)
        {
            _countKillUI.text = countKill.ToString();
            _countMoneyUI.text = countMoney.ToString();
        }
    }
}
