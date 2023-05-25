using System;
using UnityEngine;
using UnityEngine.UI;
using Assets.UI;

namespace Assets.Player
{
    public class PlayerData : MonoBehaviour
    {
        [SerializeField] private float _health;
        [SerializeField] private float _spawn;
        [SerializeField] private int _countKill;
        [SerializeField] private int _countMoney;

        public float Health => _health;
        public float Spawn => _spawn;
        public int CountKill => _countKill;
        public int CountMoney => _countMoney;

        public void IncreaseMaxHealth(int increase)
        {
            if (increase > 0)
                _health += increase;
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