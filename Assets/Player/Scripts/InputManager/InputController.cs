using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Assets.Player
{
    public class InputController : MonoBehaviour
    {
        private InputSystem _inputSystem;
        private IControllable _controllable;
        private InputAction _inputActionMovement;
        private string _tagTwoPlayer = "Pl2";
        private GameEventsServise _gameEvents;

        [Inject]
        private void Construct(GameEventsServise gameEvents)
        {
            _gameEvents = gameEvents;

            _gameEvents.DisablePlayerMovement.AddListener(OnDisablePlayerMovement);
        }

        public void Awake()
        {
            _inputSystem = new InputSystem();
            if (TryGetComponent(out IControllable controllable))
                _controllable = controllable;
            else
                throw new Exception("Не найден IControllable");
                
            Init();
        }

        void Update()
        {
            ReadMove();
        }

        private void Init()
        {
            _inputSystem?.Enable();

            _inputActionMovement = _inputSystem.PlayerOne.Move;

            if (transform.CompareTag(_tagTwoPlayer))
            {
                _inputActionMovement = _inputSystem.PlayerTwo.Move;
            }
        }

        private void OnDisablePlayerMovement(bool isDisable)
        {
            if (isDisable) 
                _inputSystem?.Disable();
            else
                _inputSystem?.Enable();
        }

        private void ReadMove()
        {
            Vector2 direction = _inputActionMovement.ReadValue<Vector2>();

            _controllable.SetDirectionMove(new Vector3(direction.y, 0.0f, -direction.x));
        }
    }
}
