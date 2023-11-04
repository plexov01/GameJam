namespace GameJob.Features.CardSystem
{
    using UnityEngine;
    
    /// <summary>
    /// Абстрактная карточка с событием
    /// </summary>
    public abstract class AbstractCard : ScriptableObject
    {
        /// <summary>
        /// Название
        /// </summary>
        public string Name = default;
        /// <summary>
        /// Описание
        /// </summary>
        public string Description = default;
        /// <summary>
        /// Картинка карты
        /// </summary>
        public Sprite CardSprite = default;
        /// <summary>
        /// Звук события в карте
        /// </summary>
        public AudioSource EventSound = default;
        
        /// <summary>
        /// Активировать карту
        /// </summary>
        public abstract void ActivateCard();

    }

}
