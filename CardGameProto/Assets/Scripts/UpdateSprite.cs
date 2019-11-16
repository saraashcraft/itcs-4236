using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSprite : MonoBehaviour
{ 
    public Sprite cardFace;
    public Sprite cardBack;

    private SpriteRenderer spriteRenderer;

    private Select select;
	private OpponentSelect opponentSelect;

    private TriumphLogic triumphLogic;
    [SerializeField] private Transform cardTrans;
	[SerializeField] public GameObject card;

    private bool selected = false;
	public double cardValue; 

    // Start is called before the first frame update
    void Start()
    {
        List<string> deck = TriumphLogic.GenerateDeck();
        triumphLogic = FindObjectOfType<TriumphLogic>();

        int i = 0;
        foreach (string card in deck) {
            if (this.name == card) {
                cardFace = triumphLogic.cardFaces[i];
                break; 
            }
            i++; 
        }
        spriteRenderer = card.GetComponent<SpriteRenderer>();
        select = GetComponent<Select>();
		opponentSelect = GetComponent<OpponentSelect>(); 
    }

    // Update is called once per frame
    void Update()
    {
		if (select.faceUp == true)
		{
			spriteRenderer.sprite = cardFace;
		}
		else if (opponentSelect.faceUp == true) {
			spriteRenderer.sprite = cardFace;
		}
		else
		{
			spriteRenderer.sprite = cardBack;
		}
	}

    public void OnMouseDown()
    {
        if (select.faceUp)
        {
            // If an unselected card in the hand has been selected, set boolean value and raise the card
            // isSelectable makes sure there are only ever 2 cards raised
            if (!selected && triumphLogic.isSelectable())
            {
                selected = true;
                cardTrans.position = new Vector3(cardTrans.position.x, cardTrans.position.y + 1, cardTrans.position.z);
                cardValue = select.value;
                triumphLogic.setPlayerCards(card, cardValue);
            }
        }
        else
        {
            // If the card on the top of the deck has been selected and there are less than 4 cards in the hand, add it to the hand

        }
    }
}