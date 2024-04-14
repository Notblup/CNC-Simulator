using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class GCodeStep{
    
    public int G;
    public int M;
   public float X;
    public float Y;
    public float Z;
    public float I;
    public float J;
    public float F;
    public float P;

    public GCodeStep()
    {
        G = 99999;
        M = 99999;
        X = 0;
        Y = 0;
        Z = 0;
        I = 99999;
        J = 99999;
        F = 99999;
    }

    char checkLetter(char temp, ref bool waitingForLetter)
    {
        if(temp != ' ')
        {
            waitingForLetter = false;
            return temp;
        }
        else
        {
            return '\0';
        }        
    }

    void setVariable(char asssignedLetter, char[] arrayForNumber, int lengthOfArray)
    {
        float temp = 0;
        bool decimalFound = false;
        int j = 1;

        for(int i = 0; i < lengthOfArray; ++i)
        {
            if(arrayForNumber[i] == '.')
            {
                decimalFound = true;
            }
            else if(!decimalFound)
            {                
                temp = temp * 10 + arrayForNumber[i] - 48;;
            }
            else if(decimalFound)
            {
                temp = temp + (arrayForNumber[i] - 48)/Mathf.Pow(10,j);
                ++j;
            }
        }

        switch(asssignedLetter)
        {
            case 'G':
                G = (int)temp;break;
            case 'M':
                M = (int)temp;break;
            case 'X':
                X = (float)temp;break;
            case 'Y':
                Y = (float)temp;break;
            case 'Z':
                Z = (float)temp;break;
            case 'I':
                I = temp;break;
            case 'J':
                J = temp;break;
            case 'F':
                F = temp;break;
        }

        
    }

    void parseNumber(char temp, char[] arrayForNumber, ref int numberArrayPointer, ref bool waitingForLetter, char assignedLetter)
    {
        if(temp == ' ' || temp == '\0')
        {
            waitingForLetter = true;
            setVariable(assignedLetter, arrayForNumber, numberArrayPointer);//numberArraypointer sent will be 1 more than actual length of string
            numberArrayPointer = 0;
            
        }
        else
        {
            arrayForNumber[numberArrayPointer] = temp;
            ++numberArrayPointer;
        }
    }

    public void parseLine(string lineToBeParsed)
    {
        char temp;
        char assignedLetter = 'a';
        bool waitingForLetter = true;
        int i = 0;
        char[] arrayForNumber = new char[30];
        int numberArrayPointer = 0;

        while(i < lineToBeParsed.Length)
        {
            temp = lineToBeParsed[i];

            if(waitingForLetter)
            {
                assignedLetter = checkLetter(temp, ref waitingForLetter);
            }
            else
            {
                parseNumber(temp, arrayForNumber, ref numberArrayPointer, ref waitingForLetter, assignedLetter);
            }

            ++i;
        }

    }
};
public class CNCParser : MonoBehaviour
{
    // Start is called before the first frame update
    public GCodeStep a = new GCodeStep();
    
    public string gCodeInput;
    public GameObject drillBit;
    public float speed = 50;
    public void getInput(string s)
    {
               
        gCodeInput = s;

        gCodeInput = gCodeInput + '\0';

    }
    public Vector3 newPosition = new Vector3(0,0,0);
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        a.parseLine(gCodeInput);

        newPosition.x = a.X;
        newPosition.y = a.Y;
        newPosition.z = a.Z;
        if (a.G == 0)
        {
            drillBit.transform.position = Vector3.MoveTowards(drillBit.transform.position, newPosition, 50);
        }
        if (a.G == 1)
        {
            drillBit.transform.position = Vector3.MoveTowards(drillBit.transform.position, newPosition, a.F/50);
        }
    }
}
