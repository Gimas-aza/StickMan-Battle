using UnityEngine;

namespace Assets.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class MotionControl : MonoBehaviour, IControllable
    {
        [Header("Moveming")]
        [SerializeField] private float _speedMove;
        [SerializeField] private float _speedRotate;

        [Header("Animation")]
        [SerializeField] private Animator _playerAnimator;

        private CharacterController _controller;
        private Vector3 _drivingDirections = Vector3.zero;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        private void FixedUpdate()
        {
            MoveInternal();
        }

        public void SetDirectionMove(Vector3 direction)
        {
            _drivingDirections = direction;
        }

        private void MoveInternal()
        {
            Vector3 drivingDirectionsSpeed = Vector3.ClampMagnitude(_drivingDirections * _speedMove, _speedMove);

            MoveAnimation(drivingDirectionsSpeed);
            RotateCharacter(drivingDirectionsSpeed);
            MoveCharacter(drivingDirectionsSpeed);
        }

        private void MoveCharacter(Vector3 moveDirection)
        {
            _controller.Move(moveDirection * Time.deltaTime);
        }

        private void RotateCharacter(Vector3 moveDirection)
        {            
            if (Vector3.Angle(transform.forward, -moveDirection) > 0)
            {                
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, -moveDirection, _speedRotate, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDirection);
            }
        }

        private void MoveAnimation(Vector3 moveDirection)
        {
            if (moveDirection != Vector3.zero)
                _playerAnimator.SetBool("Run", true);
            else
                _playerAnimator.SetBool("Run", false);
        }
    }
}