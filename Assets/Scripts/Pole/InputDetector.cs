using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Pole
{
    public class InputDetector : MonoBehaviour
    {
        [SerializeField] private Image _meterFiller;
        [SerializeField] private float _fillSpeed = 1f;
        [SerializeField] private float _minForce = 5f;
        [SerializeField] private float _maxForce = 20f;
        
        private IEnumerator _fillMeterCoroutine;
        private bool _isHolding = false;
        private float _holdTime = 0f;

        public event Action<float> ForceSelected; 
        
        public void StartDetectingInput()
        {
            StopDetectingInput();

            _fillMeterCoroutine = DetectInput();
            StartCoroutine(_fillMeterCoroutine);
        }

        public void StopDetectingInput()
        {
            if (_fillMeterCoroutine != null)
            {
                StopCoroutine(_fillMeterCoroutine);
                _fillMeterCoroutine = null;
            }
        }

        private IEnumerator DetectInput()
        {
            while (true)
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Began)
                    {
                        OnTouchDown();
                    }
                    else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    {
                        OnTouchUp();
                    }

                    if (_isHolding)
                    {
                        _holdTime += Time.deltaTime;
                        _meterFiller.fillAmount = Mathf.Clamp01(_holdTime * _fillSpeed);
                    }
                }

                yield return null;
            }
        }
        
        private void OnTouchDown()
        {
            _isHolding = true;
            _holdTime = 0f;
            _meterFiller.fillAmount = 0f;
        }

        private void OnTouchUp()
        {
            _isHolding = false;
            
            float force = Mathf.Lerp(_minForce, _maxForce, _meterFiller.fillAmount);
            ForceSelected?.Invoke(force);
        }
    }
}
