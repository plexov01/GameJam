namespace GameJam.Features.CardSystem
{
    using GameJam.Features.CardSystem;
    using GameJam.Features.UI;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    /// <summary>
    /// Кнопка активации карточки
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class ChooseCardButton : AbstractButton
    {
        private string _nameCard = default;
        
        private CardManager _cardManager = default;
        private UiCardController _uiCardController = default;
        
        protected override void Awake()
        {
            base.Awake();
            
            _cardManager = FindObjectOfType<CardManager>();
            _uiCardController = FindObjectOfType<UiCardController>();
        }
        
        protected override void ClickAction()
        {
            _cardManager.SelectCard(_nameCard);
        }
        /// <summary>
        /// Установить карту, которую активирует кнопка
        /// </summary>
        /// <param name="name"></param>
        public void SetNameCard(string name) => _nameCard = name;
        
        
    }
}

