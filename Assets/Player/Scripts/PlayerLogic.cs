using UnityEngine;
using Zenject;

namespace Assets.Player
{
    public class PlayerLogic : MonoBehaviour
    {
        [SerializeField] private float _currentPlayerHealth;

        private PlayerData _currentPlayerData;
        private int _countKill;
        private int _countMoney;
        private int _moneyMultiplier = 1;
        private PlayerData _lastPlayerKilled;
        private GameEventsServise _gameEvents;

        [Inject]
        private void Construct(GameEventsServise gameEvents)
        {
            _gameEvents = gameEvents;

            _gameEvents.HealthToMaximum.AddListener(SetHealthToMaximum);
        }

        private void Awake()
        {
            _currentPlayerData = GetComponent<PlayerData>();
        }

        private void Start()
        {
            InitializePlayerData();
        }

        public void InflictDamage(float damage)
        {
            if (damage >= _currentPlayerHealth)
            {
                _currentPlayerHealth = 0;

                IncreaseCashBalance(_currentPlayerData.OpponentPlayer);

                _gameEvents.DisablePlayerMovement.Invoke(true);

                WriteInStatisticKill();
                _gameEvents.GameOverMenu.Invoke();
            }
            else
                _currentPlayerHealth -= damage;

            _gameEvents.UpdateHealthBar.Invoke(_currentPlayerData, _currentPlayerHealth);
        }

        private void InitializePlayerData()
        {
            SetHealthToMaximum();

            _countKill = _currentPlayerData.OpponentPlayer.CountKill;
            _countMoney = _currentPlayerData.OpponentPlayer.CountMoney;

            _currentPlayerData.OpponentPlayer.StatisticGameMenu.Render(_countKill, _countMoney);
        }

        private void SetHealthToMaximum()
        {
            _currentPlayerHealth = _currentPlayerData.Health;
        }

        private void IncreaseCashBalance(PlayerData playerData)
        {
            SetCashMultiplier(playerData);

            _countMoney += _moneyMultiplier * 5;
        }

        private void SetCashMultiplier(PlayerData playerData)
        {
            if (playerData != _lastPlayerKilled && _moneyMultiplier > 0)
            {
                _moneyMultiplier = 0;
                _lastPlayerKilled = playerData;
            }
            _moneyMultiplier++;
        }

        private void WriteInStatisticKill()
        {
            _gameEvents.UpdateStatisticGame.Invoke(_currentPlayerData.OpponentPlayer, _countMoney);
        }
    }
}