namespace GameJam.Features.UI
{
    using System;
    using System.Collections;
    using UnityEngine;
    using Random = UnityEngine.Random;

    /// <summary>
    /// Контроллер управления темнотой
    /// </summary>
    public class DarkController : MonoBehaviour
    {
        public float TimeShowDark = default;
        public float LightingFrequencyTime = default;

        private Coroutine _coroutineShowDark = default;
        private Coroutine _coroutineShowLightning = default;
        
        private DarkUiController _darkUiController = default;

        private IEnumerator DarkActive(float time)
        {
            CoolnessScaleController.Instance.isDark = true;
            _darkUiController.SetAlpha(0.99f);
            _coroutineShowLightning = StartCoroutine(LightningActive(LightingFrequencyTime));
            yield return new WaitForSecondsRealtime(time);
            StopCoroutine(_coroutineShowLightning);
            _darkUiController.StopAnimation();
            _darkUiController.SetAlpha(0);
            CoolnessScaleController.Instance.isDark = false;
            _coroutineShowDark = null;
        }
        
        private IEnumerator LightningActive(float time)
        {
            while (true)
            {
                _darkUiController.ShowLightning();
                yield return new WaitForSeconds(time + Random.Range(-time/5, time/2));
            }
            
        }

        private void Awake()
        {
            _darkUiController = FindObjectOfType<DarkUiController>();
        }
        /// <summary>
        /// Активировать тьму
        /// </summary>
        public void ShowDark()
        {
            if (_coroutineShowDark != null)
            {
                StopCoroutine(_coroutineShowDark);
                StopCoroutine(_coroutineShowLightning);
            }
            
            _coroutineShowDark = StartCoroutine(DarkActive(TimeShowDark));
        }
        
    }

}
