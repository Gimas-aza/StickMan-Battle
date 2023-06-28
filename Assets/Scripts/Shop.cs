using UnityEngine;
using Assets.Player;
using Assets.Items;
using Assets.UI;
using System.Collections.Generic;

namespace Assets
{
    public class Shop : MonoBehaviour
    {
        [SerializeField] private List<Item> _itemList;
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private ItemView _template;
        [SerializeField] private bool _inverseItems = false;
        [SerializeField] private GameObject _itemContainer;
        [SerializeField] private float _marginTopForItems;

        private Vector3 _currentPositionItemView;
        private Vector2 _currentSizeDeltaItemView;
        private float _changePositionX;
        private float _changePositionY;
        private float _shiftXItems = -105;

        private void Start()
        {
            for (int i = 0; i < _itemList.Count; i++)
            {
                AddItem(_itemList[i], i);
            }
        }

        private void AddItem(Item item, int index)
        {
            var view = Instantiate(_template, _itemContainer.transform);
            view.onSellButtonClick += SellButtonClick;
            Vector3 changePosition = GetChangePositionItemView(view, index);

            view.Render(item, _inverseItems);
            view.SetLocationItemView(changePosition);
        }

        private void SellButtonClick(Item item, ItemView view)
        {
            TrySellItem(item, view);
        }
        
        private void TrySellItem(Item item, ItemView view)
        {
            if (_playerData.CountMoney >= item.Price)
            {
                _playerData.WithdrawMoney(item.Price);
                view.SetSuccessOfPurchase();
                item.GiveItemPlayer(_playerData);

                view.onSellButtonClick -= SellButtonClick;
            }
        }

        private Vector3 GetChangePositionItemView(ItemView view, int index)
        {
            _currentPositionItemView = view.RectTransform.localPosition;
            _currentSizeDeltaItemView = view.RectTransform.sizeDelta;
            
            SetPositionItem(index);
            ChangeDirectionOfCentreShift(index);

            return new Vector3(_changePositionX, _changePositionY, _currentPositionItemView.z);
        }

        private void SetPositionItem(int index)
        {            
            if (index == 0) 
            {
                _changePositionX = _currentPositionItemView.x;
                _changePositionY = _currentPositionItemView.y;
            }
            else
            {
                _changePositionX += _shiftXItems;
                _changePositionY += _currentPositionItemView.y - (_currentSizeDeltaItemView.y / 2) - _marginTopForItems;
            }
        }

        private void ChangeDirectionOfCentreShift(int index)
        {
            if (index == (_itemList.Count) / 2)
            {
                _shiftXItems *= -1;
            }
        }
    }
}