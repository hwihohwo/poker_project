using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

static class Constants
{
    public const int CARD_NUMBER = 52;
    public const int PLAYER_MAX_CARD_NUMBER = 5;
    public const int STARTING_CARD_NUMBER = 3;
    public enum score
    {
        RSF = 48000000,
        BSF = 44000000,
        STF = 40000000,
        FCD = 36000000,
        FH =  32000000,
        FLS = 28000000,
        MTN = 24000000,
        BST = 20000000,
        ST =  16000000,
        TRI = 12000000,
        TWO = 8000000,
        ONE = 4000000,
        NOP = 0
    };
}

static class boolean
{
    public const int TRUE = 1;
    public const int FALSE = 0;
}

static class PublicFunction
{
    public static string GetCardName(int cards_element)
    {
        int shape = cards_element / 13;
        int number = cards_element % 13 + 1;
        string ret = null;

        if (shape == 0)
            ret = "Club";
        else if (shape == 1)
            ret = "Heart";
        else if (shape == 2)
            ret = "Diamond";
        else if (shape == 3)
            ret = "Spade";

        if (number >= 1 && number <= 9)
            ret += "0" + number.ToString();
        else if (number == 10)
            ret += "10";
        else if (number == 11)
            ret += "11";
        else if (number == 12)
            ret += "12";
        else if (number == 13)
            ret += "13";
        return (ret);
    }

    public static void OnApplicationQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
    }
}

public class SingleLaneGame : MonoBehaviour
{
    public GameObject card;
    public GameObject canvas;
    public SingleLanePlayer singleLanePlayer1;
    public SingleLanePlayer singleLanePlayer2;
    public static List<int> cards = new List<int>(Constants.CARD_NUMBER + 2);

    void Start()
    {
        SetCard_(card, canvas);
        singleLanePlayer1.SetHand(cards);
        singleLanePlayer2.SetHand(cards);
    }

    void Update()
    {
        if (singleLanePlayer1.turn_over == true && singleLanePlayer2.turn_over == true)
            PublicFunction.OnApplicationQuit();
    }
    private void SetCard_(GameObject card, GameObject canvas)
    {
        System.Random randomobj = new();
        int randomValue;

        for (int i = 0; i < Constants.CARD_NUMBER; i++)
        {
            do
            {
                randomValue = randomobj.Next(Constants.CARD_NUMBER);
            } while (cards.Contains(randomValue));
            cards.Add(randomValue);
        }
        for (int i = 0; i < cards.Count; i++)
        {
            GameObject temp = Instantiate(card, canvas.transform);
            temp.transform.localPosition = new Vector3((float)i / 5, (float)i / 5, 0);
            //temp.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = PublicFunction.GetCardName(cards[i]);
            temp.name = PublicFunction.GetCardName(cards[i]);
            temp.GetComponent<Image>().sprite = Resources.Load("Sprite/PlayingCards/BackColor_Black", typeof(Sprite)) as Sprite;
        }
    }
}
