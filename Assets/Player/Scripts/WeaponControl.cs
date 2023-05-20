using System.Collections;
using UnityEngine;
using Zenject;
using Assets.Guns;
using UnityEngine.InputSystem;
using Assets.Items;

namespace Assets.Player
{
    public class WeaponControl : MonoBehaviour
    {
        [Header("Weapon setting")]
        [SerializeField] private Transform _aimingPoint;
        [SerializeField] private float _aimingDistance;
        [Header("Weapon effects")]
        [SerializeField] private ParticleSystem _muzzleFlash;
        [SerializeField] private SpawnerBullet _bulletSpawner;
        [SerializeField] private AudioClip _shotSFX;
        [SerializeField] private AudioSource _audioSource;
        [Header("Shot setting")]
        [SerializeField] private Animator _gunAnimator;
        [SerializeField] private GameObject _weaponContainer;
        [SerializeField] private Transform _drawRayForAim;
        [Space(10)]
        [SerializeField] private Weapon[] _weaponSlots;

        private Rigidbody _rigidbody;
        private int _previousSlotIndex = -1;
        private float _rateOfFireWeapon;
        private float _damageWeapon;

        private InputSystem _inputSystem;
        private InputAction _inputActionShoot;
        private GameEventsServise _gameEvents;
        private readonly float _removeWeaponsThroughTime = 1f;
        private float _tmpRateOfFireGun;
        private float _counterTime;
        private PlayerData _playerData;
        private string _tagCurrentPlayer;
        private string _tagTwoPlayer = "Pl2";

        [Inject]
        private void Construct(GameEventsServise gameEvents)
        {
            _gameEvents = gameEvents;

            _gameEvents.DisablePlayerMovement.AddListener(DisablePlayerMovement);
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _playerData = GetComponent<PlayerData>();
            _tagCurrentPlayer = transform.tag;
            _inputSystem = new InputSystem();

            _inputSystem?.Enable();
            IdentifyPlayer();
        }

        private void Start()
        {
            IdentifyWeapons();
            ChangeWeapon(0);          
        }

        private void OnEnable() => _inputActionShoot.performed += Shoot;

        private void OnDisable() => _inputActionShoot.performed -= Shoot;

        public void ChangeWeapon(int index)
        {
            _damageWeapon = _weaponSlots[index].WeaponFeatures.Damage;
            _rateOfFireWeapon = _weaponSlots[index].WeaponFeatures.RateOfFire;

            _weaponSlots[index].GunModel.SetActive(true);
            _gunAnimator.SetInteger("IndexWeapon", index);

            DeactivatePreviousWeapon(index);
        }

        private void DeactivatePreviousWeapon(int index)
        {
            if (_previousSlotIndex != -1)
                _weaponSlots[_previousSlotIndex].GunModel.SetActive(false);

            _previousSlotIndex = index;
        }

        private void IdentifyPlayer()
        {
            _inputActionShoot = _inputSystem.PlayerOne.Shoot;

            if (transform.CompareTag(_tagTwoPlayer))
            {
                _tagTwoPlayer = "Pl2"; 

                _inputActionShoot = _inputSystem.PlayerTwo.Shoot; 
            }
        }

        private void IdentifyWeapons()
        {
            for (int i = 0; i < _weaponSlots.Length; i++)
            {
                _weaponSlots[i].WeaponFeatures.WeaponSlotIndex = i;
            }
        }

        private void DisablePlayerMovement(bool isDisable)
        {
            if (isDisable)
                _inputSystem?.Disable();
            else
                _inputSystem?.Enable();
        }

        private void Shoot(InputAction.CallbackContext callbackContext)
        {
            _counterTime += Time.fixedDeltaTime;

            //Debug.DrawRay(_drawRayForAim.transform.position, -transform.forward * 4, Color.red);

            if (Time.time > _tmpRateOfFireGun && _weaponContainer.activeInHierarchy)
            {
                _tmpRateOfFireGun = Time.time + 1f / _rateOfFireWeapon;
                _counterTime = 0f;
                _gunAnimator.SetBool("WeaponAim", true);

                StartCoroutine(ShootOnTarget());
            }
            else if (_counterTime >= _removeWeaponsThroughTime)
                _gunAnimator.SetBool("WeaponAim", false);
        }

        IEnumerator ShootOnTarget()
        {
            float shotRadius = 80f;

            //_audioSource.PlayOneShot(_shotSFX);

            LaunchBeamsInRadius(shotRadius);

            yield return null;

            _muzzleFlash.Play();
            _bulletSpawner.ShotBullet(_damageWeapon);
        }

        private void LaunchBeamsInRadius(float shotRadius)
        {
            Vector3 beamDirection;
            RaycastHit beamHit;

            for (float deg = -shotRadius; deg <= shotRadius; deg++)
            {
                beamDirection = Quaternion.Euler(0, deg, 0) * transform.forward;
                beamHit = LaunchBeam(beamDirection);

                if (beamHit.point != Vector3.zero && beamHit.transform.CompareTag(_tagTwoPlayer))
                    _rigidbody.MoveRotation((Quaternion.LookRotation(beamDirection)));
            }
        }

        private RaycastHit LaunchBeam(Vector3 beamDirection)
        {
            Physics.Raycast(_aimingPoint.position, -beamDirection, out RaycastHit hit, _aimingDistance);
            return hit;
        }

        
    }
}