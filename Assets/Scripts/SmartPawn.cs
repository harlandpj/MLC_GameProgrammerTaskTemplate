using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartPawn : Chessman
{
    [SerializeField] public string characterName = "Soldier";
    [SerializeField] public string characterDescription = "No description.";
    [SerializeField] public string characterWeapon = "Fists";
/*    [SerializeField] public string currentBeliefState = "Inspired";
    [SerializeField] public List<string> itemsOwned = new List<string>();
    [SerializeField] public string currentHungerState = "Full";
    [SerializeField] public List<string> awardsOwned = new List<string>();
    [SerializeField] public string currentEmotionalState = "Indifferent";
*/
    public override bool[,] PossibleMoves()
    {
        bool[,] r = new bool[8, 8];
        Move(CurrentX + 1, CurrentY, ref r); // up
        Move(CurrentX - 1, CurrentY, ref r); // down
        Move(CurrentX, CurrentY - 1, ref r); // left
        Move(CurrentX, CurrentY + 1, ref r); // right
        Move(CurrentX + 1, CurrentY - 1, ref r); // up left
        Move(CurrentX - 1, CurrentY - 1, ref r); // down left
        Move(CurrentX + 1, CurrentY + 1, ref r); // up right
        Move(CurrentX - 1, CurrentY + 1, ref r); // down right

        return r;
    }
}
