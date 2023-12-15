namespace GameJam.Features.UI
{
    using UnityEngine;
    using UnityEngine.UI;
    /// <summary>
    /// Абстрактный класс кнопки
    /// </summary>
    [RequireComponent(typeof(Button))]
    public abstract class AbstractButton : MonoBehaviour
    {
        protected Button _button = default;

        protected virtual void Awake()
        {
            _button = GetComponent<Button>();
            
            _button.onClick.AddListener(ClickAction);
        }

        protected abstract void ClickAction();

        protected virtual  void OnDestroy() => _button.onClick.RemoveListener(ClickAction);
    }
}

