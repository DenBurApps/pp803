using System;
using System.Collections;
using UnityEngine;

namespace Pole
{
    public class RotateScreen : MonoBehaviour
    {
        private int _showDuration = 2;
        private int _interval = 1;

        public event Action ScreenClosed;

        private void Start()
        {
            StartCoroutine(ShowScreen());
        }

        private IEnumerator ShowScreen()
        {
            var interval = new WaitForSeconds(_interval);

            for (int i = 0; i <= _showDuration; i++)
            {
                yield return interval;
            }

            ScreenClosed?.Invoke();
            gameObject.SetActive(false);
        }
    }
}