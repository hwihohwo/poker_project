using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Choice_panel : MonoBehaviour
{
    const int player_first_cards_number = 4;
    void Start()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {

    }

    public IEnumerator choice_card(List<int> player_hand_cards)
    {
        GameObject[] cards = new GameObject[player_first_cards_number];
        Vector3 position = new Vector3(-250, 0, 0);
        for (int i = 0; i < player_first_cards_number; i++)
        {
            cards[i] = GameObject.Find(PublicFunction.GetCardName(player_hand_cards[i]));
            position.x += 100;
            cards[i].transform.localPosition = position;
        }
        this.transform.Find("msg").GetComponent<TextMeshProUGUI>().text = "Choose card to discard";
        yield return null;
    }
}