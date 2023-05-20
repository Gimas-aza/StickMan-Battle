using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Player;

namespace Assets.Items
{
    public abstract class Item : ScriptableObject
    {
        private readonly string _label;
        private readonly int _price;
        private readonly Sprite _icon;

        public virtual string Label => _label;
        public virtual int Price => _price;
        public virtual Sprite Icon => _icon;
        
        public abstract void GiveItemPlayer(PlayerData player);
    }
}
