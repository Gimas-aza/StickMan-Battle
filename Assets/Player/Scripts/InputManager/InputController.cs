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
        private CharacterController _characterController;
        private GlobalEventsSystem _gameEvents;

        [Inject]
        private void Construct(GlobalEventsSystem gameEvents)
        {
            _gameEvents = gameEvents;

            _gameEvents.onDisablePlayerMovement.AddListener(DisableMovement);
        }

        public void Awake()
        {
            _inputSystem = new InputSystem();
            if (TryGetComponent(out IControllable controllable))
                _controllable = controllable;
            else
                throw new Exception("Не найден IControllable");
              
            _characterController = GetComponent<CharacterController>();
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

        private void DisableMovement(bool isDisable)
        {
            if (isDisable) 
                _inputSystem?.Disable();
            else
                _inputSystem?.Enable();

            _characterController.enabled = !isDisable;
        }

        private void ReadMove()
        {
            Vector2 direction = _inputActionMovement.ReadValue<Vector2>();

            _controllable.SetDirectionMove(new Vector3(direction.y, 0.0f, -direction.x));
        }
    }
}
