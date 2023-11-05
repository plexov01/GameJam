using UnityEngine.Serialization;

namespace GameJam.Features.CardSystem
{
    using GameJam.Features.CardSystem;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    /// <summary>
    /// Менеджер карт
    /// </summary>
    public class CardManager : MonoBehaviour
    {
        [FormerlySerializedAs("TimeLoopShow")] [Range(2, 6)] public int cardTimerMax = default;
        
        private List<AbstractCard> _abstractCards = new List<AbstractCard>();
        private List<AbstractCard> _abstractSpecialCards = new List<AbstractCard>();
        private List<AbstractCard> _abstractNotSpecialCards = new List<AbstractCard>();
        
        private int _numberOfSelection = default;

        private bool isRandomlyChoosing = false;

        private UiCardController _uiCardController = default;

        private Coroutine _coroutineShowCard = default;


        private float cardTimer = 0f;
        private bool isGamePlaying = false;

        private void Awake()
        {
            _abstractCards = Resources.LoadAll<AbstractCard>("Cards").ToList();
            _abstractSpecialCards = _abstractCards.Where(x => x.Special == true).ToList();
            _abstractNotSpecialCards = _abstractCards.Where(x => x.Special == false).ToList();

            _uiCardController = FindObjectOfType<UiCardController>(true);
        }

        private void Update()
        {
            if (isRandomlyChoosing)
            {
                cardTimer += Time.deltaTime;
                if (cardTimer >= cardTimerMax)
                {
                    cardTimer = 0f;

                    List<AbstractCard> tempList = new List<AbstractCard>();
                    tempList.Add(GetRandomCommonCard());
                    tempList.Add(GetRandomCommonCard());

                    int number = UnityEngine.Random.Range(0, 100);
                    Debug.Log("Random number is "+number);
                    
                    if (number < 15)
                    {
                        tempList.Add(GetRandomRareCard());
                    }
                    
                    _uiCardController.ShowCards(tempList);
                    _uiCardController.ShowUIChooseCard();
                }
            }
        }

        private void Start()
        {
            _uiCardController.ShowCards(GetStarterCards());
            _uiCardController.ShowUIChooseCard();
        }

        private void OnEnable()
        {
            GameHandler.OnStateChanged += GameHandler_OnStageChanged;
        }

        private void OnDisable()
        {
            GameHandler.OnStateChanged -= GameHandler_OnStageChanged;
        }

        // private IEnumerator ShowCardEveryTime(float time)
        // {
        //     while (true)
        //     {
        //         if (_numberOfSelection < 1)
        //         {
        //             SetNumberCardSelection(1);
        //         }
        //         ChooseCards();
        //         
        //         yield return new WaitForSecondsRealtime(time-1);
        //         _uiCardController.HideUIChooseCard();
        //         yield return new WaitForSecondsRealtime(1f);
        //     }
        //     
        // }

        /// <summary>
        /// Получить обычные карты в указаном количестве
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        // public List<AbstractCard> GetRandomCards(int number)
        // {
        //     System.Random rnd = new System.Random(System.DateTime.Now.Millisecond + System.DateTime.Now.Second);
        //     List<AbstractCard> cards = new List<AbstractCard>();
        //     for (int i = 0; i < number; i++)
        //     {
        //         cards.Add(_abstractNotSpecialCards[rnd.Next(0,_abstractNotSpecialCards.Count)]);
        //     }
        //     
        //     return cards;
        // }
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
        private AbstractCard GetRandomCommonCard()
        {
            return _abstractNotSpecialCards[UnityEngine.Random.Range(0, _abstractNotSpecialCards.Count())];
        }
        
        private AbstractCard GetRandomRareCard()
        {
            return _abstractSpecialCards[UnityEngine.Random.Range(0, _abstractSpecialCards.Count())];
        }

        public void PlayCard(string cardName)
        {
            AbstractCard selectedCard = _abstractCards.FirstOrDefault(x => x.name == cardName);
            if (selectedCard!=null)
            {
                selectedCard.ActivateCard();
                _uiCardController.HideUIChooseCard();
            }
            else
            {
                Debug.LogWarning($"Среди доступных карт не было {cardName}");
            }
        }
        
        
        // public void GetRandomCommonCard1(string nameCard)
        // {
        //     AbstractCard selectedCard = _abstractCards.FirstOrDefault(x => x.name == nameCard);
        //     if (selectedCard!=null)
        //     {
        //         selectedCard.ActivateCard();
        //         ChooseCards();
        //     }
        //     else
        //     {
        //         Debug.LogWarning($"Среди доступных карт не было {nameCard}");
        //     }
        // }
        

     
        // public void ChooseCards(List<AbstractCard> cardPool, int numberOfCardsToGet)
        // {
        //     _uiCardController.ShowUIChooseCard();
        //     List<AbstractCard> cards = new List<AbstractCard>();
        //     
        //     if (_numberOfSelection>0)
        //     {
        //         if (isRandomlyChoosing)
        //         {
        //             cards = GetStarterCards();
        //         }
        //         else
        //         {
        //             cards = GetRandomCards(2);
        //             
        //             // Добавление 1 карты с 15% шансом
        //             System.Random rnd = new System.Random(System.DateTime.Now.Millisecond + System.DateTime.Now.Second);
        //
        //             if (rnd.Next(0,100) < 15)
        //             {
        //                 cards.Add(_abstractSpecialCards[rnd.Next(0, _abstractSpecialCards.Count)]);
        //             }
        //         }
        //         
        //         
        //         _uiCardController.ShowCards(cards);
        //     }
        //     else
        //     {
        //         _uiCardController.HideUIChooseCard();
        //     }
        // }
        

        private void GameHandler_OnStageChanged(object sender, EventArgs args)
        {
            if (GameHandler.Instance.IsSecondStageActive())
            {
                isRandomlyChoosing = true;
            }

        }
    }
}

