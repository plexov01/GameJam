using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SingleCardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler 
{
    
    [SerializeField] private Image cardImage;
    [SerializeField] private Image selectedCardImage;

    private Transform cardsHandler;

    public void SetCardImage(Sprite sprite) {
        cardImage.sprite = sprite;
        selectedCardImage.sprite = sprite;

        cardsHandler = transform.parent.parent;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        selectedCardImage.gameObject.SetActive(true);
        selectedCardImage.transform.SetParent(cardsHandler);
    }

    public void OnPointerExit(PointerEventData eventData) {
        selectedCardImage.gameObject.SetActive(false);
        selectedCardImage.transform.SetParent(transform);
    }

    private void OnDisable() {
        selectedCardImage.gameObject.SetActive(false);
    }
}
