using GameJam.Features.CardSystem;
using GameJam.Features.UI;
using UnityEngine;
using UnityEngine.UI;

namespace GameResources.Features.UI.Scripts.Buttons {
    
    [RequireComponent(typeof(Button))]
    public class ReshuffleCardsButton : AbstractButton{
        
        private CardManager cardManager = default;
        
        protected override void Awake() {
            base.Awake();
            
            cardManager = FindObjectOfType<CardManager>();
        }
        
        protected override void ClickAction() {
            cardManager.TryToReshuffleCards();
        }
    }
}