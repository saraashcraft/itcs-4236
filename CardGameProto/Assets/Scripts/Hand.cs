using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private GameObject[] cards = new GameObject[4];
    private Vector3[] cardSlots = new Vector3[4];
    public GameObject cardPrefab;
    public List<string> deck;
    public static string[] suits = new string[] { "C", "D", "H", "S" };
    public static string[] values = new string[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
    private int cardsPlayed = 0;

    // Start is called before the first frame update
    void Start()
    {
        GenerateDeck();
        Shuffle(deck);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Add cards to hand
    void DrawCards()
    {
        foreach (GameObject card in cards)
        {
            if(card == null)
            {
                GameObject newCard = Instantiate(cardPrefab, new Vector3(transform.position.x + xOffset, transform.position.y - yOffset,
                    transform.position.z + 1000), Quaternion.identity);
                newCard.name = card;

                newCard.GetComponent<Select>().faceUp = false;
            }
        }
    }

    // Play a card from the hand
    void PlayCard()
    {
        // Only two cards can be played
        if(cardsPlayed < 2)
        {
            // TODO: add code to play card

            cardsPlayed++;
        }
    }

    public static List<string> GenerateDeck()
    {
        List<string> newDeck = new List<string>();
        foreach (string s in suits)
        {
            foreach (string v in values)
            {
                newDeck.Add(s + v);
            }
        }

        return newDeck;
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
}
