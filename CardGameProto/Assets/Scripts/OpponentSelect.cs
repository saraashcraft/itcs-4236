using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentSelect : MonoBehaviour
{
	private OpponentSelect opSelect;

	public bool faceUp = false;

	public char suit; //have to separate them bc .name is really convenient for string comparison, but not getting a value for the card 
	public double value; //11 = J; 12 = Q, 13 = K, 1 = A - for the time being, Ace is 1 unless in a combo
						 //to access card value - opSelect.value; 

	// Start is called before the first frame update
	void Start()
	{
		opSelect = GetComponent<OpponentSelect>();

		separateSuitAndValue();

	}

	void separateSuitAndValue()
	{
		string tempName = opSelect.name;

		suit = tempName[0];

		if (tempName[1] == 'J')
		{
			value = 11;
		}
		else if (tempName[1] == 'Q')
		{
			value = 12;
		}
		else if (tempName[1] == 'K')
		{
			value = 13;
		}
		else if (tempName[1] == 'A')
		{
			value = 1;
		}
		else
		{
			value = char.GetNumericValue(tempName[1]);
		}

	}

	// Update is called once per frame
	void Update()
	{

	}
}
