using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.U2D.Common;
using UnityEngine;
using UnityEngine.UI;

public class SingleLaneElement
    {
        public int score = 0;
        public void SetHand(List<int> player_hand_cards, Vector3 player_position)
        {
            Vector3 position = new Vector3(0, (player_position.y - 0) / (float)1.5, 0);
            int position_x = -450;

            for (int i = 0; i < 4; i++)
            {
                GameObject temp;
                temp = GameObject.Find(PublicFunction.GetCardName(SingleLaneGame.cards[i]));
                if (temp != null)
                {
                    Image image = temp.GetComponent<Image>();
                    player_hand_cards.Add(SingleLaneGame.cards[i]);
                    SingleLaneGame.cards.RemoveAt(i);
                    position_x += 50;
                    position.x = position_x;
                    temp.transform.localPosition = position;
                    image.sprite = Resources.Load(Constants.card_image_path + temp.name.ToString(), typeof(Sprite)) as Sprite;
                    Debug.Log(temp.name + " moved to " + position.ToString());
                }
                else
                    Debug.Log("Cannot find " + PublicFunction.GetCardName(SingleLaneGame.cards[i]));
            }
        }

        public int CheckScore(List<int> cards_to_be_scored)
        {
            int[,] total_cards = new int[4, 13];
            int top_card = FindTopCard_(cards_to_be_scored);
            int top_card_number = top_card % 13;
            if (top_card_number == 0)
                top_card_number += 13;
            int top_card_shape = top_card / 13;
            int spade_number, dia_number, heart_number, clover_number;

            foreach (int card in cards_to_be_scored)
                total_cards[card / 13, card % 13] = 1;
            spade_number = RowSum_(total_cards, 3);
            dia_number = RowSum_(total_cards, 2);
            heart_number = RowSum_(total_cards, 1);
            clover_number = RowSum_(total_cards, 0);

            #region 로스플, 백스플, 스티플, 플러쉬
            if (spade_number == 5)
            {
                if (StraightFlushCheck_(total_cards, 3, 9) == boolean.TRUE)
                    score = (int)Constants.score.RSF + 3;
                else if (StraightFlushCheck_(total_cards, 3, 0) == boolean.TRUE)
                    score = (int)Constants.score.BSF + 3;
                for (int i = 0; i < 9; i++)
                {
                    if (StraightFlushCheck_(total_cards, 3, i) == boolean.TRUE)
                    {
                        score = (int)Constants.score.STF + 3;
                        score += top_card_number * 1000;
                        break;
                    }
                }
                if (score == 0)
                {
                    score = (int)Constants.score.FLS + 3;
                    score += top_card_number * 1000;
                }
                return (score);
            }
            else if (dia_number == 5)
            {
                if (StraightFlushCheck_(total_cards, 2, 9) == boolean.TRUE)
                    score = (int)Constants.score.RSF + 2;
                else if (StraightFlushCheck_(total_cards, 2, 0) == boolean.TRUE)
                    score = (int)Constants.score.BSF + 2;
                for (int i = 0; i < 9; i++)
                {
                    if (StraightFlushCheck_(total_cards, 2, i) == boolean.TRUE)
                    {
                        score = (int)Constants.score.STF;
                        score += top_card_number * 1000;
                        break;
                    }
                }
                if (score == 0)
                {
                    score = (int)Constants.score.FLS + 2;
                    score += top_card_number * 1000;
                }
                return (score);
            }
            else if (heart_number == 5)
            {
                if (StraightFlushCheck_(total_cards, 1, 9) == boolean.TRUE)
                    score = (int)Constants.score.RSF + 1;
                else if (StraightFlushCheck_(total_cards, 1, 0) == boolean.TRUE)
                    score = (int)Constants.score.BSF + 1;
                for (int i = 0; i < 9; i++)
                {
                    if (StraightFlushCheck_(total_cards, 1, i) == boolean.TRUE)
                    {
                        score = (int)Constants.score.STF;
                        score += top_card_number * 1000;
                        break;
                    }
                }
                if (score == 0)
                {
                    score = (int)Constants.score.FLS + 1;
                    score += top_card_number * 1000;
                }
                return (score);
            }
            else if (clover_number == 5)
            {
                if (StraightFlushCheck_(total_cards, 0, 9) == boolean.TRUE)
                    score = (int)Constants.score.RSF;
                else if (StraightFlushCheck_(total_cards, 0, 0) == boolean.TRUE)
                    score = (int)Constants.score.BSF;
                for (int i = 0; i < 9; i++)
                {
                    if (StraightFlushCheck_(total_cards, 0, i) == boolean.TRUE)
                    {
                        score = (int)Constants.score.STF;
                        score += top_card_number * 1000;
                        break;
                    }
                }
                if (score == 0)
                {
                    score = (int)Constants.score.FLS;
                    score += top_card_number * 1000;
                }
                return (score);
            }
            #endregion

            #region 포카드
            for (int i = 0; i < 13; i++)
            {
                if (ColumnSum_(total_cards, i) == 4)
                {
                    if (i == 0)
                        i += 13;
                    score = (int)Constants.score.FCD + i;
                    return (score);
                }
            }
            #endregion

            #region 풀하우스
            for (int i = 0; i < 13; i++)
            {
                if (ColumnSum_(total_cards, i) == 3)
                {
                    int three_card_number = i;
                    if (three_card_number == 0)
                        three_card_number += 13;
                    for (int j = 0; j < 13; j++)
                    {
                        if (ColumnSum_(total_cards, j) == 2)
                        {
                            score = (int)Constants.score.FH + three_card_number;
                            return (score);
                        }
                    }
                }
            }
            #endregion

            #region 마운틴, 백스트레이트, 스트레이트
            for (int i = 0; i <= 9; i++)
            {
                int temp = StraightCheck_(total_cards, i);
                if (temp > score)
                {
                    score = temp;
                    if (score == (int)Constants.score.ST)
                        score += top_card_number * 1000;
                    score += top_card_shape;
                }
            }
            if (score > 0)
                return (score);
            #endregion

            #region 트리플
            for (int i = 0; i < 13; i++)
            {
                if (ColumnSum_(total_cards, i) == 3)
                {
                    if (i == 0)
                        i += 13;
                    score = (int)Constants.score.TRI + i;
                    return (score);
                }
            }
            #endregion

            #region 투 페어
            for (int i = 0; i < 12; i++)
            {
                int pair_card_number_1, pair_card_number_2, higher_pair, lower_pair, one_card = 0;
                int pair_card_shape;

                FindTopShapeInPair_(total_cards, out pair_card_shape, cards_to_be_scored);
                if (ColumnSum_(total_cards, i) == 2)
                {
                    pair_card_number_1 = i;
                    if (pair_card_number_1 == 0)
                        pair_card_number_1 += 13;
                    for (int j = i + 1; j < 13; j++)
                    {
                        if (ColumnSum_(total_cards, j) == 2)
                        {
                            pair_card_number_2 = j;
                            higher_pair = (pair_card_number_1 > pair_card_number_2) ? pair_card_number_1 : pair_card_number_2;
                            lower_pair = (pair_card_number_1 < pair_card_number_2) ? pair_card_number_1 : pair_card_number_2;
                            for (int k = 0; k < 13; k++)
                            {
                                if (ColumnSum_(total_cards, k) == 1)
                                {
                                    one_card = k;
                                    if (one_card == 0)
                                        one_card += 13;
                                }
                            }
                            score = (int)(Constants.score.TWO) + (higher_pair * 20000) + (lower_pair * 1000) + (one_card * 10) + pair_card_shape;
                            return (score);
                        }
                    }
                }
            }
            #endregion

            #region 원 페어
            int[] withoutpair;
            int pair_shape;
            FindTopShapeInPair_(total_cards, out pair_shape, cards_to_be_scored);
            CardsWithOutPair(total_cards, out withoutpair, cards_to_be_scored);
            // 페어 제외 카드 3장의 number를 배열에 넣고 내림차순 정렬.
            for (int i = 0; i < 13; i++)
            {
                if (ColumnSum_(total_cards, i) == 2)
                {
                    if (i == 0)
                        i += 13;
                    score = (int)Constants.score.ONE + (i * 300000) + (withoutpair[0] * 20000) + (withoutpair[1] * 1000) + (withoutpair[2] * 10) + pair_shape;
                    return (score);
                }
            }
            #endregion

            #region 하이카드
            score = (int)Constants.score.NOP + (withoutpair[0] * (int)Mathf.Pow(2, 10)) + (withoutpair[1] * (int)Mathf.Pow(2, 8)) +
                    (withoutpair[2] * (int)Mathf.Pow(2, 6)) + (withoutpair[3] * (int)Mathf.Pow(2, 4)) + (withoutpair[4] * (int)Mathf.Pow(2, 2)) + top_card_shape;
            return (score);
            #endregion
        }

        private int RowSum_(int[,] array, int row)
        {
            int sum = 0;

            for (int i = 0; i < 13; i++)
            {
                sum += array[row, i];
            }
            return (sum);
        }

        private int ColumnSum_(int[,] array, int column)
        {
            int sum = 0;

            for (int i = 0; i < 4; i++)
            {
                sum += array[i, column];
            }
            return (sum);
        }

        private int StraightFlushCheck_(int[,] array, int row, int num)
        {
            if (num > 9)
                return (boolean.FALSE);
            else if (num == 9)
            {
                if (array[row, num] == 1 && array[row, num + 1] == 1 &&
                array[row, num + 2] == 1 && array[row, num + 3] == 1 &&
                array[row, 0] == 1)
                    return (boolean.TRUE);
                else
                    return (boolean.FALSE);
            }
            if (array[row, num] == 1 && array[row, num + 1] == 1 &&
                array[row, num + 2] == 1 && array[row, num + 3] == 1 &&
                array[row, num + 4] == 1)
                return (boolean.TRUE);
            else
                return (boolean.FALSE);
        }

        private int StraightCheck_(int[,] array, int num)
        {
            if (num > 9)
                return (0);
            else if (num == 9)
            {
                if (ColumnSum_(array, 0) == 1 && ColumnSum_(array, num) == 1
                    && ColumnSum_(array, num + 1) == 1 && ColumnSum_(array, num + 2) == 1
                    && ColumnSum_(array, num + 3) == 1)
                    return ((int)Constants.score.MTN);
                else
                    return (0);
            }
            if (ColumnSum_(array, num + 4) == 1 && ColumnSum_(array, num) == 1
                && ColumnSum_(array, num + 1) == 1 && ColumnSum_(array, num + 2) == 1
                && ColumnSum_(array, num + 3) == 1)
            {
                if (num == 0)
                    return ((int)Constants.score.BST);
                else
                    return ((int)Constants.score.ST);
            }
            else
                return (boolean.FALSE);
        }

        private int FindTopCard_(List<int> cards_to_be_scored)
        {
            int top_card;
            int top_card_index = 0;

            top_card = cards_to_be_scored[0] % 13;
            if (top_card == 0)
                top_card += 13;
            for (int i = 1; i < cards_to_be_scored.Count(); i++)
            {
                int temp = cards_to_be_scored[i] % 13;
                if (temp == 0)
                    temp += 13;
                if (top_card < temp)
                {
                    top_card = temp;
                    top_card_index = i;
                }
            }
            return (cards_to_be_scored[top_card_index]);
        }

        private void FindTopShapeInPair_(int[,] array, out int shape1, List<int> cards_to_be_scored)
        {
            int temp;
            shape1 = 0;

            shape1 = cards_to_be_scored[0] / 13;
            for (int i = 0; i < cards_to_be_scored.Count(); i++)
            {
                temp = cards_to_be_scored[i] / 13;
                if (temp > shape1)
                {
                    if (ColumnSum_(array, temp) == 2)
                    {
                        shape1 = temp;
                    }
                }
            }
        }

        private void CardsWithOutPair(int[,] array, out int[] withoutpair, List<int> cards_to_be_scored)
        {
            int temp;
            withoutpair = new int[5] { -1, -1, -1, -1, -1 };
            for (int i = 0; i < cards_to_be_scored.Count(); i++)
            {
                temp = cards_to_be_scored[i] % 13;
                if (ColumnSum_(array, temp) == 2)
                    continue;
                withoutpair[i] = temp;
                if (withoutpair[i] == 0)
                    withoutpair[i] += 13;
            }
            withoutpair.Reverse();
        }
}
