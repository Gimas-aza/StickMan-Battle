using System;
using UnityEngine;
using UnityEngine.UI;
using Assets.UI;

namespace Assets.Player
{
    public class PlayerData : MonoBehaviour
    {
        [SerializeField] private int _countKill;
        [SerializeField] private int _countMoney;

        public float Health { get; private set; }
        public float Spawn { get; private set; }
        public int CountKill => _countKill;
        public int CountMoney => _countMoney;

        // TODO Перенести в отдельный класс
        public StatisticGameMenu StatisticGameMenu;
        public PlayerData OpponentPlayer;
        public Image HealthBarUI;

        public void IncreaseMaxHealth(int increase)
        {
            if (increase > 0)
                Health += increase;
        }

        public void WithdrawMoney(int price)
        {
            _countMoney -= price;
        }

        public void UpdateStatistic(int money)
        {
            _countKill++;
            _countMoney += money;
        }
    }
}