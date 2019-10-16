using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriumphLogic : MonoBehaviour
{
    //TODO: make a Card object and a Card[] array s.t. you can get the 

    public Sprite[] cardFaces;
    public GameObject cardPrefab;
	public GameObject opponentPrefab; //for the opponent's cards

    public static string[] suits = new string[] { "C", "D", "H", "S" };
    public static string[] values = new string[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

    public List<string> deck;
	public List<string> opponentDeck;

	private GameObject[] playerCards = new GameObject[2]; //needs to be private - otherwise, initializes to 0 because unity has it as a public property set outside in the inspector
	private double[] playerCardValues = new double[2]; //parallel array that holds the values of the cards - couldn't access values. because the player choses their values, they only need an array of 2

	private GameObject[] opponentCards = new GameObject[4];
	private double[] opponentCardValues = new double[4];
	private double opponentBestValue;

	private double playerHandResult; 

    // Start is called before the first frame update
    void Start()
    {
        PlayCards();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		if (playerCards[0] != null && playerCards[1] != null)
		{
			playerHandResult = playerCardValues[0] + playerCardValues[1];
			print("Cards in play: " + playerCardValues[0] + " " + playerCardValues[1]);
		}
		else
		{
			print("Nothing set");
		}

	}

	int twoCardLimit = 0;

	public void setPlayerCards(GameObject card, double cardValue) {
		
		if (twoCardLimit < 2)
		{
			playerCards[twoCardLimit] = card;
			playerCardValues[twoCardLimit] = cardValue;

			twoCardLimit += 1;
		}
	}

	//int fourCardLimit = 0; 
	//public void setOpponentCards(GameObject card, double cardValue) {
	//	if (fourCardLimit < 4) {
	//		opponentCards[fourCardLimit] = card;
	//		opponentCardValues[fourCardLimit] = cardValue;

	//		fourCardLimit += 1; 
	//	}
	//}

	public bool isSelectable() {
		if (twoCardLimit >= 2)
		{
			return false;
		}
		else {
			return true; 
		}
	}

    /**
     * Main method where most functions will be called - creates/shuffles deck for both player and opponent
     */
    public void PlayCards() {
        deck = GenerateDeck();
		Shuffle(deck);
		opponentDeck = GenerateDeck(); 
		Shuffle(opponentDeck);

        DealCardsPlayer();
		DealCardsOpponent();

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

    void DealCardsPlayer() {
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

	void DealCardsOpponent() {
		//used to offset the dealt cards
		float xOffset = 2f;
		float yOffset = 10f;

		Vector3 originalPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
		int drawFour = 0; //if <4, put onto player's hand, otherwise, stay in deck.

		foreach (string card in opponentDeck)
		{
			//this makes it in a row.
			GameObject newCard;

			OpponentSelect opponentSelect = GetComponent<OpponentSelect>();

			if (drawFour < 4)
			{
				newCard = Instantiate(opponentPrefab, new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, transform.position.z + 1000), Quaternion.identity);
				newCard.name = card;

				newCard.GetComponent<OpponentSelect>().faceUp = true; //set to false for the opponent so the player can't see the cards. set to true to debug
				xOffset += 2f;

				opponentCards[drawFour] = newCard;
				//	opponentCardValues[drawFour] = opponentSelect.separateSuitAndValue(newCard.name[1]);

				opponentCardValues[drawFour] = returnCardValue(newCard.name[1]);
				///	print("card val: " + opponentCardValues[drawFour]);
				//	print(opponentCardValues[drawFour]);

				drawFour++;
			}
			else
			{
				//offsets used to get the deck to the upper right corner of the screen
				xOffset = 14f;
				yOffset = 11f;

				newCard = Instantiate(opponentPrefab, new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, transform.position.z + 1000), Quaternion.identity);
				newCard.name = card;

				newCard.GetComponent<OpponentSelect>().faceUp = false;

			}

			if (drawFour == 4) {
				print("CHECK?");
			}
		}
	}

	/*
        This shouldn't be used and 100% should be cut once I implement a Card Object w inherent values. Sorry, Walker. 
     */

	double returnCardValue(char value) {
		double tempValue;


		if (value == 'J')
		{
			tempValue = 11;
		}
		else if (value == 'Q')
		{
			tempValue = 12;
		}
		else if (value == 'K')
		{
			tempValue = 13;
		}
		else if (value == 'A')
		{
			tempValue = 1;
		}
		else
		{
			tempValue = char.GetNumericValue(value);
		}

		return tempValue;

	}
}
