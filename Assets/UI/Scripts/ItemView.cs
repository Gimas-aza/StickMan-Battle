using Assets.Items;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.UI 
{
    [RequireComponent(typeof(RectTransform))]
    public class ItemView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _label;
        [SerializeField] private TMP_Text _price;
        [SerializeField] private Image _icon;

        private Item _item;
        private readonly string _wordBeforePrice = "Стоимость: ";
        private bool _isBuyItem = false;
        private RectTransform _rectTransform;

        public bool IsBuyItem => _isBuyItem;
        public RectTransform RectTransform => _rectTransform;

        public UnityAction<Item, ItemView> onSellButtonClick;

        void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Render(Item item, bool inverse = false)
        {
            _item = item;

            _label.text = item.Label;
            _price.text = _wordBeforePrice + item.Price;
            _icon.sprite = item.Icon;

            if (inverse)
            {
                _label.gameObject.transform.localRotation = Quaternion.Euler(0, 180, 0);
                _price.gameObject.transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }

        public void ClickButton()
        {
            onSellButtonClick?.Invoke(_item, this);
        }

        public void SetSuccessOfPurchase() 
        { 
            _isBuyItem = true; 
        }

        public void SetLocationItemView(Vector3 startPosition)
        {
            _rectTransform.localPosition = startPosition;
        }
    }
}
