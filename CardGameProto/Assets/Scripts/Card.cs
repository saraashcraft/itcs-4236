using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public Sprite cardFace;
    public Sprite cardBack;
    private SpriteRenderer spriteRenderer;
    public GameObject cardPrefab;

    [SerializeField] private Transform cardTrans;
    [SerializeField] public GameObject card;

    public bool selected { get; set; }
    public bool faceUp { get; set; }
    public char suit { get; set; }
    public int value { get; set; }
    public int design { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        selected = false;
        faceUp = false;
        suit = '\0';
        value = 0;
        design = 0;
        spriteRenderer = card.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = cardBack;
        card = Instantiate(cardPrefab, new Vector3(transform.position.x, transform.position.y + 2.5f, transform.position.z + 1000), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (faceUp == true)
        {
            spriteRenderer.sprite = cardFace;
        }
        else
        {
            spriteRenderer.sprite = cardBack;
        }
    }

    public void OnMouseDown()
    {
        if (faceUp)
        {
            // If an unselected card in the hand has been selected, set boolean value and raise the card
            if (!selected)
            {
                selected = true;
                cardTrans.position = new Vector3(cardTrans.position.x, cardTrans.position.y + 1, cardTrans.position.z);
            }
        }
    }

    // Move card will move the card's location
    public void moveCard(Vector3 newLocation)
    {
        cardTrans.position = newLocation;
    }

    // Flip card will switch a card's sprite between face up and face down
    public void flipCard()
    {
        // If card is face up, then flip to display card back
        if (faceUp)
            spriteRenderer.sprite = cardBack;
        // If card is face down, then flip to display card face
        else
            spriteRenderer.sprite = cardFace;

        // Change faceUp to reflect flip
        faceUp = !faceUp;
    }
}
