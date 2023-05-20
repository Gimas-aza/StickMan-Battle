using Assets.Player;
using UnityEngine;
using Zenject;

namespace Assets
{
    public class Main : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerOneData;
        [SerializeField] private PlayerData _playerTwoData;

        private GameEventsServise _gameEvents;

        [Inject]
        private void Construct(GameEventsServise gameEvents)
        {
            _gameEvents = gameEvents;

            _gameEvents.RestartLevel.AddListener(ResetPosition);
            _gameEvents.RestartLevel.AddListener(ResetHealth);
        }

        private void ResetPosition()
        {
            Transform playerOneTransform = _playerOneData.gameObject.transform;
            Transform playerTwoTransform = _playerTwoData.gameObject.transform;

            playerOneTransform.position = new Vector3(0, 0, _playerOneData.Spawn);
            playerTwoTransform.position = new Vector3(0, 0, _playerTwoData.Spawn);
            playerOneTransform.eulerAngles = new Vector3(0, 0, 0);
            playerTwoTransform.eulerAngles = new Vector3(0, 180, 0);
        }

        private void ResetHealth()
        {
            _gameEvents.HealthToMaximum.Invoke();

            _gameEvents.UpdateHealthBar.Invoke(_playerOneData, _playerOneData.Health);
            _gameEvents.UpdateHealthBar.Invoke(_playerTwoData, _playerTwoData.Health);
        }
    }
}