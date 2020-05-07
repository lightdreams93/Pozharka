using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class OrderGame : MonoBehaviour
{
    [SerializeField] private OrderCard[] _orderCards;
    [SerializeField] private float _timer; 

    [SerializeField] private Text _timerText;

    [SerializeField] private GameObject _cardsContainer;
    [SerializeField] private GameObject _dragCard;
    [SerializeField] private GameObject _gamePanel;
    [SerializeField] private GameObject _cardPanel;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private Image _winImage;

    [SerializeField] private Healthbar _healthbar;

    [SerializeField] private TodoList _todoList;

    private bool _gameStart;
    private float _currentTime;

    public static event Action OnOrderGameWin;
    public static event Action OnOrderGameLose;

    private void Start()
    {
        _currentTime = _timer;
        _orderCards = MixCard(_orderCards);
        DisplayCards();

        DragCard.OnCardStartDrag += DragCard_OnCardStartDrag;
        DragCard.OnCardDrag += DragCard_OnCardDrag;
        DragCard.OnCardEndDrag += DragCard_OnCardEndDrag;
        DragCard.OnCardDrop += DragCard_OnCardDrop;

        OnOrderGameWin += OrderGame_OnOrderGameWin;
        OnOrderGameLose += OrderGame_OnOrderGameLose;

        Healthbar.OnVictumDie += Healthbar_OnVictumDie;
    }

    private void Healthbar_OnVictumDie()
    {
        EndGame();
    }

    private void OrderGame_OnOrderGameLose()
    {
        _healthbar.MultiplyPauseSpeed();
        ResetTimer();
        _orderCards = MixCard(_orderCards);
        DisplayCards();
    }

    private void Update()
    {
        if (_gameStart)
            StartTimer();
    }

    private void OrderGame_OnOrderGameWin()
    {
        _cardPanel.SetActive(false);
        EndGame();
        //_todoList.DoneTask();
        _winPanel.SetActive(true);
        _winImage.sprite = _orderCards[_orderCards.Length - 1].Sprite;
    }

    private void OnDestroy()
    {
        DragCard.OnCardStartDrag -= DragCard_OnCardStartDrag;
        DragCard.OnCardDrag -= DragCard_OnCardDrag;
        DragCard.OnCardEndDrag -= DragCard_OnCardEndDrag;
        DragCard.OnCardDrop -= DragCard_OnCardDrop;

        OnOrderGameWin -= OrderGame_OnOrderGameWin;
        OnOrderGameLose -= OrderGame_OnOrderGameLose;
    }

    private void DragCard_OnCardDrop(int drag, int drop)
    {  
        OrderCard tempCard = _orderCards[drag];

        _orderCards[drag] = _orderCards[drop];
        _orderCards[drop] = tempCard;

        DisplayCards();
    }

    private void DragCard_OnCardEndDrag(int dragIndex)
    {
        _dragCard.SetActive(false);
        _dragCard.GetComponent<CanvasGroup>().blocksRaycasts = true;
        _cardsContainer.transform.GetChild(dragIndex).GetChild(1).GetComponent<Image>().enabled = true;
    }

    private void DragCard_OnCardDrag(int dragIndex)
    {
        _dragCard.transform.position = Input.mousePosition;
    }

    private void DragCard_OnCardStartDrag(int dragIndex)
    {
        _dragCard.SetActive(true);
        _dragCard.GetComponent<CanvasGroup>().blocksRaycasts = false;
        _dragCard.transform.GetChild(0).GetComponent<Image>().sprite = _orderCards[dragIndex].Sprite;
        _cardsContainer.transform.GetChild(dragIndex).GetChild(1).GetComponent<Image>().enabled = false;
    }

    public void StartGame()
    {
        _gameStart = true;
        ShowGamePanel();
        _cardPanel.SetActive(true);
        _winPanel.SetActive(false);
    }

    public void EndGame()
    {
        _gameStart = false;
        _healthbar.ResumeSpeed();
    }

    private void StartTimer()
    {
        if (_currentTime > 0)
            _currentTime -= Time.deltaTime;
        else
        {
            _orderCards = MixCard(_orderCards);
            _currentTime = _timer;
            _healthbar.MultiplyPauseSpeed();
            DisplayCards();
        }
        DisplayTimer();

        _healthbar.PauseSpeed();
    }

    private void ResetTimer()
    {
        _currentTime = _timer;
        DisplayTimer();
    }

    private void DisplayTimer()
    {
        if (Mathf.Round(_currentTime) >= 10)
            _timerText.text = $"00:{Mathf.Round(_currentTime)}";
        else
            _timerText.text = $"00:0{Mathf.Round(_currentTime)}";
    }


    private OrderCard[] MixCard(OrderCard[] orderCards)
    {
        for (int i = 0; i < orderCards.Length; i++)
        {
            OrderCard currentCard = orderCards[i];
            int randomCardIndex = UnityEngine.Random.Range(i, orderCards.Length);
            orderCards[i] = orderCards[randomCardIndex];
            orderCards[randomCardIndex] = currentCard;
        }
        return orderCards;
    }


    private bool isGameWin()
    {
        for (int i = 0; i < _orderCards.Length; i++)
        {
            if (i != _orderCards[i].Order)
                return false;
        }

        return true;
    }

    public void GameResult()
    {
        if (isGameWin())
            OnOrderGameWin?.Invoke();
        else
            OnOrderGameLose?.Invoke();
    }

    private void HideGamePanel()
    {
        _gamePanel.SetActive(false);
    }

    private void ShowGamePanel()
    {
        _gamePanel.SetActive(true);
    }


    private void DisplayCards()
    {
        for (int i = 0; i < _orderCards.Length; i++)
        {
            Image cardIcon = _cardsContainer.transform.GetChild(i).GetChild(1).GetComponent<Image>();
            cardIcon.sprite = _orderCards[i].Sprite;
        }
    }
}

[System.Serializable]
public class OrderCard
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private int _order;

    public Sprite Sprite => _sprite;

    public int Order => _order;
}
