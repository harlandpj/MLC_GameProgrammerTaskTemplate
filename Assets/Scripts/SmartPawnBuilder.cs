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
                //"A godly warrior's name.",
                //"A dancing warrior's name.",
                "A female warrior's name.",
                "A warrior man's name.",
                "A foreign warrior gentlemen name.",
                "A political warrior's name.",
                "A tall warrior's name.",
                "A strong warrior's name."
            },
            new List<string>
            {
                //"Zuesania",
                //"Emeraldee",
                "Betsy",
                "Doox",
                "Fux",
                "Gabro",
                "Harold",
                "Ned"
            });

        //namePrompt.Build(warriorType + " " + name + ".\n");

        Debug.Log("Name: " + name);

        // I am guessing a little here based on format of the commented out original prompt for beliefs/hunger below!
        string theNamePrompt = $"Return the warrior name from {namePrompt}"; 
        // /*named {name} */ from ";

        smartPawn.characterName = await namePrompt.Prompt(
            manager,
            theNamePrompt + namePrompt /*warriorType*/);

        // ctor uses lists of input and output strings to build query
        var weaponPrompt = new PromptFormatter(
            new List<string>
            {
                //"A godly warrior named Zuesania's weapon name.",
                //"A dancing warrior named Emeraldee's weapon name.",
                "A dancing warrior lady named Betsy's weapon name.",
                "A fighting warrior man with a mysterious past Doox's weapon name.",
                "A cheerful warrior from a distant land Fux's weapon name.",
                "Not a warrior member of high society Gabro's weapon name.",
                "A warrior politician Harold's weapon name.",
                "A horse warrior Ned's weapon name."
            },
            new List<string>
            {
                //"Zuesania's Pike",
                //"The Tango Mancer"
                "Betsy's Shotgun",
                "Doox's Harpoon",
                "Fux's Mace",
                "Gabro's Spear",
                "Harold's Pistol",
                "Ned's Grenade"
            });
        
        weaponPrompt.Build(warriorType + " " + name + ".\n");

        string theWeaponPrompt = $"Return the weapon name of a warrior from {warriorType} named {namePrompt} from ";  

        smartPawn.characterWeapon = await weaponPrompt.Prompt(
            manager,
            //warriorType + " named " + name + "'s weapon name.");
            theWeaponPrompt + weaponPrompt);

        var descriptionPrompt = new PromptFormatter(
            new List<string>
            {
                //"A godly warrior Zuesania.",
                //"A dancing warrior Emeraldee.",
                "A dancing warrior Betsy.",
                "A fighting warrior Doox.",
                "A cheerful warrior Fux.",
                "A rotund warrior Gabro.",
                "A mesmerising warrior Harold.",
                "A sensible warrior Ned."
            },
            new List<string>
            {
                //"Zuesania was born from the skies.",
                //"The warrior Emeraldee is known to weild tango dancing as her true weapon."
                "Betsy came from another age and another time.",
                "Doox is known for his ability to lagh at any jokes even bad ones.",
                "Fux had as many problems with his name as the boy named Sue!",
                "Gabro thought he could sing but everyone thought he sounded like a cat wailing.",
                "Harold never liked his name but secretely called himself Tom.",
                "Ned has a reputation for a big laugh, a big belly, and an even bigger appetite for LLM!"
            });

        Debug.Log(descriptionPrompt);

        // another guess here on prompt format (I don't know how to specify name, so a bit stuffed!)
        string descripPrompt = $"Return the description of {warriorType} named name /*{namePrompt}*/ from "; 

        string description = await manager.Prompt(descripPrompt + descriptionPrompt);
        smartPawn.characterDescription = description.Trim();
        
        Debug.Log("Description: " + description + "\n");

        /*string quotePrompt = $"Give a snippet of dialogue from {warriorType} named {name} about";

        string belief = await manager.Prompt(quotePrompt + " their beliefs:");
        smartPawn.currentBeliefState = belief.Trim();
        Debug.Log("Belief: " + belief);

        string hunger = await manager.Prompt(quotePrompt + " their hunger:");
        smartPawn.currentHungerState = hunger.Trim();
        Debug.Log("Hunger: " + hunger);
        */
    }
}
