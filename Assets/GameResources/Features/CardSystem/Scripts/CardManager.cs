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
        [Range(2, 6)] public int TimeLoopShow = default;
        
        private List<AbstractCard> _abstractCards = new List<AbstractCard>();
        private List<AbstractCard> _abstractSpecialCards = new List<AbstractCard>();
        private List<AbstractCard> _abstractNotSpecialCards = new List<AbstractCard>();
        
        private int _numberOfSelection = default;

        private bool _firstStage = true;

        private UiCardController _uiCardController = default;

        private Coroutine _coroutineShowCard = default;

        private void Awake()
        {
            _abstractCards = Resources.LoadAll<AbstractCard>("Cards").ToList();
            _abstractSpecialCards = _abstractCards.Where(x => x.Special == true).ToList();
            _abstractNotSpecialCards = _abstractCards.Where(x => x.Special == false).ToList();

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

        private IEnumerator ShowCardEveryTime(float time)
        {
            while (true)
            {
                if (_numberOfSelection < 1)
                {
                    SetNumberCardSelection(1);
                }
                TryChooseNextCard();
                
                yield return new WaitForSecondsRealtime(time-1);
                _uiCardController.HideUIChooseCard();
                yield return new WaitForSecondsRealtime(1f);
            }
            
        }

        /// <summary>
        /// Получить обычные карты в указаном количестве
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public List<AbstractCard> GetRandomCards(int number)
        {
            List<AbstractCard> cards = new List<AbstractCard>();
            for (int i = 0; i < number; i++)
            {
                cards.Add(_abstractNotSpecialCards[Random.Range(0,_abstractNotSpecialCards.Count)]);
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
                
                if (_firstStage)
                {
                    cards = GetStarterCards();
                }
                else
                {
                    cards = GetRandomCards(2);
                    // Добавление 1 карты с 15% шансом
                    if (Random.Range(0, 100)<15)
                    {
                        cards.Add(_abstractSpecialCards[Random.Range(0, _abstractSpecialCards.Count)]);
                    }
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
                _firstStage = false;
                if (_coroutineShowCard == null)
                {
                    SetNumberCardSelection(0);
                    _coroutineShowCard = StartCoroutine(ShowCardEveryTime(TimeLoopShow));
                }
                
            }

        }
    }
}

