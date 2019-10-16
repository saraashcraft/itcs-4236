using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriumphLogic : MonoBehaviour
{
    public Sprite[] cardFaces;
    public GameObject cardPrefab;

    public static string[] suits = new string[] { "C", "D", "H", "S" };
    public static string[] values = new string[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

    public List<string> deck;

	private GameObject[] playerCards = new GameObject[2]; //needs to be private - otherwise, initializes to 0 because unity has it as a public property set outside in the inspector

    // Start is called before the first frame update
    void Start()
    {
        PlayCards();
    }

    // Update is called once per frame
    void Update()
    {

    }

	int twoCardLimit = 0;

	public void setPlayerCards(GameObject card) {
		
		if (twoCardLimit < 2)
		{
			playerCards[twoCardLimit] = card;
			twoCardLimit += 1;
		}
	}

	public bool isSelectable() {
		if (twoCardLimit >= 2)
		{
			return false;
		}
		else {
			return true; 
		}
	}

    public void PlayCards() {
        deck = GenerateDeck();
        Shuffle(deck);

        DealCards();
    }

    public static List<string> GenerateDeck() {
        List<string> newDeck = new List<string>();
        foreach (string s in suits) {
            foreach (string v in values) {
                newDeck.Add(s + v);
            }
        }

        return newDeck;
    }

    void Shuffle<T>(List<T> list) {
        System.Random random = new System.Random();
        int n = list.Count;
        while (n > 1) {
            int k = random.Next(n);
            n--;

            T temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
    }

    void DealCards() {
        //used to offset the dealt cards
        float xOffset = 6f;
        float yOffset = -2.5f;

        Vector3 originalPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        int drawFour = 0; //if <4, put onto player's hand, otherwise, stay in deck.


        foreach (string card in deck) {
            //this makes it in a row.
            GameObject newCard;


            if (drawFour < 4)
            {
                newCard = Instantiate(cardPrefab, new Vector3(transform.position.x + xOffset, transform.position.y - yOffset, transform.position.z + 1000), Quaternion.identity);
                newCard.name = card;

                newCard.GetComponent<Select>().faceUp = true;
                xOffset += 2f;
                drawFour++;
            }
            else {
                xOffset = 0f;
                yOffset = -2.5f;

                newCard = Instantiate(cardPrefab, new Vector3(transform.position.x + xOffset, transform.position.y - yOffset, transform.position.z +1000), Quaternion.identity);
                newCard.name = card;


                newCard.GetComponent<Select>().faceUp = false;
            }
        }
    }
}
