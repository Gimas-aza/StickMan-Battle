using UnityEngine;
using Assets.Player;

namespace Assets.Items
{
    [CreateAssetMenu(menuName = "Items/new itemImprovements")]
    public class ItemForImprovements : Item
    {
        [SerializeField] private string _label;
        [SerializeField] private int _effect;
        [SerializeField] private int _price;
        [SerializeField] private Sprite _icon;

        public override string Label => _label;
        public int Effect => _effect;
        public override int Price => _price;
        public override Sprite Icon => _icon;

        public override void GiveItemPlayer(PlayerData player)
        {
            player.IncreaseMaxHealth(_effect);
        }
    }
}
