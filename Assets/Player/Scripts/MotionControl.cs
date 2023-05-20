using Assets.Level;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Assets.Player
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(WeaponControl))]
    public class MotionControl : MonoBehaviour
    {
        [Header("Moveming")]
        [SerializeField] private float _speedMoveming;
        [SerializeField] private float _jerkTime;
        [SerializeField] private float _jerkForce;
        [SerializeField] private float _jerkCooldown;

        [Header("Animation")]
        [SerializeField] private Animator _playerAnimator;
        [SerializeField] private AnimationCurve _jerkAnimationCurve;

        private Rigidbody _rigidbody;
        private PlayerData _playerData;
        private InputSystem _inputSystem;
        private InputAction _inputActionMovement;
        private InputAction _inputActionJerk;

        private Vector3 _drivingDirections;
        private Vector3 _drivingDirectionsSpeed;
        private Vector3 _playerLastPosition;
        private Vector3 _finalPositionJerk;

        private float _recoveryJerkTime;
        private bool _isWalkLock = false;
        private bool _isJerkLock = false;
        private string _tagTwoPlayer = "Pl2";

        private GameEventsServise _gameEvents;

        [Inject]
        private void Construct(GameEventsServise gameEvents)
        {
            _gameEvents = gameEvents;

            _gameEvents.TriggerWall.AddListener(TryToBlockJerk);
            _gameEvents.DisablePlayerMovement.AddListener(DisablePlayerMovement);
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _playerData = GetComponent<PlayerData>();
            _inputSystem = new InputSystem();

            _inputSystem?.Enable();
            IdentifyPlayer();
        }

        private void FixedUpdate()
        {
            Movement();
            StartCoroutine(PerformJerk());
        }

        private void TryToBlockJerk()
        {
            if (!_isJerkLock)
            {
                _playerLastPosition = RoundOffVector(_drivingDirections);
                _isJerkLock = true;
            }
        }

        private void DisablePlayerMovement(bool isDisable)
        {
            if (isDisable) 
                _inputSystem?.Disable();
            else
                _inputSystem?.Enable();
        }

        private void IdentifyPlayer()
        {
            _inputActionMovement = _inputSystem.PlayerOne.Move;
            _inputActionJerk = _inputSystem.PlayerOne.Jerk;

            if (transform.CompareTag(_tagTwoPlayer))
            {
                _tagTwoPlayer = "Pl1";

                _inputActionMovement = _inputSystem.PlayerTwo.Move;
                _inputActionJerk = _inputSystem.PlayerTwo.Jerk;
            }
        }

        private void Movement()
        {
            _drivingDirections = ReadMovement();
            _drivingDirectionsSpeed = Vector3.ClampMagnitude(_drivingDirections * _speedMoveming, _speedMoveming);

            MoveCharacter();

            TryToUnlockJerk(_drivingDirections);
        }

        private Vector3 ReadMovement()
        {
            Vector2 direction = _inputActionMovement.ReadValue<Vector2>();

            return new Vector3(direction.y, 0.0f, -direction.x);
        }

        private void MoveCharacter()
        {
            if (_drivingDirectionsSpeed != Vector3.zero && !_isWalkLock)
            {
                Quaternion SmoothRotate = Quaternion.LookRotation(-_drivingDirectionsSpeed);

                _rigidbody.MovePosition(transform.position + _drivingDirectionsSpeed * Time.deltaTime);
                _rigidbody.MoveRotation(SmoothRotate);

                _playerAnimator.SetBool("Run", true);
            }
            else
            {
                _playerAnimator.SetBool("Run", false);
            }
        }

        private void TryToUnlockJerk(Vector3 drivingDirections)
        {
            if (_playerLastPosition != RoundOffVector(drivingDirections) && drivingDirections != Vector3.zero)
            {
                _isJerkLock = false;
            }
        }

        private IEnumerator PerformJerk()
        {
            bool buttonJerk = _inputActionJerk.ReadValue<float>() == 1;

            if (buttonJerk && _recoveryJerkTime == 0 && _drivingDirections != Vector3.zero && !_isJerkLock)
            {
                _playerAnimator.SetBool("Jerk", true);
                _isWalkLock = true;

                SetFinalPositionJerk();

                yield return StartCoroutine(PerformJerkCharacter());

                _isWalkLock = false;
                _playerAnimator.SetBool("Jerk", false);
            }
            else if (_recoveryJerkTime != 0)
            {
                RegainingAbilityToJerk();
            }
        }

        private void SetFinalPositionJerk()
        {
            RaycastHit raycastHit;
            Vector3 raycastStart = new Vector3(transform.position.x, 1, transform.position.z);

            _drivingDirections = RoundOffVector(_drivingDirections);
            _finalPositionJerk = transform.position + _drivingDirections * _jerkForce;
            if (Physics.Raycast(raycastStart, _finalPositionJerk, out raycastHit, _jerkForce))
            {
                Debug.Log(raycastHit.collider.name);
            }

            //_finalPositionJerk = new Vector3(Mathf.Clamp(_finalPositionJerk.x, -13.51f, 13.51f), _finalPositionJerk.y, Mathf.Clamp(_finalPositionJerk.z, -28.54f, 28.54f));
        }

        private Vector3 RoundOffVector(Vector3 drivingDirections)
        {
            return new Vector3(Mathf.Ceil(drivingDirections.x), Mathf.Ceil(drivingDirections.y), Mathf.Ceil(drivingDirections.z));
        }

        private IEnumerator PerformJerkCharacter()
        {
            for (float currentJerkTime = 0; currentJerkTime < 0.1f; currentJerkTime += Time.fixedDeltaTime / _jerkTime)
            {
                _rigidbody.MovePosition(Vector3.Lerp(transform.position, _finalPositionJerk, _jerkAnimationCurve.Evaluate(currentJerkTime)));

                _recoveryJerkTime += Time.fixedDeltaTime / _jerkTime;

                _gameEvents.UpdateJerkBar.Invoke(_playerData, -0.02f);

                yield return null;
            }
        }

        private void RegainingAbilityToJerk()
        {
            _recoveryJerkTime += Time.fixedDeltaTime;
            _gameEvents.UpdateJerkBar.Invoke(_playerData, 0.008f);

            if (_recoveryJerkTime >= _jerkCooldown)
            {
                Debug.Log(_recoveryJerkTime);
                _recoveryJerkTime = 0;
            }
        }
    }
}