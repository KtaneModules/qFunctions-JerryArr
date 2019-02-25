using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using KModkit;


public class qFunctions : MonoBehaviour
{
    public KMBombInfo Bomb;
    public KMAudio Audio;
    public KMBombModule Module;

    private static int _moduleIdCounter = 1;
    private int _moduleId;

    public MeshRenderer meshNumberA;
    public MeshRenderer meshNumberB;
    public MeshRenderer functLetter;
    public MeshRenderer inputA;
    public MeshRenderer inputB;
    public MeshRenderer inputComma;
    public MeshRenderer inputResult;

    public KMSelectable[] buttons;
    public KMSelectable buttonQuery;
    public KMSelectable buttonComma;
    public KMSelectable buttonClear;
    public KMSelectable buttonSubmit;

    public KMRuleSeedable RuleSeedable;

    string[] alphabet = new string[26]
    { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
      "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};

    int[] theNumbers = new int[60] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24,
                        25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41,
        //RULESEED NUMBERS BELOW
        42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59 };

    string[] theFunctions = new string[60]
    {
"Digital root of ((a + b) squared)", "a times b, even-position digits removed", "8 concatenated with the number of odd digits, concatenated with the number of even digits", "Digital root of (a + b)",
        "(a + b) modulo 1000", "(a + b) squared", "Highest digit", "Number of different digits missing", "(Larger times 2) minus Smaller", "Sum of times each digit appears in bomb serial number",
        "Number of even numbers", "Dots found in digits when using morse code", "a + b, concatenated with |a minus b|.", "Integer of (Larger divided by Smaller)) modulo 1000", "Digital root of |a minus b|",
     "Lit indicators times 63", "a times b", "(a times b) modulo 1000", "(sum of digits in a) times (sum of digits in b)", "Smaller minus (Larger modulo Smaller)",
     "a times b, odd-position digits removed", "All digits missing concatenated from 1 to 0", "Lunar Addition", "a times b, odd digits removed",
     "(Digit concatenated with 2 if even and 1 if odd) for all digits in order", "sqrt(a) + sqrt(b)", "Digital root of (a times b)", "Digits in a and b times 202",
     "808", "810 minus (Number of numbers below 100)", "Larger modulo Smaller", "Sum of letters in each digit",
     "(Product of first and last digit of a) times (Product of first and last digit of b)", "sqrt(a + b)", "Product of first and last digits overall", "(a squared) + (b squared)",
     "(a + b) modulo 12", "|a minus b|", "(Digit concatenated with the number of times that digit appears) for all digits in order", "a + b", "Larger divided by Smaller",
     "(a + b) times (Larger divided by Smaller)",
     // RULESEED FUNCTIONS BELOW
        "(a times 10) + b", "Dashes found in digits when using morse code", "a concatenated with b, even-position digits removed", "Sum of digits in bomb serial number",
        "(a squared) * (b squared)", "| (a squared) minus (b squared) |", "(a squared) + b", "a + (b squared)", "(a times b) modulo 73", "(a modulo 50) + b",
        "((a modulo 4) + 2) to the power of ((b modulo 4) + 2)", "808 modulo (a + b)", "|a minus b| modulo 1000", "(a + b) times (a minus b)", "(a + b) times Larger", "(a + b) times Smaller",
        "(a + b) * b", "(a + b) * a"

    };

    string[,] theRules = new string[26, 2]
    {
        { "KBU or M in SN", "KBU and M not in SN" },        { "Battery, indicator, or port count = 2", "Battery, indicator, or port count not equal to 2" },
        { "First character in SN a digit", "First character in SN a letter" },        { "Lit BOB indicator", "No lit BOB indicator" },
        { "Unlit BOB indicator", "No unlit BOB indicator" },        { "First character in SN a letter", "First character in SN a digit" },
        { "Parallel port but no Serial port", "!(Parallel port but no Serial port)" },        { "At least one empty port plate", "No empty port plates" },
        { "No batteries", "At least one battery" },        { "Vowel in SN", "No vowel in SN" },        { "Indicators > 3", "Indicators <= 2" },
        { "Battery count even", "Battery count odd" },
        { "Ports > indicators", "Ports <= indicators" },        { "Lit indicators > Unlit indicators", "Lit indicators <= Unlit indicators" },
        { "Indicators > batteries", "Indicators <= batteries" },        { "Indicator count even", "Indicator count odd" },
        { "ERI or S in SN", "ERI and S not in SN" },        { "Exactly 3 letters in SN", "Not exactly 3 letters in SN" },
        { "Batteries > ports", "Batteries <= ports" },        { "Batteries > 4", "Batteries <= 4" },
        { "Lit and unlit indicator counts equal", "Lit and unlit indicator counts not equal" },        { "JQX or Z in SN", "JQX and Z not in SN" },
        { "At least three ports", "Two or fewer ports" },        { "No indicators", "At least one indicator" },
        { "At least 4 SN digits", "3 or fewer SN digits" },        { "No ports", "At least one port" }
    };

    int[] rsFunctionNum = new int[42];

    int[] rsRuleNum = new int[26];

    int[,] rsRuleOffset = new int[26, 2];

    int[,] ruleNumber = new int[26, 2]
    {
        { 6, -4 },        { 2, -3 },        { 5, -4 },        { 8, -8 },        { 6, -2 },        { 6, -5 },
        { 1, -5 },        { 1, -3 },        { 1, 5 },        { 5, -3 },        { 4, -1 },        { 6, 7 },
        { 3, -7 },        { 3, -5 },        { 6, -1 },        { 2, -3 },        { 1, -3 },        { 3, -2 },
        { 2, 4 },         { 4, 1 },        { 2, -2 },        { 7, 1 },        { 3, -5 },        { 3, -3 },
        { 4, -1 },        { 5, -1 }
    };
    bool pressedAllowed = false;

    // TWITCH PLAYS SUPPORT
    //int tpStages; This one is not needed for this module
    // TWITCH PLAYS SUPPORT

    int numberA = -1;
    int numberB = -1;
    int pickedLetter = -1;
    int pickedFunction = -1;
    int finalFunction = -1;
    int firstLastDigit = -1;

    int letterRuleOn = -1; // 0 is true, 1 is false, because I said so this time

    string currentInput = "";
    long currentInputAsNumber;
    long moduleSolution;

    int inputNumberA;
    int inputNumberB;
    bool commaIn = false;
    bool justQueried = false;
    bool isSolved = false;
    
    void Start()
    {
        _moduleId = _moduleIdCounter++;

        Init();
        pressedAllowed = true;
    }

    void Init()
    {
        /*
         *     int[] rsFunctionNum = new int[42];

    int[] rsRuleNum = new int[26];

    int[,] rsRuleOffset = new int[26, 2];
    */

        
        for (int i = 0; i < 42; i++)
            {
                rsFunctionNum[i] = i;
            }
        for (int i = 0; i < 26; i++)
        {
            rsRuleNum[i] = i;
            rsRuleOffset[i, 0] = ruleNumber[i, 0];
            rsRuleOffset[i, 1] = ruleNumber[i, 1];
        }
        var rnd = RuleSeedable.GetRNG();
        if (rnd.Seed != 1)
        {
            var letterRuleHit = 0;
            var letterRuleMiss = 0;
            rnd.ShuffleFisherYates(theNumbers);
            Debug.LogFormat("[Functions #{0}] Using rule seed: {1}.", _moduleId, rnd.Seed);
            
            for (int i = 0; i < 42; i++)
            {
                rsFunctionNum[i] = theNumbers[i];
                /*if (i != 90)
                {
                    Debug.Log("Function " + i + " is " + theFunctions[rsFunctionNum[i]] + ", " + rsFunctionNum[i]);
                }*/
            }
            for (int i = 0; i < 26; i++)
            {
                letterRuleHit = rnd.Next(1, 10);
                letterRuleMiss = rnd.Next(1, 10);
                if (letterRuleMiss == letterRuleHit || rnd.Next(0, 4) != 0)
                {
                    letterRuleMiss = letterRuleMiss * -1;
                }
                rsRuleOffset[i, 0] = letterRuleHit;
                rsRuleOffset[i, 1] = letterRuleMiss;
               /* if (i == 0)
                {
                    Debug.Log("Rule A is " + rsRuleOffset[0, 0] + " / " + rsRuleOffset[0, 1]);
                }*/
            }

        }
        /*for (var i = 0; i < 42; i++)
        {
            list[i].innerText = theFunctions[theNumbers[i]];
            //document.getElementsByClassName('listData')[i].innerText = theFunctions[theNumbers[i]];
        }*/

        delegationZone();
        Module.OnActivate += delegate { inputResult.GetComponentInChildren<TextMesh>().text = ""; };
        pickedLetter = UnityEngine.Random.Range(0, 26);
        pickedFunction = UnityEngine.Random.Range(0, 42);
        //pickedFunction = 26;
        if (UnityEngine.Random.Range(0, 10) < 7)
        {
            numberA = UnityEngine.Random.Range(1, 100);
        }
        else
        {
            numberA = UnityEngine.Random.Range(1, 1000);
        }

        numberB = numberA;
        while (numberB == numberA)
        {
            if (UnityEngine.Random.Range(0, 10) < 7)
            {
                numberB = UnityEngine.Random.Range(1, 100);
            }
            else
            {
                numberB = UnityEngine.Random.Range(1, 1000);
            }
        }
        meshNumberA.GetComponentInChildren<TextMesh>().text = numberA + "";
        meshNumberB.GetComponentInChildren<TextMesh>().text = numberB + "";
        functLetter.GetComponentInChildren<TextMesh>().text = alphabet[pickedLetter];
        doClear();
        letterRuleOn = UnityEngine.Random.Range(0, 2);
        doesRuleApply();
        finalFunction = pickedFunction + rsRuleOffset[pickedLetter, letterRuleOn];
        if (finalFunction > 41)
        {
            finalFunction = finalFunction - 42;
        }
        else if (finalFunction < 0)
        {
            finalFunction = finalFunction + 42;
        }
        Debug.LogFormat("[Functions #{0}] Display is {1} {2} {3}.", _moduleId, numberA, alphabet[pickedLetter], numberB);
        Debug.LogFormat("[Functions #{0}] Query Function is #{1}: {2}.", _moduleId, pickedFunction, theFunctions[rsFunctionNum[pickedFunction]]);
        Debug.LogFormat("[Functions #{0}] {1}, meaning rule {2} is {3}, so adjust {4} by {5}, so the Final Function is number {6}, solution below.", _moduleId,
        theRules[pickedLetter, letterRuleOn], alphabet[pickedLetter], letterRuleOn == 0 ? "true" : "false", pickedFunction, rsRuleOffset[pickedLetter, letterRuleOn],
        finalFunction);
        moduleSolution = -1;
        //Debug.Log("The number from 0-41 that was picked for the query function was " + pickedFunction);
        //Debug.Log("The number from 0-41 that was picked for the final function was " + finalFunction);
        functionZone(rsFunctionNum[finalFunction], numberA, numberB);
        moduleSolution = currentInputAsNumber;
        currentInputAsNumber = -1;
        currentInput = "";
        pressedAllowed = true;
        //dispText.text = numberA + " " + alphabet[pickedLetter] + " " + numberB;
    }

    void doNumber(int n)
    {
        currentInput = currentInput + "" + n;
        if (currentInput.Length > 12)
        {
            currentInput = currentInput.Substring(1, 12);
        }
        currentInputAsNumber = Int64.Parse(currentInput);
        inputResult.GetComponentInChildren<TextMesh>().text = currentInput;
    }

    void doClear()
    {
        inputNumberA = inputNumberB = 0;
        currentInput = "";
        currentInputAsNumber = 0;
        inputComma.GetComponentInChildren<TextMesh>().text = "";
        inputA.GetComponentInChildren<TextMesh>().text = "";
        inputB.GetComponentInChildren<TextMesh>().text = "";
        inputResult.GetComponentInChildren<TextMesh>().text = "";
        commaIn = false;
        justQueried = false;
    }

    void doComma()
    {
        if (!commaIn)
        {
            if (currentInput.Length > 4)
            {
                inputNumberA = Int32.Parse(currentInput.Substring(currentInput.Length - 4, 4));
            }
            else
            {
                inputNumberA = Int32.Parse(currentInput);
            }
            commaIn = true;
            inputComma.GetComponentInChildren<TextMesh>().text = ",";
            currentInput = "";
            currentInputAsNumber = 0;
            inputA.GetComponentInChildren<TextMesh>().text = "" + inputNumberA;
        }

    }

    void doQuery()
    {

        if (commaIn && !justQueried)
        {
            var giveStrike = false;
            if (currentInput.Length > 4)
            {
                inputNumberB = Int32.Parse(currentInput.Substring(currentInput.Length - 4, 4));
            }
            else
            {
                inputNumberB = Int32.Parse(currentInput);
            }
            if (inputNumberA == 0 || inputNumberB == 0)
            {
                Debug.LogFormat("[Functions #{0}] You queried a zero, that's a strike! Query not made.", _moduleId);
                giveStrike = true;
            }
            else if (inputNumberA == inputNumberB)
            {
                Debug.LogFormat("[Functions #{0}] You queried two of the same number, that's a strike! Query not made.", _moduleId);
                giveStrike = true;
            }
            if (giveStrike)
            {
                giveStrike = false;
                GetComponent<KMBombModule>().HandleStrike();
            }
            else
            {
                inputB.GetComponentInChildren<TextMesh>().text = "" + inputNumberB;
                currentInputAsNumber = inputNumberA + inputNumberB;
                functionZone(rsFunctionNum[pickedFunction], inputNumberA, inputNumberB);
                inputResult.GetComponentInChildren<TextMesh>().text = "" + currentInputAsNumber;
                if (firstLastDigit == -1)
                {
                    firstLastDigit = (int)currentInputAsNumber % 10;
                }
                currentInputAsNumber = 0;
                currentInput = "";
                justQueried = true;

            }
        }
    }

    void doSubmit()
    {
        if (pressedAllowed)
        {
            if (Int64.Parse(currentInput) == moduleSolution)
            {
                Debug.LogFormat("[Functions #{0}] Submitted input of {1} and the expected {2} match, module disarmed!", _moduleId, Int64.Parse(currentInput), moduleSolution);
                var winMessage = new string[10] { "BOOYAH!", "--DISARMED--", "YES! YES!", "NAILED IT!", "WOO!", "CHA-CHING!", "GOT IT!", "GENIUS!", "WELL DONE!", "YOU DID IT!" };
                isSolved = true;
                inputResult.GetComponentInChildren<TextMesh>().text = winMessage[UnityEngine.Random.Range(0, 10)];
                pressedAllowed = false;
                GetComponent<KMBombModule>().HandlePass();
            }
            else
            {
                Debug.LogFormat("[Functions #{0}] Submitted input of {1} and the expected {2} do not match, that's a strike!", _moduleId, Int64.Parse(currentInput), moduleSolution);
                GetComponent<KMBombModule>().HandleStrike();
            }
        }

    }

    void OnPress()
    {
        GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
    }

    void OnRelease()
    {
        GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonRelease, transform);
        if (pressedAllowed)
        {

            return;
        }

    }

#pragma warning disable 414
    private readonly string TwitchHelpMessage = @"Perform a query (which will first clear the top and middle screens) with !{0} (query/q) 9876, 5432. You can omit the comma. Submit an answer with !{0} (submit/s/answer/a) 1234567890.";
    private readonly bool TwitchShouldCancelCommand = false;
#pragma warning restore 414

    private IEnumerator ProcessTwitchCommand(string command)
    {

        var pieces = command.ToLowerInvariant().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        string theError;
        theError = "";
        yield return null;
        if (pieces.Count() == 0)
        {
            theError = "sendtochaterror Not enough arguments! You need at least 'query/q' with two numbers separated with a comma, or 'submit/s/answer/a', with one number.";
            yield return theError;
        }
        else if (pieces.Count() == 1 && (pieces[0] == "submit" || pieces[0] == "answer" || pieces[0] == "s" || pieces[0] == "a"))
        {
            theError = "sendtochaterror Not enough arguments! You need a number to submit, e.g. !{0} submit 1234567890.";
            yield return theError;

        }
        else if (pieces.Count() == 1 && (pieces[0] == "surpriseme" || pieces[0] == "mystery" || pieces[0] == "surprise" || pieces[0] == "doyourworst" || pieces[0] == "random"))
        {
            int surpriseA;
            int surpriseB;
            if (UnityEngine.Random.Range(0, 10) < 7)
            {
                surpriseA = UnityEngine.Random.Range(1, 100);
            }
            else
            {
                surpriseA = UnityEngine.Random.Range(1, 1000);
            }

            surpriseB = surpriseA;
            while (surpriseB == surpriseA)
            {
                if (UnityEngine.Random.Range(0, 10) < 7)
                {
                    surpriseB = UnityEngine.Random.Range(1, 100);
                }
                else
                {
                    surpriseB = UnityEngine.Random.Range(1, 1000);
                }
            }
            yield return new WaitForSeconds(.1f);
            yield return null;
            buttonClear.OnInteract();
            for (int l = 0; l < (""+ surpriseA).Length; l++)
            {
                var curDigit = Int16.Parse((""+ surpriseA).Substring(l, 1));
                yield return new WaitForSeconds(.1f);
                yield return null;
                buttons[curDigit].OnInteract();
            }
            yield return new WaitForSeconds(.1f);
            yield return null;
            buttonComma.OnInteract();
            for (int l = 0; l < (""+surpriseB).Length; l++)
            {
                var curDigit = Int16.Parse((""+surpriseB).Substring(l, 1));
                yield return new WaitForSeconds(.1f);
                yield return null;
                buttons[curDigit].OnInteract();
            }
            yield return new WaitForSeconds(.1f);
            yield return null;
            buttonQuery.OnInteract();
            theError = "sendtochat You just got SURPRISED!";
            yield return theError;

        }
        else if ((pieces.Count() == 1 || pieces.Count() == 2) && (pieces[0] == "query" || pieces[0] == "q"))
        {
            theError = "sendtochaterror Not enough arguments! You need two numbers to query, e.g. !{0} query 1234, 5678 or !{0} q 9876, 5432.";
            yield return theError;

        } 
        else if (pieces[0] != "submit" && pieces[0] != "s" && pieces[0] != "answer" && pieces[0] != "a" && pieces[0] != "query" && pieces[0] != "q")
        {
            theError = "sendtochaterror Invalid argument! You need at least 'query/q' with two numbers separated with a comma, or 'submit/s/answer/a', with one number.";
            yield return theError;
        }
        else if (pieces.Count() >= 2 && (pieces[0] == "submit" || pieces[0] == "s" || pieces[0] == "answer" || pieces[0] == "a"))
        {
                for (int k = 0; k < pieces[1].Length; k++)
                {
                    if (pieces[1].Substring(k, 1) != "0" && pieces[1].Substring(k, 1) != "1" && pieces[1].Substring(k, 1) != "2" && pieces[1].Substring(k, 1) != "3" &&
                        pieces[1].Substring(k, 1) != "4" && pieces[1].Substring(k, 1) != "5" && pieces[1].Substring(k, 1) != "6" && pieces[1].Substring(k, 1) != "7" &&
                        pieces[1].Substring(k, 1) != "8" && pieces[1].Substring(k, 1) != "9")
                    {
                        if (pieces[1].Substring(k, 1) == "-")
                        {
                            theError = "sendtochaterror Invalid character! Minus signs and negatives have no place here.";
                        }
                        else
                        {
                            theError = "sendtochaterror Invalid character! " + pieces[1].Substring(k, 1) + " is not a digit.";
                        }
                        yield return theError;
                    }
                }
            if (theError == "")
            {
                yield return new WaitForSeconds(.1f);
                yield return null;
                buttonClear.OnInteract();
                for (int l = 0; l < pieces[1].Length; l++)
                {
                    var curDigit = Int16.Parse(pieces[1].Substring(l, 1));
                    yield return new WaitForSeconds(.1f);
                    yield return null;
                    buttons[curDigit].OnInteract();
                }
                yield return new WaitForSeconds(.1f);
                yield return null;
                buttonSubmit.OnInteract();
            }
        }
        else if (pieces.Count() >= 3 && (pieces[0] == "query" || pieces[0] == "q"))
        {
            if (pieces[1].Substring(pieces[1].Length - 1, 1) == ",")
            {
                pieces[1] = pieces[1].Substring(0, pieces[1].Length - 1);
            }

            for (int j = 1; j < 3; j++)
            {
                for (int k = 0; k < pieces[j].Length; k++)
                {
                    if (pieces[j].Substring(k, 1) != "0" && pieces[j].Substring(k, 1) != "1" && pieces[j].Substring(k, 1) != "2" && pieces[j].Substring(k, 1) != "3" &&
                        pieces[j].Substring(k, 1) != "4" && pieces[j].Substring(k, 1) != "5" && pieces[j].Substring(k, 1) != "6" && pieces[j].Substring(k, 1) != "7" &&
                        pieces[j].Substring(k, 1) != "8" && pieces[j].Substring(k, 1) != "9" )
                    {
                        if (pieces[j].Substring(k, 1) == "-")
                        {
                            theError = "sendtochaterror Invalid character! Minus signs and negatives have no place here.";
                        }
                        else
                        {
                            theError = "sendtochaterror Invalid character! " + pieces[j].Substring(k,1) + " is not a digit.";
                        }
                        yield return theError;
                    }
                }
            }
            if (theError == "")
            {

                yield return new WaitForSeconds(.1f);
                yield return null;
                buttonClear.OnInteract();
                for (int l = 0; l < pieces[1].Length; l++)
                {
                    var curDigit = Int16.Parse(pieces[1].Substring(l, 1));
                    yield return new WaitForSeconds(.1f);
                    yield return null;
                    buttons[curDigit].OnInteract();
                }
                yield return new WaitForSeconds(.1f);
                yield return null;
                buttonComma.OnInteract();
                for (int l = 0; l < pieces[2].Length; l++)
                {
                    var curDigit = Int16.Parse(pieces[2].Substring(l, 1));
                    yield return new WaitForSeconds(.1f);
                    yield return null;
                    buttons[curDigit].OnInteract();
                }
                yield return new WaitForSeconds(.1f);
                yield return null;
                buttonQuery.OnInteract();
            }
        }
     }

    void delegationZone()
    {

        buttons[0].OnInteract += delegate () { OnPress(); doNumber(0); buttons[0].AddInteractionPunch(0.2f); return false; };
        buttons[1].OnInteract += delegate () { OnPress(); doNumber(1); buttons[1].AddInteractionPunch(0.2f); return false; };
        buttons[2].OnInteract += delegate () { OnPress(); doNumber(2); buttons[2].AddInteractionPunch(0.2f); return false; };
        buttons[3].OnInteract += delegate () { OnPress(); doNumber(3); buttons[3].AddInteractionPunch(0.2f); return false; };
        buttons[4].OnInteract += delegate () { OnPress(); doNumber(4); buttons[4].AddInteractionPunch(0.2f); return false; };
        buttons[5].OnInteract += delegate () { OnPress(); doNumber(5); buttons[5].AddInteractionPunch(0.2f); return false; };
        buttons[6].OnInteract += delegate () { OnPress(); doNumber(6); buttons[6].AddInteractionPunch(0.2f); return false; };
        buttons[7].OnInteract += delegate () { OnPress(); doNumber(7); buttons[7].AddInteractionPunch(0.2f); return false; };
        buttons[8].OnInteract += delegate () { OnPress(); doNumber(8); buttons[8].AddInteractionPunch(0.2f); return false; };
        buttons[9].OnInteract += delegate () { OnPress(); doNumber(9); buttons[9].AddInteractionPunch(0.2f); return false; };

        buttonClear.OnInteract += delegate () {
            OnPress(); doClear();
            buttonClear.AddInteractionPunch(0.2f); return false;
        };
        buttonComma.OnInteract += delegate () { OnPress(); doComma(); buttonClear.AddInteractionPunch(0.2f); return false; };
        buttonSubmit.OnInteract += delegate () { OnPress(); doSubmit(); buttonClear.AddInteractionPunch(0.4f); return false; };
        buttonQuery.OnInteract += delegate () { OnPress(); doQuery(); buttonClear.AddInteractionPunch(0.2f); return false; };

        buttons[0].OnInteractEnded += delegate () { OnRelease(); };
        buttons[1].OnInteractEnded += delegate () { OnRelease(); };
        buttons[2].OnInteractEnded += delegate () { OnRelease(); };
        buttons[3].OnInteractEnded += delegate () { OnRelease(); };
        buttons[4].OnInteractEnded += delegate () { OnRelease(); };
        buttons[5].OnInteractEnded += delegate () { OnRelease(); };
        buttons[6].OnInteractEnded += delegate () { OnRelease(); };
        buttons[7].OnInteractEnded += delegate () { OnRelease(); };
        buttons[8].OnInteractEnded += delegate () { OnRelease(); };
        buttons[9].OnInteractEnded += delegate () { OnRelease(); };

        buttonClear.OnInteractEnded += delegate () { OnRelease(); };
        buttonComma.OnInteractEnded += delegate () { OnRelease(); };
        buttonSubmit.OnInteractEnded += delegate () { OnRelease(); };
        buttonQuery.OnInteractEnded += delegate () { OnRelease(); };
        

    }
    /*
    public static class Extensions
    {
        // Fisher-Yates Shuffle
        public static IList<T> shuffle<T>(this IList<T> list, MonoRandom rnd)
        {
            var i = list.Count;
            while (i > 1)
            {
                var index = rnd.Next(i);
                i--;
                var value = list[index];
                list[index] = list[i];
                list[i] = value;
            }

            return list;
        }
    } */
    /*
    void doShuffle()
    {
        var rnd = RuleSeedable.GetRNG();
        if (rnd.Seed == 1)
        {

        }
        else
        {
            
            var numberCount = theNumbers.Length;
            while (numberCount > 1)
            {
                var xyz = rnd.Next(numberCount);
                numberCount--;
                var value = theNumbers[xyz];
                theNumbers[xyz] = theNumbers[numberCount];
                theNumbers[numberCount] = value;
            }
            var theThingy = "";

            for (var i = 0; i < 42; i++)
            {
                //list[i].innerText = theFunctions[theNumbers[i]];
            }
        }
        
    } */

    void doesRuleApply()
    {
        letterRuleOn = 1;
        switch (pickedLetter)
        {
            case 0:
                for (int i = 0; i < 6; i++)
                {
                    if (Bomb.GetSerialNumber().Substring(i, 1) == "K" || Bomb.GetSerialNumber().Substring(i, 1) == "B" ||
                        Bomb.GetSerialNumber().Substring(i, 1) == "U" || Bomb.GetSerialNumber().Substring(i, 1) == "M")
                    {
                        letterRuleOn = 0;
                    }
                }
                break;
            case 1:
                if (Bomb.GetBatteryCount() == 2 || Bomb.GetIndicators().Count() == 2 || Bomb.GetPortCount() == 2)
                {
                    letterRuleOn = 0;
                }
                break;
            case 2:
                if (Bomb.GetSerialNumber().Substring(0, 1) == "1" || Bomb.GetSerialNumber().Substring(0, 1) == "2" ||
                    Bomb.GetSerialNumber().Substring(0, 1) == "3" || Bomb.GetSerialNumber().Substring(0, 1) == "4" ||
                    Bomb.GetSerialNumber().Substring(0, 1) == "5" || Bomb.GetSerialNumber().Substring(0, 1) == "6" ||
                    Bomb.GetSerialNumber().Substring(0, 1) == "7" || Bomb.GetSerialNumber().Substring(0, 1) == "8" ||
                    Bomb.GetSerialNumber().Substring(0, 1) == "9" || Bomb.GetSerialNumber().Substring(0, 1) == "0")
                {
                    letterRuleOn = 0;
                }
                break;
            case 3:
                if (Bomb.IsIndicatorOn("BOB"))
                {
                    letterRuleOn = 0;
                }
                break;
            case 4:
                if (Bomb.IsIndicatorOff("BOB"))
                {
                    letterRuleOn = 0;
                }
                break;
            case 5:
                if (!(Bomb.GetSerialNumber().Substring(0, 1) == "1" || Bomb.GetSerialNumber().Substring(0, 1) == "2" ||
                    Bomb.GetSerialNumber().Substring(0, 1) == "3" || Bomb.GetSerialNumber().Substring(0, 1) == "4" ||
                    Bomb.GetSerialNumber().Substring(0, 1) == "5" || Bomb.GetSerialNumber().Substring(0, 1) == "6" ||
                    Bomb.GetSerialNumber().Substring(0, 1) == "7" || Bomb.GetSerialNumber().Substring(0, 1) == "8" ||
                    Bomb.GetSerialNumber().Substring(0, 1) == "9" || Bomb.GetSerialNumber().Substring(0, 1) == "0"))
                {
                    letterRuleOn = 0;
                }
                break;
            case 6:
                if (Bomb.IsPortPresent("Parallel") && !Bomb.IsPortPresent("Serial"))
                {
                    letterRuleOn = 0;
                }
                break;
            case 7:
                foreach (object[] plate in Bomb.GetPortPlates())
                {
                    if (plate.Length == 0)
                    {
                        letterRuleOn = 0;

                    }
                }
                break;
            case 8:
                if (Bomb.GetBatteryCount() == 0)
                {
                    letterRuleOn = 0;
                }
                break;
            case 9:
                for (int i = 0; i < 6; i++)
                {
                    if (Bomb.GetSerialNumber().Substring(i, 1) == "A" || Bomb.GetSerialNumber().Substring(i, 1) == "E" ||
                        Bomb.GetSerialNumber().Substring(i, 1) == "I" || Bomb.GetSerialNumber().Substring(i, 1) == "O" ||
                        Bomb.GetSerialNumber().Substring(i, 1) == "U")
                    {
                        letterRuleOn = 0;
                    }
                }
                break;
            case 10:
                if (Bomb.GetIndicators().Count() > 3)
                {
                    letterRuleOn = 0;
                }
                break;
            case 11:
                if (Bomb.GetBatteryCount() % 2 == 0)
                {
                    letterRuleOn = 0;
                }
                break;
            case 12:
                if (Bomb.GetPortCount() > Bomb.GetIndicators().Count())
                {
                    letterRuleOn = 0;
                }
                break;
            case 13:
                if (Bomb.GetOnIndicators().Count() > Bomb.GetOffIndicators().Count())
                {
                    letterRuleOn = 0;
                }
                break;
            case 14:
                if (Bomb.GetIndicators().Count() > Bomb.GetBatteryCount())
                {
                    letterRuleOn = 0;
                }
                break;
            case 15:
                if (Bomb.GetIndicators().Count() % 2 == 0)
                {
                    letterRuleOn = 0;
                }
                break;
            case 16:
                for (int i = 0; i < 6; i++)
                {
                    if (Bomb.GetSerialNumber().Substring(i, 1) == "E" || Bomb.GetSerialNumber().Substring(i, 1) == "R" ||
                        Bomb.GetSerialNumber().Substring(i, 1) == "I" || Bomb.GetSerialNumber().Substring(i, 1) == "S")
                    {
                        letterRuleOn = 0;
                    }
                }
                break;
            case 17:
                var numLetters = 0;
                for (int i = 0; i < 6; i++)
                {
                    if (!(Bomb.GetSerialNumber().Substring(i, 1) == "1" || Bomb.GetSerialNumber().Substring(i, 1) == "2" ||
                        Bomb.GetSerialNumber().Substring(i, 1) == "3" || Bomb.GetSerialNumber().Substring(i, 1) == "4" ||
                        Bomb.GetSerialNumber().Substring(i, 1) == "5" || Bomb.GetSerialNumber().Substring(i, 1) == "6" ||
                        Bomb.GetSerialNumber().Substring(i, 1) == "7" || Bomb.GetSerialNumber().Substring(i, 1) == "8" ||
                        Bomb.GetSerialNumber().Substring(i, 1) == "9" || Bomb.GetSerialNumber().Substring(i, 1) == "0"))
                    {
                        numLetters++;
                    }
                }
                if (numLetters == 3)
                {
                    letterRuleOn = 0;
                }
                break;
            case 18:
                if (Bomb.GetBatteryCount() > Bomb.GetPortCount())
                {
                    letterRuleOn = 0;
                }
                break;
            case 19:
                if (Bomb.GetBatteryCount() > 4)
                {
                    letterRuleOn = 0;
                }
                break;
            case 20:
                if (Bomb.GetOnIndicators().Count() == Bomb.GetOffIndicators().Count())
                {
                    letterRuleOn = 0;
                }
                break;
            case 21:
                for (int i = 0; i < 6; i++)
                {
                    if (Bomb.GetSerialNumber().Substring(i, 1) == "J" || Bomb.GetSerialNumber().Substring(i, 1) == "Q" ||
                        Bomb.GetSerialNumber().Substring(i, 1) == "X" || Bomb.GetSerialNumber().Substring(i, 1) == "Z")
                    {
                        letterRuleOn = 0;
                    }
                }
                break;
            case 22:
                if (Bomb.GetPortCount() > 2)
                {
                    letterRuleOn = 0;
                }
                break;
            case 23:
                if (Bomb.GetIndicators().Count() == 0)
                {
                    letterRuleOn = 0;
                }
                break;
            case 24:
                var numNumbers = 0;
                for (int i = 0; i < 6; i++)
                {
                        if (Bomb.GetSerialNumber().Substring(i, 1) == "1" || Bomb.GetSerialNumber().Substring(i, 1) == "2" ||
                    Bomb.GetSerialNumber().Substring(i, 1) == "3" || Bomb.GetSerialNumber().Substring(i, 1) == "4" ||
                    Bomb.GetSerialNumber().Substring(i, 1) == "5" || Bomb.GetSerialNumber().Substring(i, 1) == "6" ||
                    Bomb.GetSerialNumber().Substring(i, 1) == "7" || Bomb.GetSerialNumber().Substring(i, 1) == "8" ||
                    Bomb.GetSerialNumber().Substring(i, 1) == "9" || Bomb.GetSerialNumber().Substring(i, 1) == "0")
                    {
                        numNumbers++;
                    }
                }
                if (numNumbers > 3)
                {
                    letterRuleOn = 0;
                }
                break;
            case 25:
                if (Bomb.GetPortCount() == 0)
                {
                    letterRuleOn = 0;
                }
                break;
            default:
                break;
        }
    }


    void functionZone(int fNum, int inputY, int inputZ)
    {
        var toPrepend = "";
        if (moduleSolution == -1)
        {
            toPrepend = "Final ";
        }
        else
        {
            toPrepend = "Query ";
        }
        var wackyString = "";
        //fNum = 52;
        switch (fNum)
        {
            case 0: // Digital root of ((a+b) squared)
                currentInputAsNumber = (inputY + inputZ) * (inputY + inputZ);
                currentInput = toPrepend + "Function: Digital root of ((a+b) squared). ((" + inputY + "+" + inputZ + ") squared) is " + currentInputAsNumber + ".";
                while (currentInputAsNumber > 9)
                {
                    wackyString = "" + currentInputAsNumber;
                    currentInputAsNumber = 0;
                    for (int i = 0; i < wackyString.Length; i++)
                    {
                        currentInputAsNumber = Int16.Parse(wackyString.Substring(i, 1)) + currentInputAsNumber;

                    }
                }
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} Digital root of that is {4}.", _moduleId, inputY, inputZ, currentInput, currentInputAsNumber);
                break;
            case 1: // a*b, digits in even positions removed
                currentInputAsNumber = (inputY * inputZ);
                currentInput = toPrepend + "Function: a*b, digits in even positions removed. " + inputY + "*" + inputZ + " is " + currentInputAsNumber + ".";
                wackyString = "";
                for (int i = 0; i < ("" + currentInputAsNumber).Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        wackyString = wackyString + ("" + currentInputAsNumber).Substring(i, 1);
                    }

                }
                currentInputAsNumber = Int32.Parse(wackyString);
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} Removing even-position digits gives {4}.", _moduleId, inputY, inputZ, currentInput, currentInputAsNumber);

                break;
            case 2: //8 ccw # odd digits ccw with # even digits
                currentInput = toPrepend + "Function: 8, then number of odd digits, then number of even digits.";
                wackyString = "" + inputY + inputZ;
                int oddZ = 0;
                for (int i = 0; i < wackyString.Length; i++)
                {
                    if (wackyString.Substring(i, 1) == "1" || wackyString.Substring(i, 1) == "3" || wackyString.Substring(i, 1) == "5" || wackyString.Substring(i, 1) == "7"
                        || wackyString.Substring(i, 1) == "9")
                    {
                        oddZ++;
                    }

                }
                currentInputAsNumber = 800 + (10 * oddZ) + (wackyString.Length - oddZ);
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} This is {4}.", _moduleId, inputY, inputZ, currentInput, currentInputAsNumber);
                break;
            case 3: //Digital root of a+b
                currentInputAsNumber = inputY + inputZ;
                currentInput = toPrepend + "Function: Digital root of a+b. " + inputY + "+" + inputZ + " is " + currentInputAsNumber + ".";
                while (currentInputAsNumber > 9)
                {
                    wackyString = "" + currentInputAsNumber;
                    currentInputAsNumber = 0;
                    for (int i = 0; i < wackyString.Length; i++)
                    {
                        currentInputAsNumber = Int16.Parse(wackyString.Substring(i, 1)) + currentInputAsNumber;

                    }
                }
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} Digital root of that is {4}.", _moduleId, inputY, inputZ, currentInput, currentInputAsNumber);
                break;
            case 4: //a+b modulo 1000
                currentInputAsNumber = inputY + inputZ;
                currentInput = toPrepend + "Function: a+b modulo 1000. " + inputY + "+" + inputZ + " is " + currentInputAsNumber + ".";
                currentInputAsNumber = currentInputAsNumber % 1000;
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} That modulo 1000 is {4}.", _moduleId, inputY, inputZ, currentInput, currentInputAsNumber);
                break;
            case 5: // (a+b) squared
                currentInputAsNumber = (inputY + inputZ) * (inputY + inputZ);
                currentInput = toPrepend + "Function: (a+b) squared. " + inputY + "+" + inputZ + " squared is " + currentInputAsNumber + ".";
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3}", _moduleId, inputY, inputZ, currentInput);
                break;
            case 6: //Highest digit
                currentInput = toPrepend + "Function: Highest digit.";
                wackyString = "" + inputY + inputZ;
                int hiDigit = 0;
                for (int i = 0; i < wackyString.Length; i++)
                {
                    if (Int16.Parse(wackyString.Substring(i, 1)) > hiDigit)
                    {
                        hiDigit = Int16.Parse(wackyString.Substring(i, 1));
                    }

                }
                currentInputAsNumber = hiDigit;
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} This is {4}.", _moduleId, inputY, inputZ, currentInput, currentInputAsNumber);
                break;
            case 7: //Number of digits missing
                currentInput = toPrepend + "Function: Number of digits missing.";
                wackyString = "" + inputY + inputZ;
                int digitsOut = 0;
                bool isDigitFound = false;

                for (int j = 0; j < 10; j++)
                {
                    isDigitFound = false;
                    for (int i = 0; i < wackyString.Length; i++)
                    {
                        if (Int16.Parse(wackyString.Substring(i, 1)) == j)
                        {
                            isDigitFound = true;
                        }

                    }
                    if (!isDigitFound)
                    {
                        digitsOut++;
                    }
                }
                currentInputAsNumber = digitsOut;
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} This is {4}.", _moduleId, inputY, inputZ, currentInput, currentInputAsNumber);
                break;
            case 8: //(Larger*2) - Smaller
                int largerOne;
                int smallerOne;
                if (inputY > inputZ)
                {
                    largerOne = inputY;
                    smallerOne = inputZ;
                }
                else
                {
                    largerOne = inputZ;
                    smallerOne = inputY;
                }
                currentInput = toPrepend + "Function: (Larger*2)-Smaller. Larger is " + largerOne + ".";
                currentInputAsNumber = largerOne + largerOne - smallerOne;
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} This is {4}.", _moduleId, inputY, inputZ, currentInput, currentInputAsNumber);
                break;
            case 9: //Sum of times each digit appears in serial number
                currentInput = toPrepend + "Function: Sum of times each digit appears in serial number, which is " + Bomb.GetSerialNumber() + ".";
                wackyString = "" + inputY + inputZ;
                currentInputAsNumber = 0;
                for (int j = 0; j < wackyString.Length; j++)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        if (Bomb.GetSerialNumber().Substring(i, 1) == wackyString.Substring(j, 1))
                        {
                            currentInputAsNumber++;
                        }
                    }
                }
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} This is {4}.", _moduleId, inputY, inputZ, currentInput, currentInputAsNumber);
                break;
            case 10: //Number of even numbers
                currentInputAsNumber = 0;
                if (inputY % 2 == 0)
                {
                    currentInputAsNumber++;
                }
                if (inputZ % 2 == 0)
                {
                    currentInputAsNumber++;
                }

                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. " + toPrepend + "Function: Number of even numbers, which is {3}.", _moduleId, inputY, inputZ, currentInputAsNumber);
                break;
            case 11: //Dots in digits when using morse code
                currentInput = toPrepend + "Function: Dots found in digits when using morse code.";
                wackyString = "" + inputY + inputZ;
                currentInputAsNumber = 0;
                string curMorse = "( ";
                for (int i = 0; i < wackyString.Length; i++)
                {
                    if (wackyString.Substring(i, 1) == "1")
                    {
                        currentInputAsNumber += 1;
                        curMorse = curMorse + ".----";
                    }
                    else if (wackyString.Substring(i, 1) == "2")
                    {
                        currentInputAsNumber += 2;
                        curMorse = curMorse + "..---";
                    }
                    else if (wackyString.Substring(i, 1) == "3")
                    {
                        currentInputAsNumber += 3;
                        curMorse = curMorse + "...--";
                    }
                    else if (wackyString.Substring(i, 1) == "4")
                    {
                        currentInputAsNumber += 4;
                        curMorse = curMorse + "....-";

                    }
                    else if (wackyString.Substring(i, 1) == "5")
                    {
                        currentInputAsNumber += 5;
                        curMorse = curMorse + ".....";

                    }
                    else if (wackyString.Substring(i, 1) == "6")
                    {
                        currentInputAsNumber += 4;
                        curMorse = curMorse + "-....";
                    }
                    else if (wackyString.Substring(i, 1) == "7")
                    {
                        currentInputAsNumber += 3;
                        curMorse = curMorse + "--...";
                    }
                    else if (wackyString.Substring(i, 1) == "8")
                    {
                        currentInputAsNumber += 2;
                        curMorse = curMorse + "---..";
                    }
                    else if (wackyString.Substring(i, 1) == "9")
                    {
                        currentInputAsNumber += 1;
                        curMorse = curMorse + "----.";
                    }
                    else if (wackyString.Substring(i, 1) == "0")
                    {
                        currentInputAsNumber += 0;
                        curMorse = curMorse + "-----";
                    }
                    if (i == wackyString.Length - 1)
                    {
                        curMorse = curMorse + " ).";
                    }
                    else
                    {
                        curMorse = curMorse + " | ";
                    }
                }
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} Digits in morse were {4}, which is {5} dots.", _moduleId, inputY, inputZ, currentInput,
                    curMorse, currentInputAsNumber);
                break;
            case 12: // a+b, then |a-b|
                currentInput = toPrepend + "Function: a+b, then absolute value of a-b.";
                wackyString = "" + (inputY + inputZ) + Math.Abs(inputY - inputZ);
                currentInputAsNumber = Int64.Parse(wackyString);
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} Sum is {4} and difference is {5}, so the answer is {6}.", _moduleId, inputY, inputZ,
                    currentInput, (inputY + inputZ), Math.Abs(inputY - inputZ), currentInputAsNumber);
                break;
            case 13: //larger / smaller modulo 1000
                int largerOneB;
                int smallerOneB;
                currentInput = toPrepend + "Function: Integer of (Larger / Smaller) modulo 1000.";
                if (inputY > inputZ)
                {
                    largerOneB = inputY;
                    smallerOneB = inputZ;
                }
                else
                {
                    largerOneB = inputZ;
                    smallerOneB = inputY;
                }
                currentInputAsNumber = (int)(largerOneB / smallerOneB) % 1000;
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} The quotient is {4}, which modulo 1000 is {5}.", _moduleId, inputY, inputZ, currentInput,
                    (int)(largerOneB / smallerOneB), currentInputAsNumber);
                break;
            case 14: //Digital root of |a-b|
                currentInputAsNumber = Math.Abs(inputY - inputZ);
                currentInput = toPrepend + "Function: Digital root of |a-b|. |" + inputY + "-" + inputZ + "| is " + currentInputAsNumber + ".";
                while (currentInputAsNumber > 9)
                {
                    wackyString = "" + currentInputAsNumber;
                    currentInputAsNumber = 0;
                    for (int i = 0; i < wackyString.Length; i++)
                    {
                        currentInputAsNumber = Int16.Parse(wackyString.Substring(i, 1)) + currentInputAsNumber;

                    }
                }
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} Digital root of that is {4}.", _moduleId, inputY, inputZ, currentInput, currentInputAsNumber);
                break;
            case 15: //Lit indicators times 63
                currentInputAsNumber = 63 * Bomb.GetOnIndicators().Count();
                currentInput = toPrepend + "Function: Lit indicators times 63. Lit indicator count is " + Bomb.GetOnIndicators().Count() + ", times 63 is " + currentInputAsNumber + ".";

                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3}", _moduleId, inputY, inputZ, currentInput);
                break;
            case 16: //a * b
                currentInputAsNumber = inputY * inputZ;
                currentInput = toPrepend + "Function: a*b.";

                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} This is {4}", _moduleId, inputY, inputZ, currentInput, currentInputAsNumber);
                break;
            case 17: //(a * b) modulo 1000
                currentInputAsNumber = inputY * inputZ;
                currentInput = toPrepend + "Function: (a*b) modulo 1000. " + inputY + "*" + inputZ + " is " + currentInputAsNumber + ".";
                currentInputAsNumber = (inputY * inputZ) % 1000;
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} That product modulo 1000 is {4}", _moduleId, inputY, inputZ,
                    currentInput, currentInputAsNumber);
                break;
            case 18: //(sum of a's digits) * (sum of b's digits)
                currentInput = toPrepend + "Function: Sum of a's digits times sum of b's digits.";
                int sumA = 0;
                int sumB = 0;
                for (int i = 0; i < ("" + inputY).Length; i++)
                {
                    sumA = sumA + Int16.Parse(("" + inputY).Substring(i, 1));
                }
                for (int j = 0; j < ("" + inputZ).Length; j++)
                {
                    sumB = sumB + Int16.Parse(("" + inputZ).Substring(j, 1));
                }
                currentInputAsNumber = sumA * sumB;
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} Digit sums are {4} and {5}, and their product is {6}.", _moduleId, inputY, inputZ,
                    currentInput, sumA, sumB, currentInputAsNumber);
                break;
            case 19: //smaller - (larger modulo smaller)
                int largerOneC;
                int smallerOneC;
                currentInput = toPrepend + "Function: Smaller - (Larger modulo smaller).";
                if (inputY > inputZ)
                {
                    largerOneC = inputY;
                    smallerOneC = inputZ;
                }
                else
                {
                    largerOneC = inputZ;
                    smallerOneC = inputY;
                }
                currentInputAsNumber = smallerOneC - (largerOneC % smallerOneC);
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} That is {4}.", _moduleId, inputY, inputZ,
                currentInput, currentInputAsNumber);
                break;
            case 20: //a*b, odd-position digits removed
                currentInputAsNumber = (inputY * inputZ);
                currentInput = toPrepend + "Function: a*b, digits in odd positions removed. " + inputY + "*" + inputZ + " is " + currentInputAsNumber + ".";
                wackyString = "";
                for (int i = 0; i < ("" + currentInputAsNumber).Length; i++)
                {
                    if (i % 2 == 1)
                    {
                        wackyString = wackyString + ("" + currentInputAsNumber).Substring(i, 1);
                    }

                }
                if (wackyString == "")
                {
                    wackyString = "0";
                    currentInput = currentInput + " Product is only one digit so it becomes a 0.";
                }
                currentInputAsNumber = Int32.Parse(wackyString);
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} Removing odd-position digits gives {4}.", _moduleId, inputY, inputZ, currentInput, currentInputAsNumber);

                break;
            case 21: //All digits missing from 1 to 0
                wackyString = "" + inputY + inputZ;
                currentInput = toPrepend + "Function: All digits missing, from 1 to 0.";
                string finalString = "";
                for (int i = 1; i < 11; i++)
                {
                    bool itExists = false;
                    if (i == 10)
                    {
                        for (int j = 0; j < wackyString.Length; j++)
                        {
                            if (wackyString.Substring(j, 1) == "0")
                            {
                                itExists = true;
                            }
                        }
                        if (!itExists)
                        {
                            finalString = finalString + "0";
                        }
                    }
                    else
                    {
                        for (int j = 0; j < wackyString.Length; j++)
                        {
                            if (wackyString.Substring(j, 1) == ("" + i))
                            {
                                itExists = true;
                            }
                        }
                        if (!itExists)
                        {
                            finalString = finalString + "" + i;
                        }
                    }
                }
                currentInputAsNumber = Int32.Parse(finalString);
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} This is {4}.", _moduleId, inputY, inputZ, currentInput, currentInputAsNumber);
                break;
            case 22: //Lunar Addition
                currentInput = toPrepend + "Function: Lunar Addition.";
                wackyString = "";
                string LAa = "" + inputY;
                string LAb = "" + inputZ;
                while (LAa.Length < 4)
                {
                    LAa = "0" + LAa;
                }
                while (LAb.Length < 4)
                {
                    LAb = "0" + LAb;
                }
                for (int i = 0; i < 4; i++)
                {
                    if (Int16.Parse(LAa.Substring(i, 1)) > Int16.Parse(LAb.Substring(i, 1)))
                    {
                        wackyString = wackyString + LAa.Substring(i, 1);
                    }
                    else
                    {
                        wackyString = wackyString + LAb.Substring(i, 1);
                    }
                }
                currentInputAsNumber = Int16.Parse(wackyString);
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} This is {4}.", _moduleId, inputY, inputZ, currentInput, currentInputAsNumber);
                break;
            case 23: //a*b, odd digits removed
                currentInputAsNumber = (inputY * inputZ);
                currentInput = toPrepend + "Function: a*b, odd digits removed. " + inputY + "*" + inputZ + " is " + currentInputAsNumber + ".";
                wackyString = "";
                for (int i = 0; i < ("" + currentInputAsNumber).Length; i++)
                {
                    if (Int32.Parse(("" + currentInputAsNumber).Substring(i, 1)) % 2 == 0)
                    {
                        wackyString = wackyString + ("" + currentInputAsNumber).Substring(i, 1);
                    }

                }
                if (wackyString == "")
                {
                    wackyString = "0";
                    currentInput = currentInput + " Product had only odd digits, so it becomes a 0.";
                }
                currentInputAsNumber = Int32.Parse(wackyString);
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} Removing odd digits gives {4}.", _moduleId, inputY, inputZ, currentInput, currentInputAsNumber);

                break;
            case 24: //(digit, then 2 for even and 1 for odd) for all digits in order
                wackyString = "" + inputY + inputZ;
                currentInput = toPrepend + "Function: Digit, then 2 if even and 1 if odd for all digits in order.";
                string digitOrder = "";
                int[] digit = new int[wackyString.Length];
                int[] digitCount = new int[10];
                bool digitUsed = false;
                for (int i = 0; i < wackyString.Length; i++)
                {
                    digitUsed = false;
                    digit[i] = Int32.Parse(wackyString.Substring(i, 1));
                    digitCount[digit[i]]++;

                    for (int j = 0; j < i; j++)
                    {
                        if (digit[i] == Int16.Parse(wackyString.Substring(j, 1)))
                        {
                            digitUsed = true;
                        }
                    }
                    if (!digitUsed)
                    {
                        digitOrder = digitOrder + "" + digit[i];
                    }
                }
                wackyString = "";
                for (int k = 0; k < digitOrder.Length; k++)
                {
                    wackyString = wackyString + "" + digitOrder.Substring(k, 1);
                    if (Int16.Parse(digitOrder.Substring(k, 1)) % 2 == 0)
                    {
                        wackyString = wackyString + "2";
                    }
                    else
                    {
                        wackyString = wackyString + "1";
                    }
                }
                if (wackyString.Length > 12)
                {
                    currentInput = currentInput + " Leftmost 12 digits used.";
                    wackyString = wackyString.Substring(0, 12);
                }
                currentInputAsNumber = Int64.Parse(wackyString);
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} This is {4}.", _moduleId, inputY, inputZ, currentInput, currentInputAsNumber);

                break;
            case 25: // sqrt(a) + sqrt(b)
                currentInputAsNumber = (int)(Mathf.Sqrt(inputY) + Mathf.Sqrt(inputZ));
                currentInput = toPrepend + "Function: (Square root of a) + (Square root of b). sqrt(" + inputY + ") + sqrt(" + inputZ + ")" +
                    " is " + (Mathf.Sqrt(inputY) + Mathf.Sqrt(inputZ)) + ".";

                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} This is made into the integer {4}.", _moduleId, inputY, inputZ, currentInput, currentInputAsNumber);
                break;
            case 26: //Digital root of a*b
                currentInputAsNumber = (inputY * inputZ);
                currentInput = toPrepend + "Function: Digital root of (a*b). (" + inputY + "*" + inputZ + ") is " + currentInputAsNumber + ".";
                while (currentInputAsNumber > 9)
                {
                    wackyString = "" + currentInputAsNumber;
                    currentInputAsNumber = 0;
                    for (int i = 0; i < wackyString.Length; i++)
                    {
                        currentInputAsNumber = Int16.Parse(wackyString.Substring(i, 1)) + currentInputAsNumber;

                    }
                }
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} Digital root of that is {4}.", _moduleId, inputY, inputZ, currentInput, currentInputAsNumber);
                break;
            case 27: //Number of digits * 202
                currentInput = toPrepend + "Function: Number of digits times 202.";
                currentInputAsNumber = ("" + inputY + inputZ).Length * 202;
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} This is {4}.", _moduleId, inputY, inputZ, currentInput, currentInputAsNumber);
                break;
            case 28: //808
                currentInput = toPrepend + "Function: Just return 808.";
                currentInputAsNumber = 808;
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} Okay, here is {4}.", _moduleId, inputY, inputZ, currentInput, currentInputAsNumber);
                break;
            case 29: //810 - (number of numbers below 100)
                currentInputAsNumber = 810;
                if (inputY < 100)
                {
                    currentInputAsNumber--;
                }
                if (inputZ < 100)
                {
                    currentInputAsNumber--;
                }

                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. " + toPrepend + "Function: 810 - (Number of numbers below 100), which equals {3}.", _moduleId, inputY, inputZ, currentInputAsNumber);
                break;
            case 30: //larger modulo smaller
                int largerOneD;
                int smallerOneD;
                currentInput = "Function: Larger modulo smaller.";
                if (inputY > inputZ)
                {
                    largerOneD = inputY;
                    smallerOneD = inputZ;
                }
                else
                {
                    largerOneD = inputZ;
                    smallerOneD = inputY;
                }
                currentInputAsNumber = largerOneD % smallerOneD;
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} This is {4}.", _moduleId, inputY, inputZ, currentInput, currentInputAsNumber);
                break;
            case 31: //Sum of letters in each digit
                currentInput = toPrepend + "Function: Sum of letters in each digit.";
                wackyString = "" + inputY + inputZ;
                currentInputAsNumber = 0;
                for (int i = 0; i < wackyString.Length; i++)
                {
                    if (wackyString.Substring(i, 1) == "1" || wackyString.Substring(i, 1) == "2" || wackyString.Substring(i, 1) == "6")
                    {
                        currentInputAsNumber += 3;
                    }
                    else if (wackyString.Substring(i, 1) == "4" || wackyString.Substring(i, 1) == "5" || wackyString.Substring(i, 1) == "9" || wackyString.Substring(i, 1) == "0")
                    {
                        currentInputAsNumber += 4;
                    }
                    else if (wackyString.Substring(i, 1) == "3" || wackyString.Substring(i, 1) == "7" || wackyString.Substring(i, 1) == "8")
                    {
                        currentInputAsNumber += 5;
                    }
                }
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} This is {4} letters.", _moduleId, inputY, inputZ, currentInput, currentInputAsNumber);
                break;
            case 32: //Product of products of first and last digits
                currentInput = toPrepend + "Function: Product of first and last digits in each number.";
                currentInputAsNumber = 1;
                wackyString = "" + inputY.ToString().Substring(0, 1) + inputY.ToString().Substring(inputY.ToString().Length - 1, 1) +
                                   inputZ.ToString().Substring(0, 1) + inputZ.ToString().Substring(inputZ.ToString().Length - 1, 1);
                for (int i = 0; i < 4; i++)
                {
                    currentInputAsNumber = currentInputAsNumber * Int16.Parse(wackyString.Substring(i, 1));
                }
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} This is {4}.", _moduleId, inputY, inputZ, currentInput, currentInputAsNumber);
                break;
            case 33: //Square root of (a+b)
                currentInputAsNumber = (int)Mathf.Sqrt(inputY + inputZ);
                currentInput = toPrepend + "Function: Square root of (a+b). " + inputY + " plus " + inputZ + " is " + (inputY + inputZ) + ".";
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} The square root of that is {4}, made into the integer {5}.", _moduleId, inputY, inputZ,
                    currentInput, Mathf.Sqrt(inputY + inputZ), currentInputAsNumber);
                break;
            case 34: //Product of first digit of a and last digit of b
                currentInput = toPrepend + "Function: Product of first and last digit overall.";
                currentInputAsNumber = 1;
                wackyString = "" + inputY.ToString().Substring(0, 1) + inputZ.ToString().Substring(inputZ.ToString().Length - 1, 1);
                for (int i = 0; i < 2; i++)
                {
                    currentInputAsNumber = currentInputAsNumber * Int16.Parse(wackyString.Substring(i, 1));
                }
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} This is {4}.", _moduleId, inputY, inputZ, currentInput, currentInputAsNumber);
                break;
            case 35: //a squared plus b squared
                currentInput = toPrepend + "Function: (a^2) + (b^2).";
                currentInputAsNumber = (inputY * inputY) + (inputZ * inputZ);
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} This is {4}.", _moduleId, inputY, inputZ, currentInput, currentInputAsNumber);
                break;
            case 36: //(a+b) modulo 12
                currentInput = toPrepend + "Function: (a+b) modulo 12.";
                currentInputAsNumber = (inputY + inputZ) % 12;
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} The sum is {4}, that modulo 12 is {5}.", _moduleId, inputY, inputZ, currentInput,
                    (inputZ + inputY), currentInputAsNumber);
                break;
            case 37: //Absolute value of a-b
                currentInput = toPrepend + "Function: Absolute value of (a-b)";
                currentInputAsNumber = Math.Abs(inputY - inputZ);
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} This is {4}.", _moduleId, inputY, inputZ, currentInput, currentInputAsNumber);
                break;
            case 38: //(digit then number of that digit) for all digits in order
                wackyString = "" + inputY + inputZ;
                currentInput = toPrepend + "Function: Digit, then number of times that digit appears for all digits in order.";
                string digitOrderB = "";
                int[] digitB = new int[wackyString.Length];
                int[] digitCountB = new int[10];
                bool digitUsedB = false;
                for (int i = 0; i < wackyString.Length; i++)
                {
                    digitUsedB = false;
                    digitB[i] = Int32.Parse(wackyString.Substring(i, 1));
                    digitCountB[digitB[i]]++;

                    for (int j = 0; j < i; j++)
                    {
                        if (digitB[i] == Int16.Parse(wackyString.Substring(j, 1)))
                        {
                            digitUsedB = true;
                        }
                    }
                    if (!digitUsedB)
                    {
                        digitOrderB = digitOrderB + "" + digitB[i];
                    }
                }
                wackyString = "";
                for (int k = 0; k < digitOrderB.Length; k++)
                {
                    wackyString = wackyString + "" + digitOrderB.Substring(k, 1);
                    wackyString = wackyString + "" + digitCountB[Int16.Parse(digitOrderB.Substring(k, 1))];
                }
                if (wackyString.Length > 12)
                {
                    currentInput = currentInput + " Leftmost 12 digits used.";
                    wackyString = wackyString.Substring(0, 12);
                }
                currentInputAsNumber = Int64.Parse(wackyString);
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} This is {4}.", _moduleId, inputY, inputZ, currentInput, currentInputAsNumber);

                break;
            case 39: //a+b
                currentInput = toPrepend + "Function: a plus b.";
                currentInputAsNumber = inputY + inputZ;
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} This is {4}.", _moduleId, inputY, inputZ, currentInput, currentInputAsNumber);
                break;
            case 40: // (Larger/Smaller)
                int largerOneE;
                int smallerOneE;
                currentInput = toPrepend + "Function: Larger divided by smaller.";
                if (inputY > inputZ)
                {
                    largerOneE = inputY;
                    smallerOneE = inputZ;
                }
                else
                {
                    largerOneE = inputZ;
                    smallerOneE = inputY;
                }
                currentInputAsNumber = (int)(largerOneE / smallerOneE);
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} This is {4}, the integer of which is {5}.", _moduleId, inputY, inputZ, currentInput,
                    (float)largerOneE / (float)smallerOneE, currentInputAsNumber);
                break;
            case 41: //(a+b) * (Larger / Smaller)
                int largerOneF;
                int smallerOneF;
                currentInput = toPrepend + "Function: (a+b) times (Larger divided by smaller).";
                if (inputY > inputZ)
                {
                    largerOneF = inputY;
                    smallerOneF = inputZ;
                }
                else
                {
                    largerOneF = inputZ;
                    smallerOneF = inputY;
                }
                currentInputAsNumber = (int)((inputY + inputZ) * ((float)largerOneF / (float)smallerOneF));
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} This is {5} times {4}, or {6}, the integer of which is {7}.", _moduleId, inputY, inputZ, currentInput,
                    ((float)largerOneF / (float)smallerOneF), (inputY + inputZ),
                    ((float)largerOneF / (float)smallerOneF) * ((inputY + inputZ)), currentInputAsNumber);
                break;
            case 42: //(a times 10) + b
                currentInput = toPrepend + "Function: (a times 10) + b.";
                currentInputAsNumber = (inputY * 10) + inputZ;
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} This is {4}.", _moduleId, inputY, inputZ, currentInput, currentInputAsNumber);
                break;
            case 43: //Dashes in digits when using morse code
                currentInput = toPrepend + "Function: Dots found in digits when using morse code.";
                wackyString = "" + inputY + inputZ;
                currentInputAsNumber = 0;
                string curMorse2 = "( ";
                for (int i = 0; i < wackyString.Length; i++)
                {
                    if (wackyString.Substring(i, 1) == "1")
                    {
                        currentInputAsNumber += 4;
                        curMorse2 = curMorse2 + ".----";
                    }
                    else if (wackyString.Substring(i, 1) == "2")
                    {
                        currentInputAsNumber +=3;
                        curMorse2 = curMorse2 + "..---";
                    }
                    else if (wackyString.Substring(i, 1) == "3")
                    {
                        currentInputAsNumber += 2;
                        curMorse2 = curMorse2 + "...--";
                    }
                    else if (wackyString.Substring(i, 1) == "4")
                    {
                        currentInputAsNumber += 1;
                        curMorse2 = curMorse2 + "....-";

                    }
                    else if (wackyString.Substring(i, 1) == "5")
                    {
                        currentInputAsNumber += 0;
                        curMorse2 = curMorse2 + ".....";

                    }
                    else if (wackyString.Substring(i, 1) == "6")
                    {
                        currentInputAsNumber += 1;
                        curMorse2 = curMorse2 + "-....";
                    }
                    else if (wackyString.Substring(i, 1) == "7")
                    {
                        currentInputAsNumber += 2;
                        curMorse2 = curMorse2 + "--...";
                    }
                    else if (wackyString.Substring(i, 1) == "8")
                    {
                        currentInputAsNumber += 3;
                        curMorse2 = curMorse2 + "---..";
                    }
                    else if (wackyString.Substring(i, 1) == "9")
                    {
                        currentInputAsNumber += 4;
                        curMorse2 = curMorse2 + "----.";
                    }
                    else if (wackyString.Substring(i, 1) == "0")
                    {
                        currentInputAsNumber += 5;
                        curMorse2 = curMorse2 + "-----";
                    }
                    if (i == wackyString.Length - 1)
                    {
                        curMorse2 = curMorse2 + " ).";
                    }
                    else
                    {
                        curMorse2 = curMorse2 + " | ";
                    }
                }
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} Digits in morse were {4}, which is {5} dots.", _moduleId, inputY, inputZ, currentInput,
                    curMorse2, currentInputAsNumber);
                break;
            case 44: //a concatenated with b, even-position digits removed
                currentInput = toPrepend + "Function: a concatenated with b, even-position digits removed.";
                wackyString = inputY + "" + inputZ;
                var curConcatString = "";
                for (int i = 0; i < wackyString.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        curConcatString = curConcatString + wackyString.Substring(i, 1);
                    }
                }
                currentInputAsNumber = Int16.Parse(curConcatString);
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} This is {4}, which with even-position digits removed is {5}.", _moduleId, inputY, inputZ, 
                    currentInput, wackyString, currentInputAsNumber);
                break;
            case 45: //a + b + number of digits in bomb serial number
                currentInput = toPrepend + "Function: Sum of digits in bomb serial number.";
                currentInputAsNumber = 0;
                for (int i = 0; i < 6; i++)
                {
                    if (Bomb.GetSerialNumber().Substring(i, 1) == "1" || Bomb.GetSerialNumber().Substring(i, 1) == "2" ||
                Bomb.GetSerialNumber().Substring(i, 1) == "3" || Bomb.GetSerialNumber().Substring(i, 1) == "4" ||
                Bomb.GetSerialNumber().Substring(i, 1) == "5" || Bomb.GetSerialNumber().Substring(i, 1) == "6" ||
                Bomb.GetSerialNumber().Substring(i, 1) == "7" || Bomb.GetSerialNumber().Substring(i, 1) == "8" ||
                Bomb.GetSerialNumber().Substring(i, 1) == "9" || Bomb.GetSerialNumber().Substring(i, 1) == "0")
                    {
                        currentInputAsNumber += Int16.Parse(Bomb.GetSerialNumber().Substring(i, 1));
                    }
                }
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} The SN is {4}, so the final total is {5}.", _moduleId, inputY, inputZ,
                  currentInput, Bomb.GetSerialNumber(), currentInputAsNumber);
                break;
            case 46: //a^2 * b^2
                currentInput = toPrepend + "Function: (a squared) times (b squared).";
                long superNumber = (long)(inputY * inputY) * (long)(inputZ * inputZ);
                wackyString = superNumber + "";
                if (wackyString.Length > 12)
                {
                    currentInput = currentInput + " Leftmost 12 digits used.";
                    wackyString = wackyString.Substring(0, 12);
                }
                currentInputAsNumber = Int64.Parse(wackyString);
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} a squared times b squared is {4}, so the final answer is {5}.", _moduleId, inputY, inputZ,
                     currentInput, superNumber, currentInputAsNumber);
                break;
            case 47: //|(a squared) - (b squared)|
                currentInput = toPrepend + "Function: | (a squared) minus (b squared) |.";
                currentInputAsNumber = Math.Abs((inputY * inputY) - (inputZ * inputZ));
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} The final answer is {4}.", _moduleId, inputY, inputZ,
                     currentInput, currentInputAsNumber);
                break;
            case 48: //(a squared) + b
                currentInput = toPrepend + "Function: (a squared) + b.";
                currentInputAsNumber = (inputY * inputY) + (inputZ);
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} The final answer is {4}.", _moduleId, inputY, inputZ,
                     currentInput, currentInputAsNumber);
                break;
            case 49: //a + (b squared)
                currentInput = toPrepend + "Function: a + (b squared).";
                currentInputAsNumber = (inputZ * inputZ) + (inputY);
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} The final answer is {4}.", _moduleId, inputY, inputZ,
                     currentInput, currentInputAsNumber);
                break;
            case 50: //(a times b) module 73
                currentInput = toPrepend + "Function: (a times b) module 73.";
                currentInputAsNumber = (inputY * inputZ) % 73;
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} The product is {4}, that modulo 73 is {5}.", _moduleId, inputY, inputZ,
                     currentInput, (inputY * inputZ), currentInputAsNumber);
                break;
            case 51: //(a modulo 50) + b"
                currentInput = toPrepend + "Function: (a modulo 50) + b.";
                currentInputAsNumber = (inputY % 50) + inputZ;
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} a modulo 50 is {4}, plus {5} is {6}.", _moduleId, inputY, inputZ,
                     currentInput, (inputY % 50), inputZ, currentInputAsNumber);
                break;
            case 52: //((a modulo 4) + 2) to the power of ((b modulo 4) + 2)
                currentInput = toPrepend + "Function: ((a modulo 4) + 2) to the power of ((b modulo 4) + 2).";
                currentInputAsNumber = 1;
                for (int i = 1; i <= ((inputZ % 4) + 2); i++)
                {
                    currentInputAsNumber = currentInputAsNumber * ((inputY % 4) + 2);
                }
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} ({7} modulo 4) + 2) is {4} and ({8} modulo 4) + 2) is {5}, so the final total is {6}.", _moduleId, inputY, inputZ,
                currentInput, ((inputY % 4) + 2), ((inputZ % 4) + 2), currentInputAsNumber, inputY, inputZ);
                break;
            case 53: // 808 modulo (a + b)
                currentInput = toPrepend + "Function: 808 modulo (a + b).";
                currentInputAsNumber = 808 % (inputY + inputZ);
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} This is {4}.", _moduleId, inputY, inputZ,
                    currentInput, currentInputAsNumber);
                break;
            case 54: // |a minus b| modulo 1000
                currentInput = toPrepend + "Function: |a minus b| modulo 1000.";
                currentInputAsNumber = Math.Abs(inputY - inputZ) % 1000;
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} This is {4}.", _moduleId, inputY, inputZ,
                    currentInput, currentInputAsNumber);
                break;
            case 55: //(a + b) times (a minus b)
                currentInput = toPrepend + "Function: (a + b) times |a minus b|.";
                currentInputAsNumber = Math.Abs(inputY - inputZ) * (inputY + inputZ);
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} This is {4} times {5}, or {6}.", _moduleId, inputY, inputZ,
                    currentInput, (inputY + inputZ), Math.Abs(inputY - inputZ), currentInputAsNumber);
                break;
            case 56: //(a + b) times Larger
                currentInput = toPrepend + "Function: (a + b) times Larger.";
                currentInputAsNumber = inputY + inputZ;
                var theLargest = 0;
                if (inputY > inputZ)
                {
                    currentInputAsNumber = currentInputAsNumber * inputY;
                    theLargest = inputY;
                }
                else
                {
                    currentInputAsNumber = currentInputAsNumber + inputZ;
                    theLargest = inputZ;
                }
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} This is {4} times {5}, or {6}.", _moduleId, inputY, inputZ,
                    currentInput, (inputY + inputZ), theLargest, currentInputAsNumber);
                break;
            case 57: //(a + b) times Smaller
                currentInput = toPrepend + "Function: (a + b) times Larger.";
                currentInputAsNumber = inputY + inputZ;
                var theSmallest = 0;
                if (inputY > inputZ)
                {
                    currentInputAsNumber = currentInputAsNumber * inputZ;
                    theSmallest = inputZ;
                }
                else
                {
                    currentInputAsNumber = currentInputAsNumber + inputY;
                    theSmallest = inputY;
                }
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} This is {4} times {5}, or {6}.", _moduleId, inputY, inputZ,
                    currentInput, (inputY + inputZ), theSmallest, currentInputAsNumber);
                break;
            case 58: //(a + b) * b
                currentInput = toPrepend + "Function: (a + b) times b.";
                currentInputAsNumber = (inputY + inputZ) * inputZ;
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} This is {4}.", _moduleId, inputY, inputZ,
                    currentInput, currentInputAsNumber);
                break;
            case 59: //(a + b) * a
                currentInput = toPrepend + "Function: (a + b) times b.";
                currentInputAsNumber = (inputY + inputZ) * inputY;
                Debug.LogFormat("[Functions #{0}] Variables are {1}, {2}. {3} This is {4}.", _moduleId, inputY, inputZ,
                    currentInput, currentInputAsNumber);
                break;
            default: // Uh oh, something's wrong, solve module and tell them to contact me
                isSolved = true;
                inputA.GetComponentInChildren<TextMesh>().text = "OOPS";
                inputB.GetComponentInChildren<TextMesh>().text = "MSG";
                inputResult.GetComponentInChildren<TextMesh>().text = "@JerryEris";
                pressedAllowed = false;
                GetComponent<KMBombModule>().HandlePass();
                Debug.LogFormat("[Functions #{0}] Something went wrong, please contact JerryEris#6034 on Discord!", _moduleId);
                break;
        }
        /*  
        "|a minus b| modulo 1000", "(a + b) times (a minus b)",, "(a + b) times Larger", "(a + b) * b", "(a + b) * a"
        */
    }
}
