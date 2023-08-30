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
        manager.maxTokensPredict = 32;

        string warriorType = "A Martian warrior";

        var namePrompt = new PromptFormatter(
            new List<string>
            {
                "A godly warrior's name.",
                "A dancing warrior's name.",
            },
            new List<string>
            {
                "Zuesania",
                "Emeraldee"
            });

        Debug.Log("Name: " + name);

        smartPawn.characterName = await namePrompt.Prompt(
            manager,
            warriorType + " name.");

        var weaponPrompt = new PromptFormatter(
            new List<string>
            {
                "A godly warrior named Zuesania's weapon name.",
                "A dancing warrior named Emeraldee's weapon name.",
            },
            new List<string>
            {
                "Zuesania's Pike",
                "The Tango Mancer"
            });

        smartPawn.characterWeapon = await weaponPrompt.Prompt(
            manager,
            warriorType + " named " + name + "'s weapon name.");

/*        string descriptionPrompt = new PromptFormatter(
            new List<string>
            {
                "A godly warrior Zuesania.",
                "A dancing warrior Emeraldee.",
            },
            new List<string>
            {
                "Zuesania was born from the skies.",
                "The warriro Emeraldee is known to weild tango dancing as her true weapon."
            }).Build(warriorType + " " + name + ".");

        Debug.Log(descriptionPrompt);

        string description = await manager.Prompt(descriptionPrompt);
        smartPawn.characterDescription = description.Trim();
        Debug.Log("Description: " + description);

        string quotePrompt = $"Give a snippet of dialogue from {warriorType} named {name} about";

        string belief = await manager.Prompt(quotePrompt + " their beliefs:");
        smartPawn.currentBeliefState = belief.Trim();
        Debug.Log("Belief: " + belief);

        string hunger = await manager.Prompt(quotePrompt + " their hunger:");
        smartPawn.currentHungerState = hunger.Trim();
        Debug.Log("Hunger: " + hunger);
*/    }
}
