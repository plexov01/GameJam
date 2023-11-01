namespace GameJam.Features.UI
{
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Реализация кнопки, которая показывает панель
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class ShowPanelButton : AbstractButton
    {
        public string NamePanelForShow = default;

        private PanelsManager _panelsManager = default;

        private void Start()
        {
            _panelsManager = FindObjectOfType<PanelsManager>();
            
            if (_panelsManager==null)
            {
                Debug.LogWarning($"На сцене нет PanelsManager");
            }
        }

        protected override void ClickAction()
        {
            _panelsManager.ShowPanel(NamePanelForShow);
        }
    }

}
