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
        /// Особая карта
        /// </summary>
        public bool Special = default;
        /// <summary>
        /// Крутость карты
        /// </summary>
        public int Coolness = default;
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
        public virtual void ActivateCard()
        {
            CoolnessScaleController.Instance.AddCoolness(Coolness);
        }

    }

}
