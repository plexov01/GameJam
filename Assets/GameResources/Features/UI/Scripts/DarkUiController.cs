namespace GameJam.Features.UI
{
    using UnityEngine;
    using UnityEngine.UI;
    /// <summary>
    /// UI контроллер тёмной панели
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class DarkUiController : MonoBehaviour
    {
        private Image _image = default;
        private Animation _lightning;
        private void Awake()
        {
            _image = GetComponent<Image>();
            _lightning = GetComponent<Animation>();
            _lightning.clip = Resources.Load<AnimationClip>("Animations/Light");
            _lightning.AddClip(_lightning.clip, _lightning.clip.name);

        }
        /// <summary>
        /// Установить прозрачность панели
        /// </summary>
        /// <param name="alphaValue"></param>
        public void SetAlpha(float alphaValue)
        {
            if (alphaValue > 1)
            {
                alphaValue = 1f;
            }
            else if (alphaValue < 0)
            {
                alphaValue = 0f;
            }
            _image.color = new Color(0f, 0f, 0f, alphaValue);
            Debug.Log(_image.color);
        }
        /// <summary>
        /// Показать молниюю
        /// </summary>
        public void ShowLightning() => _lightning.Play();
    }
}

