using UnityEngine;
using Assets.Player;

namespace Assets.Items
{
    [CreateAssetMenu(menuName = "Items/new weapon")]
    public class WeaponInfo : Item
    {
        [SerializeField] private string _label;
        [SerializeField] private float _damage;
        [SerializeField] private float _rateOfFire;
        [SerializeField] private int _price;
        [SerializeField] private Sprite _icon;

        public override string Label => _label;
        public float Damage => _damage;
        public float RateOfFire => _rateOfFire;
        public override int Price => _price;
        public override Sprite Icon => _icon;

        [HideInInspector] public int WeaponSlotIndex;
        
        public override void GiveItemPlayer(PlayerLogic player)
        {
            var weaponControl = player.GetComponent<WeaponControl>();

            weaponControl.ChangeWeapon(WeaponSlotIndex);
        }
    }
}