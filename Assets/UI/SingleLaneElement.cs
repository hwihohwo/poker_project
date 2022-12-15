using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class SingleLaneElement
    {
        public string selectedCard;

        public void SetHand(List<int> player_hand_cards, Vector3 player_position)
        {
            Vector3 position = new Vector3(0, (player_position.y - 0) / (float)1.5, 0);
            int position_x = -300;
            int current_card_count = SingleLaneGame.cards.Count;

            for (int i = current_card_count - 1; i > current_card_count - 4; i--)
            {
                GameObject temp = GameObject.Find(PublicFunction.GetCardName(SingleLaneGame.cards[i]));
                if (temp != null)
                {
                    player_hand_cards.Add(SingleLaneGame.cards[i]);
                    SingleLaneGame.cards.RemoveAt(i);
                    position_x += 150;
                    position.x = position_x;
                    temp.transform.localPosition = position;
                    Debug.Log(temp.name + " moved to " + position.ToString());
                }
                else
                    Debug.Log("Cannot find " + PublicFunction.GetCardName(SingleLaneGame.cards[i]));
            }
        }
    }
