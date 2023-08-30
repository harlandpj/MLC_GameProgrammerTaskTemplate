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
        string warriorType = "A Martian warrior";

        string namePrompt = new PromptFormatter(
            new List<string>
            {
                "A godly warrior name.",
                "A dancing warrior name.",
            },
            new List<string>
            {
                "Zuesania",
                "Emeraldee"
            }).Build(warriorType + " name.");

        Debug.Log(namePrompt);
        string name = await manager.Prompt(namePrompt);
        smartPawn.characterName = name.Trim();
        Debug.Log("Name: " + name);

        string weaponPrompt = new PromptFormatter(
            new List<string>
            {
                "A godly warrior named Zuesania.",
                "A dancing warrior named Emeraldee.",
            },
            new List<string>
            {
                "Zuesania's Pike",
                "The Tango Mancer"
            }).Build(warriorType + " named " + name + ".");

        Debug.Log(weaponPrompt);
        string weapon = await manager.Prompt(weaponPrompt);
        smartPawn.characterWeapon = weapon.Trim();
        Debug.Log("Weapon: " + weapon);

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
