using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Assets.Player
{
    [RequireComponent(typeof(PlayerData))]
    [RequireComponent(typeof(PlayerUI))]
    public class PlayerLogic : MonoBehaviour
    {
        [SerializeField] private float _currentPlayerHealth;
        [SerializeField] private PlayerLogic _opponentPlayerLogic;
        [SerializeField] private GameOver _gameOver;

        private PlayerData _playerData;
        private PlayerUI _playerUI;
        private int _countKill;
        private int _countMoney;
        private GlobalEventsSystem _gameEvents;

        public UnityAction onDeath;

        [Inject]
        private void Construct(GlobalEventsSystem gameEvents)
        {
            _gameEvents = gameEvents;

            _gameEvents.onHealthToMaximum.AddListener(SetHealthToMaximum);
        }

        private void Awake()
        {
            _playerData = GetComponent<PlayerData>();
            _playerUI = GetComponent<PlayerUI>();
        }

        void OnEnable()
        {
            onDeath += IncreaseCashBalance;
            onDeath += UpdateStatistic;
        }

        void OnDisable()
        {
            onDeath -= IncreaseCashBalance;
            onDeath -= UpdateStatistic;
        }

        private void Start()
        {
            SetHealthToMaximum();
            InitializePlayerData();
        }

        public void InflictDamage(float damage)
        {
            if (damage >= _currentPlayerHealth)
            {
                _currentPlayerHealth = 0;

                _opponentPlayerLogic.onDeath?.Invoke();

                _gameEvents.onDisablePlayerMovement?.Invoke(true);
                _gameEvents.onGameOverMenu?.Invoke();
            }
            else
                _currentPlayerHealth -= damage;

            _playerUI.UpdateHealthBar(_currentPlayerHealth, _playerData.Health);
        }

        private void SetHealthToMaximum()
        {
            _currentPlayerHealth = _playerData.Health;
            _playerUI.UpdateHealthBar(_currentPlayerHealth, _playerData.Health);
        }

        private void InitializePlayerData()
        {
            _countKill = _playerData.CountKill;
            _countMoney = _playerData.CountMoney;

            _playerUI.RenderStatisticMenu(_countKill, _countMoney);
        }

        private void IncreaseCashBalance()
        {
            int moneyMultiplier = _gameOver.GetMoneyMultiplier(_playerData);

            _countMoney += moneyMultiplier * 5;
        }

        private void UpdateStatistic()
        {
            _playerData.UpdateStatistic(_countMoney);
            _countKill = _playerData.CountKill;

            _playerUI.RenderStatisticMenu(_countKill, _countMoney);
        }
    }
}