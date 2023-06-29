using System.Collections;
using UnityEngine;
using Zenject;
using Assets.Guns;
using UnityEngine.InputSystem;
using Assets.Items;
using UnityEngine.Animations.Rigging;

namespace Assets.Player
{
    public class WeaponControl : MonoBehaviour
    {
        [Header("Weapon setting")]
        [SerializeField] private Transform _aimingPoint;
        [SerializeField] private float _aimingDistance;
        [SerializeField] private RigLayer _aimRig;
        [Header("Weapon effects")]
        [SerializeField] private ParticleSystem _muzzleFlash;
        [SerializeField] private SpawnerBullet _bulletSpawner;
        [Header("Shot setting")]
        [SerializeField] private Animator _gunAnimator;
        [SerializeField] private GameObject _weaponContainer;
        [SerializeField] private Transform _aim;
        [Space(10)]
        [SerializeField] private Weapon[] _weaponSlots;

        private int _starterWeaponIndex = 0;
        private int _previousSlotIndex = -1;
        private float _rateOfFireWeapon;
        private float _damageWeapon;
        private InputSystem _inputSystem;
        private InputAction _inputActionShoot;
        private GlobalEventsSystem _gameEvents;
        private readonly float _removeWeaponsThroughTime = 1.5f;
        private float _tmpRateOfFireGun;
        private float _counterTime;
        private PlayerData _playerData;
        private string _tagCurrentPlayer;
        private string _tagTwoPlayer = "Pl2";

        [Inject]
        private void Construct(GlobalEventsSystem gameEvents)
        {
            _gameEvents = gameEvents;

            _gameEvents.onDisablePlayerMovement.AddListener(DisableMovement);
        }

        private void Awake()
        {
            _playerData = GetComponent<PlayerData>();
            _tagCurrentPlayer = transform.tag;
            _inputSystem = new InputSystem();

            _inputSystem?.Enable();
            IdentifyPlayer();
        }

        private void Start()
        {
            IdentifyWeapons();
            ChangeWeapon(_starterWeaponIndex);          
        }

        private void OnEnable() => _inputActionShoot.performed += Shoot;

        private void OnDisable() => _inputActionShoot.performed -= Shoot;

        private void Update()
        {
            TryOffAiming();
        }

        private void IdentifyPlayer()
        {
            _inputActionShoot = _inputSystem.PlayerOne.Shoot;

            if (transform.CompareTag(_tagTwoPlayer))
            {
                _tagTwoPlayer = "Pl1"; 

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

        private void DisableMovement(bool isDisable)
        {
            if (isDisable)
                _inputSystem?.Disable();
            else
                _inputSystem?.Enable();
        }

        private void Shoot(InputAction.CallbackContext callbackContext)
        {
            if (Time.time > _tmpRateOfFireGun && _weaponContainer.activeInHierarchy)
            {
                _tmpRateOfFireGun = Time.time + 1f / _rateOfFireWeapon;
                _counterTime = 0f;
                _gunAnimator.SetBool("WeaponAim", true);

                StartCoroutine(ActivateAim());
                StartCoroutine(ShootOnTarget());
            }
        }

        private IEnumerator ActivateAim()
        {
            float weight = _aimRig.rig.weight;
            for (float i = weight; i < 1; i += 0.1f)
            {
                _aimRig.rig.weight = i;
                
                yield return null;
            }
        }

        private IEnumerator ShootOnTarget()
        {
            float shotRadius = 60f;

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

                if (beamHit.point != Vector3.zero && beamHit.transform.TryGetComponent(out PlayerData player))
                {
                    _aim.position = beamHit.point;
                }
            }
        }

        private RaycastHit LaunchBeam(Vector3 beamDirection)
        {
            Physics.Raycast(_aimingPoint.position, -beamDirection, out RaycastHit hit, _aimingDistance);
            return hit;
        }

        private void TryOffAiming()
        {
            _counterTime += Time.deltaTime;
            if (_counterTime >= _removeWeaponsThroughTime)
            {
                _aim.localPosition = new Vector3(0, 2.75f, 5);
                StartCoroutine(DeactivateAim());
                _gunAnimator.SetBool("WeaponAim", false);
            }
        }

        private IEnumerator DeactivateAim()
        {
            float weight = _aimRig.rig.weight;
            for (float i = weight; i > 0; i -= 0.1f)
            {
                _aimRig.rig.weight = i;

                yield return null;
            }
        }
    }
}