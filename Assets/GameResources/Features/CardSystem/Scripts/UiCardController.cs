namespace GameJam.Features.CardSystem
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// UI Показа карточек для выбора
    /// </summary>
    public class UiCardController : MonoBehaviour
    {
        private List<ChooseCardButton> _currentButtons = new List<ChooseCardButton>();
        private List<GameObject> _currentCardsGO = new List<GameObject>();
        
        private List<ChooseCardButton> _chooseCardButtons = new List<ChooseCardButton>();

        private void Awake()
        {
            _currentButtons = GetComponentsInChildren<ChooseCardButton>().ToList();

            for (int i = 0; i < _currentButtons.Count; i++)
            {
                _currentButtons[i].gameObject.SetActive(false);
            }
        }


        public void ShowCards(List<AbstractCard> cards)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                _currentButtons[i].SetNameCard(cards[i].name);
                _currentButtons[i].gameObject.GetComponent<Image>().sprite = cards[i].CardSprite;
                _currentButtons[i].gameObject.SetActive(true);
            }
        }
        
        public void ShowUIChooseCard()
        {
            gameObject.SetActive(true);
        }

        public void HideUIChooseCard()
        {
            gameObject.SetActive(false);
        }
        
    }
}

