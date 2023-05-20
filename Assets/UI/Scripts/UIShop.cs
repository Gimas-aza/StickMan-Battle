using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.UI
{
    public class UIShop : MonoBehaviour
    {
        [SerializeField] private float _timeAnimationMenu;
        [SerializeField] private AnimationCurve _animationCurveForMenu;

        private RectTransform _rectTransform;
        private float _menuLocation;
        private readonly float _centerMenuLocation = 0f;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _menuLocation = _rectTransform.anchoredPosition.x;
        }

        public void MoveMenuShop()
        {
            float currentMenuLocation = _rectTransform.anchoredPosition.x;

            if (currentMenuLocation != 0)
            {
                StartCoroutine(MoveMenuShopInCoroutine(_centerMenuLocation, _timeAnimationMenu));
            }
            else
            {
                StartCoroutine(MoveMenuShopInCoroutine(_menuLocation, _timeAnimationMenu));
            }
        }

        private IEnumerator MoveMenuShopInCoroutine(float direction, float time)
        {
            for (float i = 0; i < 0.5f; i += Time.deltaTime / time)
            {
                _rectTransform.anchoredPosition = Vector2.Lerp(_rectTransform.anchoredPosition,
                                                       new Vector2(direction, 0),
                                                       _animationCurveForMenu.Evaluate(i));
                yield return null;
            }
            _rectTransform.anchoredPosition = new Vector2(direction, 0);
        }
    }
}
