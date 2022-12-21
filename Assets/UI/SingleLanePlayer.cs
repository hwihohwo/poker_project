using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SingleLanePlayer : MonoBehaviour
{
    SingleLaneElement singleLaneElement;
    public GameObject panel;
    public bool turn_over = false;
    public int score = 0;
    public List<int> player_hand_cards = new List<int>(Constants.PLAYER_MAX_CARD_NUMBER + 2);
    public List<int> player_open_hand_cards = new List<int>();

    private void Awake()
    {
        singleLaneElement = new SingleLaneElement();
    }
    void Start()
    {

    }
    void Update()
    {

    }

    public void ClickConfirm()
    {
        GameObject temp1 = null;
        GameObject temp2 = null;
        Vector3 position = Vector3.zero;

        if (player_hand_cards.Count < Constants.PLAYER_MAX_CARD_NUMBER)
        {
            temp2 = GameObject.Find(PublicFunction.GetCardName(SingleLaneGame.cards[SingleLaneGame.cards.Count - 1]));
            Image image = temp2.GetComponent<Image>();
            if (temp2 != null)
            {
                //foreach (int card in player_hand_cards)
                //{
                //    temp1 = GameObject.Find(PublicFunction.GetCardName(card));
                //    if (temp1 != null)
                //    {
                //        position = temp1.transform.localPosition;
                //        //position.x -= 75;
                //        temp1.transform.localPosition = position;
                //        //Debug.Log(temp1.name.ToString() + " moved to " + temp1.transform.localPosition);
                //    }
                //    else
                //        Debug.Log("Cannot find " + PublicFunction.GetCardName(card));
                //}
                temp1 = GameObject.Find(PublicFunction.GetCardName(player_hand_cards[player_hand_cards.Count() - 1]));
                position = temp1.transform.localPosition;
                position.x += 50;
                temp2.transform.localPosition = position;
                player_hand_cards.Add(SingleLaneGame.cards[SingleLaneGame.cards.Count - 1]);
                image.sprite = Resources.Load(Constants.card_image_path + temp2.name.ToString(), typeof(Sprite)) as Sprite;
                Debug.Log(temp2.name.ToString() + " moved to " + temp2.transform.localPosition);
                SingleLaneGame.cards.RemoveAt(SingleLaneGame.cards.Count - 1);
            }
            else
                Debug.Log("Cannot find " + PublicFunction.GetCardName(SingleLaneGame.cards[SingleLaneGame.cards.Count - 1]));
        }
        else
        {
            Debug.Log(this.name.ToString() + " score is " + CheckScore(player_hand_cards));
            turn_over = true; // 임시로 confirm한번더 누르면 점수 계산 후 종료하도록 설정
        }
    }

    public void SetHand(List<int> cards)
    {
        singleLaneElement.SetHand(player_hand_cards, this.transform.localPosition);
    }

    public int CheckScore(List<int> cards_to_be_scored)
    {
        score = singleLaneElement.CheckScore(cards_to_be_scored);
        Debug.Log(this.name.ToString() + " " + score.ToString());
        return (score);
    }

    public void CardChoicePop()
    {
        this.panel = Instantiate(this.panel, );
    }

}