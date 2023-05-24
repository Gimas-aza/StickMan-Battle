using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Assets.Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerData))]
    public class MotionControl : MonoBehaviour
    {
        [Header("Moveming")]
        [SerializeField] private float _speedMove;
        [SerializeField] private float _speedRotate;

        [Header("Animation")]
        [SerializeField] private Animator _playerAnimator;

        private CharacterController _controller;
        private PlayerData _playerData;
        private InputSystem _inputSystem;
        private InputAction _inputActionMovement;

        private Vector3 _drivingDirections;
        private Vector3 _drivingDirectionsSpeed;

        private bool _isWalkLock = false;
        private string _tagTwoPlayer = "Pl2";

        private GameEventsServise _gameEvents;

        [Inject]
        private void Construct(GameEventsServise gameEvents)
        {
            _gameEvents = gameEvents;

            _gameEvents.DisablePlayerMovement.AddListener(DisablePlayerMovement);
        }

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _playerData = GetComponent<PlayerData>();
            _inputSystem = new InputSystem();

            Init();
        }

        private void FixedUpdate()
        {
            Movement();
        }

        private void Init()
        {
            _inputSystem?.Enable();

            _inputActionMovement = _inputSystem.PlayerOne.Move;

            if (transform.CompareTag(_tagTwoPlayer))
            {
                _tagTwoPlayer = "Pl1";

                _inputActionMovement = _inputSystem.PlayerTwo.Move;
            }
        }

        private void DisablePlayerMovement(bool isDisable)
        {
            if (isDisable) 
                _inputSystem?.Disable();
            else
                _inputSystem?.Enable();
        }

        private void Movement()
        {
            _drivingDirections = ReadMovement();
            _drivingDirectionsSpeed = Vector3.ClampMagnitude(_drivingDirections * _speedMove, _speedMove);

            MoveAnimation(_drivingDirectionsSpeed);
            RotateCharacter(_drivingDirectionsSpeed);
            MoveCharacter(_drivingDirectionsSpeed);
        }

        private Vector3 ReadMovement()
        {
            Vector2 direction = _inputActionMovement.ReadValue<Vector2>();

            return new Vector3(direction.y, 0.0f, -direction.x);
        }

        private void MoveCharacter(Vector3 moveDirection)
        {
            _controller.Move(moveDirection * Time.deltaTime);
        }

        private void RotateCharacter(Vector3 moveDirection)
        {
            GameObject playerModel = _playerAnimator.gameObject;
            
            if (Vector3.Angle(transform.forward, -moveDirection) > 0)
            {                
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, -moveDirection, _speedRotate, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDirection);
            }
        }

        private void MoveAnimation(Vector3 moveDirection)
        {
            if (moveDirection != Vector3.zero && !_isWalkLock)
                _playerAnimator.SetBool("Run", true);
            else
                _playerAnimator.SetBool("Run", false);
        }
    }
}