using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSprite : MonoBehaviour
{
    public Sprite cardFace;
    public Sprite cardBack;

    private SpriteRenderer spriteRenderer;
    private Select select;
    private TriumphLogic triumphLogic;


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
        spriteRenderer = GetComponent<SpriteRenderer>();
        select = GetComponent<Select>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (select.faceUp == true)
        {
            spriteRenderer.sprite = cardFace;
        }
        else {
            spriteRenderer.sprite = cardBack; 
        }
    }
}
