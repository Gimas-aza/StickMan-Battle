using System;
using UnityEngine;
using UnityEngine.UI;
using Assets.UI;

namespace Assets.Player
{
    public class PlayerData : MonoBehaviour
    {
        [SerializeField] private float _health;
        [SerializeField] private Vector3 _spawn;
        [SerializeField] private float _rotationY;
        [SerializeField] private int _countKill;
        [SerializeField] private int _countMoney;

        public float Health => _health;
        public Vector3 Spawn => _spawn;
        public float RotationY => _rotationY;
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