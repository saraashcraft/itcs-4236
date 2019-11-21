using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriumphLogic : MonoBehaviour
{
	//TODO: make a Card object and a Card[] array s.t. you can get the 

	public Text score;

    public Sprite[] cardFaces;
    public GameObject cardPrefab;
	public GameObject opponentPrefab; //for the opponent's cards

    public static string[] suits = new string[] { "C", "D", "H", "S" };
    public static string[] values = new string[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

    public List<GameObject> hand = new List<GameObject>();
    public List<GameObject> cards = new List<GameObject>();
    public List<string> deck;
	public List<string> opponentDeck;

    List<GameObject> opponentDeckObjects = new List<GameObject>();
	private GameObject[] playerCards = new GameObject[2]; //needs to be private - otherwise, initializes to 0 because unity has it as a public property set outside in the inspector
	private double[] playerCardValues = new double[2]; //parallel array that holds the values of the cards - couldn't access values. because the player choses their values, they only need an array of 2

	private GameObject[] opponentCards = new GameObject[4];
	private double[] opponentCardValues = new double[4];
	private int firstBestOpponentCardPosition, secondBestOpponentCardPosition; //positions of the best cards in opponents current hand
	private double opponentBestValue;
    private double opponentTotalScore = 0;

	private double playerHandResult = -1; //initialized to -1 until it gets real values
    private double playerTotalScore = 0;

    private int cardsInPlayerHand = 0;
    private int cardsInOpponentHand = 0;

    private Vector3[] positions = new Vector3[4];
    private Vector3[] opponentPositions = new Vector3[4];

    // Start is called before the first frame update
    void Start()
    {
        PlayCards();
	}

    // Update is called once per frame
    void Update()
    {
        // Check for redeal
        if(cardsInPlayerHand < 4)
        {
            hand.Add(cards[cards.Count - 1]);
            cardsInPlayerHand++;
            cards.Remove(cards[cards.Count - 1]);
            cards.TrimExcess();

            // Rearrange cards
            int i = 0;
            foreach (GameObject card in hand) {
                card.transform.position = positions[i];
                card.GetComponent<Select>().faceUp = true;
                i++;
            }
        }

        // Check for opponent redeal
        if(cardsInOpponentHand < 4)
        {
            for (int i = 0; i < 4; i++)
            {
                if(opponentCards[i] == null)
                {
                    opponentCards[i] = opponentDeckObjects[opponentDeckObjects.Count - 1];
                    opponentDeckObjects.Remove(opponentDeckObjects[opponentDeckObjects.Count - 1]);
                }
            }
        }

        // Player picks cards
		if (playerCards[0] != null && playerCards[1] != null)
		{
			playerHandResult = playerCardValues[0] + playerCardValues[1];
			print("Cards in play: " + playerCardValues[0] + " " + playerCardValues[1]);
		}
		else
		{
			print("Nothing set");
		}

        // Opponent picks cards
		if (playerHandResult != -1)
		{
			opponentCards[firstBestOpponentCardPosition].GetComponent<OpponentSelect>().faceUp = true;
			opponentCards[secondBestOpponentCardPosition].GetComponent<OpponentSelect>().faceUp = true;
            opponentBestValue = opponentCardValues[firstBestOpponentCardPosition] + opponentCardValues[secondBestOpponentCardPosition];

            EndRound();
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
        //if <4, put onto player's hand, otherwise, stay in deck.


        foreach (string card in deck) {
            //this makes it in a row.
            GameObject newCard;


            if (cardsInPlayerHand < 4)
            {
                positions[cardsInPlayerHand] = new Vector3(transform.position.x + xOffset, transform.position.y - yOffset, transform.position.z + 1000);
                newCard = Instantiate(cardPrefab, positions[cardsInPlayerHand], Quaternion.identity);
                newCard.name = card;

                newCard.GetComponent<Select>().faceUp = true;
                xOffset += 2f;
                cardsInPlayerHand++;
                hand.Add(newCard);
            }
            else {
                xOffset = 0f;
                yOffset = -2.5f;

                newCard = Instantiate(cardPrefab, new Vector3(transform.position.x + xOffset, transform.position.y - yOffset, transform.position.z +1000), Quaternion.identity);
                newCard.name = card;


                newCard.GetComponent<Select>().faceUp = false;
                cards.Add(newCard);
            }
        }
    }

	void DealCardsOpponent() {
		//used to offset the dealt cards
		float xOffset = 2f;
		float yOffset = 10f;

		Vector3 originalPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
		//if <4, put onto player's hand, otherwise, stay in deck.

		foreach (string card in opponentDeck)
		{
			//this makes it in a row.
			GameObject newCard;

			OpponentSelect opponentSelect = GetComponent<OpponentSelect>();

			if (cardsInOpponentHand < 4)
			{
                opponentPositions[cardsInOpponentHand] = new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, transform.position.z + 1000);

                newCard = Instantiate(opponentPrefab, opponentPositions[cardsInOpponentHand], Quaternion.identity);
				newCard.name = card;

				newCard.GetComponent<OpponentSelect>().faceUp = false; //set to false for the opponent so the player can't see the cards. set to true to debug
				xOffset += 2f;

				opponentCards[cardsInOpponentHand] = newCard;
				//	opponentCardValues[drawFour] = opponentSelect.separateSuitAndValue(newCard.name[1]);

				opponentCardValues[cardsInOpponentHand] = returnCardValue(newCard.name);
					//print("card val: " + opponentCardValues[drawFour]);
				//	print(opponentCardValues[drawFour]);

				cardsInOpponentHand++;
			}
			else
			{
				//offsets used to get the deck to the upper right corner of the screen
				xOffset = 14f;
				yOffset = 11f;

				newCard = Instantiate(opponentPrefab, new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, transform.position.z + 1000), Quaternion.identity);
				newCard.name = card;

				newCard.GetComponent<OpponentSelect>().faceUp = false;

                opponentDeckObjects.Add(newCard);
			}

			if (cardsInOpponentHand == 4) {
				bestValue(); 
			}
		}
	}

	/*
    Short decision-making function to decide for which cards the AI will play
         */
	void bestValue() {
		double firstBestValue = opponentCardValues[0];
		double secondBestValue = -1;

		int firstValLocation = 0;
		int secondValLocation = 0 ;

		for (int i = 0; i < 4; i++) {
			if (opponentCardValues[i] > firstBestValue) {
				firstBestValue = opponentCardValues[i];
				firstValLocation = i; 
			}
		}

		for (int i = 0; i < 4; i++) {
			if (i != firstValLocation) {
				if (opponentCardValues[i] > secondBestValue) {
					secondBestValue = opponentCardValues[i];
					secondValLocation = i; 
				}
			}
		}

		//print("Highest card is at " + opponentCards[firstValLocation] + " and second highest is " + opponentCards[secondValLocation]);
		
		firstBestOpponentCardPosition = firstValLocation;
		secondBestOpponentCardPosition = secondValLocation; 
	}

	/*
        This shouldn't be used and 100% should be cut once I implement a Card Object w inherent values. Sorry, Walker. 
        Has to use parameter string because value[0] for 10 is '1'
         * */

	double returnCardValue(string value) {
		char charValue = value[1];
		double tempValue;


		if (charValue == 'J')
		{
			tempValue = 11;
		}
		else if (charValue == 'Q')
		{
			tempValue = 12;
		}
		else if (charValue == 'K')
		{
			tempValue = 13;
		}
		else if (charValue == 'A')
		{
			tempValue = 1;
		}
		else if ((value[1] == '1' )&& (value[2] == '0')) {
			tempValue = 10;
		}
		else
		{
			tempValue = char.GetNumericValue(charValue);
		}

		return tempValue;

	}

    // Called once cards are played to make calculations
    void EndRound()
    {
        if (playerHandResult > opponentBestValue)
        {
            // If player wins round, all played points go to them
            score.text = "Player: " + playerHandResult + "\nOpponent: " + opponentBestValue + "\nPlayer wins";
            playerTotalScore += playerHandResult + opponentBestValue;
        }
        else if (opponentBestValue > playerHandResult)
        {
            // If opponent wins round, all played points go to them
            score.text = "Player: " + playerHandResult + "\nOpponent: " + opponentBestValue + "\nOpponent wins";
            opponentTotalScore += playerHandResult + opponentBestValue;
        }
        else
        {
            // No points awarded in case of tie
            score.text = "Player: " + playerHandResult + "\nOpponent: " + opponentBestValue + "\nTie";
        }
        // Print current scores
        score.text += "\n\nPlayer Total: " + playerTotalScore + "\nOpponent Total: " + opponentTotalScore;

        // Check winning conditions
        // If player & opponent both meet winning conditions in same round,
        // compare scores
        if (opponentTotalScore > 200 && playerTotalScore > 200)
        {
            if (opponentTotalScore > playerTotalScore)
                score.text = "Opponent triumphs!";
            else if (opponentTotalScore < playerTotalScore)
                score.text = "Player triumphs!";
            else
                score.text = "Tie!";
        }
        else if (opponentTotalScore > 200)
            score.text = "Opponent triumphs!";
        else if (playerTotalScore > 200)
            score.text = "Player triumphs!";

        // Reset for next round
        cardsInPlayerHand--;
        cardsInPlayerHand--;
        hand.Remove(playerCards[0]);
        hand.Remove(playerCards[1]);
        hand.TrimExcess();
        Destroy(playerCards[0]);
        Destroy(playerCards[1]);
        cardsInOpponentHand--;
        cardsInOpponentHand--;
        Destroy(opponentCards[firstBestOpponentCardPosition]);
        Destroy(opponentCards[secondBestOpponentCardPosition]);
        playerHandResult = -1;
        opponentBestValue = -1;
        twoCardLimit = 0;
    }
}
