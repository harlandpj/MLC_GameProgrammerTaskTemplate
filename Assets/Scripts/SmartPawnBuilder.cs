using Gpt4All;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SmartPawnBuilder : MonoBehaviour
{
    public LlmManager manager;

    public async Task BuildPawn(SmartPawn smartPawn)
    {
        string warriorType = "a Martian warrior";

        string name = await manager.Prompt($"Give a two-word random name for {warriorType}: ");
        smartPawn.characterName = name.Trim();
        Debug.Log("Name: " + name);

        string description = await manager.Prompt($"Describe in fifteen words the back story of {warriorType} named {name}: ");
        if (description.Substring(description.Length - 1, 1) != ".")
            description += "...";

        smartPawn.characterDescription = description.Trim();
        Debug.Log("Description: " + description);

        string quotePrompt = $"Give a snippet of dialogue from {warriorType} named {name} about";

        string belief = await manager.Prompt(quotePrompt + " their beliefs:");
        smartPawn.currentBeliefState = belief.Trim();
        Debug.Log("Belief: " + belief);

        string hunger = await manager.Prompt(quotePrompt + " their hunger:");
        smartPawn.currentHungerState = hunger.Trim();
        Debug.Log("Hunger: " + hunger);
    }
}
