using TMPro;
using UnityEngine;

namespace Assets.UI
{
    public class StatisticGameMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _countKillUIPlayer;
        [SerializeField] private TextMeshProUGUI _countMoneyUIPlayer;

        public void Render(int countKill, int countMoney)
        {
            _countKillUIPlayer.text = countKill.ToString();
            _countMoneyUIPlayer.text = countMoney.ToString();
        }
    }   
}
