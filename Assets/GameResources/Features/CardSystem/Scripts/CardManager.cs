namespace GameJob.Features.CardSystem
{
    using GameJam.Features.CardSystem;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Random = UnityEngine.Random;

    /// <summary>
    /// Менеджер карт
    /// </summary>
    public class CardManager : MonoBehaviour
    {
        
        private List<AbstractCard> _abstractCards = new List<AbstractCard>();

        private bool _cardSelected = false;
        private int _numberOfSelection = default;

        public bool FirstStage = default;

        private UiCardController _uiCardController = default;

        private void Awake()
        {
            _abstractCards = Resources.LoadAll<AbstractCard>("Cards").ToList();

            _uiCardController = FindObjectOfType<UiCardController>(true);
        }

        private void OnEnable()
        {
            GameHandler.OnStateChanged += DoLogicOnStages;
        }

        private void OnDisable()
        {
            GameHandler.OnStateChanged -= DoLogicOnStages;
        }

        /// <summary>
        /// Получить карты в указаном количестве
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public List<AbstractCard> GetRandomCards(int number)
        {
            List<AbstractCard> cards = new List<AbstractCard>();
            for (int i = 0; i < number; i++)
            {
                cards.Add(_abstractCards[Random.Range(0,_abstractCards.Count)]);
            }
            
            return cards;
        }
        /// <summary>
        /// Получить стартоввые карты
        /// </summary>
        /// <returns></returns>
        public List<AbstractCard> GetStarterCards()
        {
            List<AbstractCard> cards = new List<AbstractCard>();
            cards.Add(_abstractCards.FirstOrDefault(x => x.name == "BuildTowerCard"));
            cards.Add(_abstractCards.FirstOrDefault(x => x.name == "BuildWallCard"));
            return cards;
        }
        /// <summary>
        /// Выбрать карту
        /// </summary>
        public void SelectCard(string nameCard)
        {
            AbstractCard selectedCard = _abstractCards.FirstOrDefault(x => x.name == nameCard);
            if (selectedCard!=null)
            {
                selectedCard.ActivateCard();
                TryChooseNextCard();
            }
            else
            {
                Debug.LogWarning($"Среди доступных карт не было {nameCard}");
            }
        }
        /// <summary>
        /// Установить количество выбираемых карточек
        /// </summary>
        /// <param name="number"></param>
        public void SetNumberCardSelection(int number) => _numberOfSelection = number;

        
        /// <summary>
        /// Попытаться выбрать следующую карту
        /// </summary>
        public void TryChooseNextCard()
        {
            if (_numberOfSelection>0)
            {
                _numberOfSelection--;
                _uiCardController.ShowUIChooseCard();
                List<AbstractCard> cards = new List<AbstractCard>();
                
                if (FirstStage)
                {
                    cards = GetStarterCards();
                }
                else
                {
                    cards = GetRandomCards(2);
                }
                
                
                _uiCardController.ShowCards(cards);
            }
            else
            {
                _uiCardController.HideUIChooseCard();
            }
        }

        private void Start()
        {
            SetNumberCardSelection(5);
            TryChooseNextCard();
        }

        private void DoLogicOnStages(object sender, EventArgs args)
        {
            if (!GameHandler.Instance.IsFirstStageActive())
            {
                FirstStage = false;
                SetNumberCardSelection(0);
                TryChooseNextCard();
            }

        }
    }
}

