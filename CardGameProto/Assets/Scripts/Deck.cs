using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    List<string> cards = new List<string>();
    public static string[] suits = new string[] { "C", "D", "H", "S" };
    public static string[] values = new string[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

    // Start is called before the first frame update
    void Start()
    {
        // Generate deck
        foreach (string s in suits)
        {
            foreach (string v in values)
            {
                cards.Add(s + v);
            }
        }
        // Assign sprites
        int i = 0;
        foreach (string card in deck)
        {
            if (this.name == card)
            {
                cardFace = triumphLogic.cardFaces[i];
                break;
            }
            i++;
        }

        // Shuffle deck
        Shuffle<string>(cards);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Shuffle<T>(List<T> list)
    {
        System.Random random = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            int k = random.Next(n);
            n--;

            T temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
    }

    public Card dealCard()
    {
        Card newCard = new Card();

        // Give new card its suit and value
        string temp = cards[cards.Count - 1];
        newCard.suit = temp[0];
        if (temp[1] == 'K')
            newCard.value = 13;
        else if (temp[1] == 'Q')
            newCard.value = 12;
        else if (temp[1] == 'J')
            newCard.value = 11;
        else if (temp[1] == 'A')
            newCard.value = 1;
        else if (temp[1] == '1')
            newCard.value = 10;
        else
            newCard.value = (int)char.GetNumericValue(temp[1]);



        return newCard;
    }
}
