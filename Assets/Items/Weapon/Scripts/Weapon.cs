using UnityEngine;

namespace Assets.Items
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private WeaponInfo _weaponFeatures;
        [SerializeField] private GameObject _gunModel;

        public WeaponInfo WeaponFeatures => _weaponFeatures;
        public GameObject GunModel => _gunModel;
    }
}
