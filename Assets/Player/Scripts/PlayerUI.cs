using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.Player
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private Image _healthBarUI;
        [SerializeField] private TextMeshProUGUI _countKillUI;
        [SerializeField] private TextMeshProUGUI _countMoneyUI;

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
