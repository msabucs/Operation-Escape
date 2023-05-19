using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EquationFinder : MonoBehaviour {

    [HideInInspector] public int goal, initialNum, sceneIndex, playerToSwitchDist, lowerMV;
    [HideInInspector] public string bestEquation;
    [HideInInspector] public char initialOp;
    [HideInInspector] public bool isSwitch;
    [HideInInspector] public StringBuilder sb = new StringBuilder();
    [HideInInspector] public List<int> numbers, posNumbers, playerToNumDist, numToGoalDist, numToSwitchDist, finalEquation;
    [HideInInspector] public List<char> operators;
    [HideInInspector] public List<Dictionary<int, int>> distanceDict = new List<Dictionary<int, int>>();
    public int numberCount, operatorCount;
    GameManager gameManager;
    GridManager gridManager;
    OperatorButtons opButtons;
    PlayerMovement player;
    GoalTile goalTile;

    void Start() {

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        gridManager = GameObject.Find("Grid Manager").GetComponent<GridManager>();
        opButtons = GameObject.Find("Game Manager").GetComponent<OperatorButtons>();

        NumberTile[] numTiles = GameObject.FindObjectsOfType<NumberTile>();
        distanceDict = gridManager.distanceDict;
        playerToNumDist = gridManager.playerToNumDist;
        numToGoalDist = gridManager.numToGoalDist;
        numToSwitchDist = gridManager.numToSwitchDist;
        playerToSwitchDist = gridManager.playerToSwitchDist;

        Scene scene = SceneManager.GetActiveScene();
        sceneIndex = scene.buildIndex;

        initialNum = (int)gameManager.currentNumber;
        goal = gameManager.goalNumber;

        if(GameObject.FindGameObjectWithTag("Switch"))
            isSwitch = true;

        foreach(NumberTile num in numTiles) {
            var number = num.GetComponent<NumberTile>().number;

            // If switch tile exists, add negative numbers to the list 
            if(isSwitch) {
                numbers.Add(number);
                var negNumber = number * -1;
                numbers.Add(negNumber);
            }
            else 
                numbers.Add(number);

            posNumbers.Add(number);
        }

        if(opButtons.isAddDefault) initialOp = '+';
        if(opButtons.isSubDefault)  initialOp = '-';
        if(opButtons.isMultiDefault) initialOp = '*';
        if(opButtons.isDivDefault) initialOp = '/';

        if(GameObject.Find("ButtonAdd")) operators.Add('+');
        if(GameObject.Find("ButtonSub")) operators.Add('-');
        if(GameObject.Find("ButtonMulti")) operators.Add('*');
        if(GameObject.Find("ButtonDiv"))  operators.Add('/');

        // Generate permutations of equations
        List<string> permutations = GeneratePermutations(operators, numbers, operatorCount, numberCount, isSwitch);
            
        // Evaluate each equation if they are equal to the goal number
        foreach (string equation in permutations) {
            var currentMV = IsEquationCorrect(equation, distanceDict, posNumbers, playerToNumDist, numToGoalDist, numToSwitchDist, operators, 
            initialOp, playerToSwitchDist, goal, initialNum, operatorCount, isSwitch);
            if(lowerMV == 0 || lowerMV > currentMV) {
                Debug.Log("The best equation is: " +equation +" MV: " +currentMV);
                lowerMV = currentMV;
                bestEquation = equation;
            }            
        }

        foreach(char c in bestEquation) 
            if((c >= '0' && c <= '9')) 
                sb.Append(c);

        finalEquation = RemoveOperators(sb.ToString(), numbers, sceneIndex, isSwitch);
    }

    // Generate all possible permutations of equations from the given numbers and operators
    static List<string> GeneratePermutations(List<char> operators, List<int> numbers, int operandCount, int numberCount, bool isSwitch) {
            
        List<string> permutations = new List<string>();
            
        // Generate all possible combinations of operators and numbers
        List<string> operatorCombinations = OperatorCombinations(operators, operandCount);
        List<string> numberCombinations = NumberCombinations(numbers, numberCount);

        // Combine each operator combination with each number combination to create equations
        foreach (string operatorCombination in operatorCombinations) {
            foreach (string numberCombination in numberCombinations) {
                string equation = Combine(operatorCombination, numberCombination, isSwitch);
                permutations.Add(equation);
            }
        }
            
        return permutations;
    }

    // Generate all possible combination of operators
    static List<string> OperatorCombinations(List<char> items, int count) {
            
        List<string> combinations = new List<string>();

        // Generate all possible combinations of 'count' items from the array
        for (int i = 0; i < items.Count; i++) {
            if (count == 1)
                combinations.Add(items[i].ToString());
            else {
                List<string> subCombinations = OperatorCombinations(items, count - 1);
                foreach (string subCombination in subCombinations) {
                    string combination = items[i] + "," + subCombination;
                    combinations.Add(combination);
                }
            }
        }

        return combinations;
    }
        
    // Generate all possible combination of numbers
    static List<string> NumberCombinations(List<int> items, int count) {
            
        List<string> combinations = new List<string>();

        // Generate all possible combinations of 'count' items from the array
        for (int i = 0; i < items.Count; i++) {
            if (count == 1)
                combinations.Add(items[i].ToString());
            else {
                List<string> subCombinations = NumberCombinations(items, count - 1);
                foreach (string subCombination in subCombinations) {
                    string combination = items[i].ToString() + subCombination;
                    combinations.Add(combination);
                }
            }
        }
            
        return combinations;
    }

    // Combine operator combinations with number combinations to create equations
    static string Combine(string operandCombination, string numberCombination, bool isSwitch) {
            
        string[] operands = operandCombination.Split(',');
        List<int> numbers = new List<int>();
            
        // If switch tile exists
        if(isSwitch) {
            for(int i = 0; i < numberCombination.Length; i++) {
                if(numberCombination[i].Equals('-')) {
                    numbers.Add(int.Parse(numberCombination[i] + numberCombination[i+1].ToString()));
                    i++;
                }
                else 
                    numbers.Add(int.Parse(numberCombination[i].ToString()));
            }
        }
        else 
            foreach(var number in numberCombination)
                numbers.Add(int.Parse(number.ToString()));
            
        string equation = numbers[0].ToString();

        // Combine
        for (int i = 0; i < operands.Length; i++)
            equation += operands[i]  + numbers[i + 1];
            
        return equation;
    }

    // Check if is equation correct
    static int IsEquationCorrect(string equation, List<Dictionary<int, int>> distanceDict, List<int> posNumbers, List<int> playerToNumDist, 
    List<int> goalToNumDist, List<int> numToSwitchDist, List<char> operatorList, char defaultOp, int playerToSwitchDist, int goal, 
    int initialNum, int count, bool isSwitch) {
            
        var isEqual = false;
        var moveValue = 0;
        List<char> operators = new List<char>();   
            
        // Add all operands in the equation into the operators list
        if(isSwitch) {
            var countChecker = 0;
            for(int i = 0; i < equation.Length; i++) {
                if(!char.IsDigit(equation[i])) {
                    if(i == 0) 
                        continue;
                    else {
                        operators.Add(equation[i]);
                        i++; countChecker++;
                    }
                }
                if(countChecker == count)
                    break;
            }
        }
        else 
            foreach(char n in equation) 
                if(!char.IsDigit(n)) 
                    operators.Add(n);
            
        List<int> numbers = new List<int>();
            
        // Add all numbers in the equation into the parts list
        if(isSwitch) {
            for(int i = 0; i < equation.Length; i++) {
                if(char.IsDigit(equation[i])) {
                    numbers.Add(int.Parse(equation[i].ToString()));
                    i++;
                }
                else 
                    if(equation[i].Equals('-')) {
                        if(equation[i+1].Equals('-'))
                            continue;
                        else {
                            numbers.Add(int.Parse(equation[i] + equation[i+1].ToString()));
                            i++;
                        }
                    }
            }
        }
        else {
            for(int i = 0; i < equation.Length; i++) 
                if(i % 2 == 0) 
                    numbers.Add(int.Parse(equation[i].ToString()));                         
        }  
            
        // Equation evaluator
        foreach(char initialOp in operatorList) {
            var curSwitch = false;
            float currentNum = initialNum;
            float curNumChecker = initialNum;

            // Add move value if default operator is different
            if(initialOp != defaultOp) 
                moveValue++;

            switch(initialOp) {
                case '+':
                    currentNum += numbers[0];
                    if(curNumChecker > currentNum)
                        curSwitch = true;
                    break;
                case '-':
                    currentNum -= numbers[0];
                    if(curNumChecker < currentNum)
                        curSwitch = true;
                    break;
                case '*':
                    currentNum *= numbers[0];
                    if((curNumChecker > 0 && currentNum < 0) || (curNumChecker < 0 && currentNum > 0))
                        curSwitch = true;
                    break;
                case '/':
                    currentNum /= numbers[0];
                    if((curNumChecker > 0 && currentNum < 0) || (curNumChecker < 0 && currentNum > 0))
                        curSwitch = true;
                    break;
            }

            if(curSwitch) {
                moveValue += playerToSwitchDist;
                moveValue += numToSwitchDist[posNumbers.IndexOf(Mathf.Abs(numbers[0]))];
                curSwitch = false;
            }
            else {
                moveValue += playerToNumDist[posNumbers.IndexOf(Mathf.Abs(numbers[0]))];
            }
                
            for(int i = 0; i < operators.Count(); i++) {
                var op = operators[i];
                var operand = numbers[i+1];

                // Add move value when operator is changed
                if(i > 0)
                    if(op != operators[i-1])
                        moveValue++;

                // Add move value when negative switched to positive and vice versa
                if((numbers[i] > 0 && operand < 0) || (numbers[i] < 0 && operand > 0)) {
                    moveValue += numToSwitchDist[posNumbers.IndexOf(Mathf.Abs(numbers[i]))];
                    curSwitch = true;
                }
                
                // Check if player is currently on a switch tile
                if(curSwitch == false) {
                    if(numbers[i] != operand) {
                        var subDict = distanceDict[posNumbers.IndexOf(Mathf.Abs(operand))];
                        moveValue += subDict[Mathf.Abs(numbers[i])];
                    }
                    else
                        moveValue += 2;
                }
                else {
                    moveValue += numToSwitchDist[posNumbers.IndexOf(Mathf.Abs(operand))];
                    curSwitch = false;
                }
                        
                switch (op) {
                    case '+':
                        currentNum += operand;
                        break;
                    case '-':
                        currentNum -= operand;
                        break;
                    case '*':
                        currentNum *= operand;
                        break;
                    case '/':
                        currentNum /= operand;
                        break;
                }
            }
                
            if(currentNum == goal) {
                moveValue += goalToNumDist[posNumbers.IndexOf(Mathf.Abs(numbers.Last()))];
                isEqual = true;
                break;
            }
        } 

            if(isEqual) {
                //Debug.Log("The correct equation is: " +equation +" MV: " +moveValue);
                return moveValue;
            } else 
                return int.MaxValue;
    }

    static List<int> RemoveOperators(string equation, List<int> numbers, int sceneIndex, bool isSwitch) {
        
        List<int> finalEquation = new List<int>();

        if(isSwitch) 
            for(int i = 0; i < numbers.Count; i++) 
                if(numbers[i] < 0)
                    numbers.Remove(numbers[i]);
        
        foreach(char num in equation)
            finalEquation.Add(int.Parse(num.ToString()));

        return finalEquation;
    } 
}