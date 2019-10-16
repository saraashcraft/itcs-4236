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

		value = separateSuitAndValue(opSelect.name[1]);

	}

	public double separateSuitAndValue(char value)
	{
		string tempName = opSelect.name;
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
			tempValue = char.GetNumericValue(tempName[1]);
		}

		return tempValue;
	}

	// Update is called once per frame
	void Update()
	{

	}
}
