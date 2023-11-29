using System;
using System.Collections.Generic;

namespace GameJam.Features.CardSystem
{
    using UnityEngine;
    
    /// <summary>
    /// Абстрактная карточка с событием
    /// </summary>
    public abstract class AbstractCard : ScriptableObject {
        
        public static event EventHandler OnCardActivated;
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
        public List<AudioClip> EventSoundList = new List<AudioClip>();

        /// <summary>
        /// Активировать карту
        /// </summary>
        public virtual void ActivateCard()
        {
            OnCardActivated?.Invoke(this, EventArgs.Empty);
            CoolnessScaleController.Instance.AddCoolness(Coolness);
        }

    }

}
