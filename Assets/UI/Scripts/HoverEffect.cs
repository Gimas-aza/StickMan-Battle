using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.UI
{
    [RequireComponent(typeof(Image))]
    public class HoverEffect : MonoBehaviour
    {
        private Image _hoverImage;
        private readonly float _minHover = 0f;
        private readonly float _maxHover = 1f;
        private readonly float _startTime = 1f;
        private readonly float _endTime = 0.5f;
        
        private void Awake()
        {
            _hoverImage = GetComponent<Image>();
        }

        public void StartHover()
        {
            StartCoroutine(SetSmoothHover(_maxHover, _startTime));
        }

        public void EndHover()
        {
            StopAllCoroutines();
            StartCoroutine(SetSmoothHover(_minHover, _endTime));
        }

        IEnumerator SetSmoothHover(float target, float time)
        {
            for (float i = 0; i < 0.5f; i += Time.deltaTime / time)
            {
                _hoverImage.fillAmount = Mathf.Lerp(_hoverImage.fillAmount, target, i);
                yield return null;
            }

            _hoverImage.fillAmount = target;
        }
    }
}