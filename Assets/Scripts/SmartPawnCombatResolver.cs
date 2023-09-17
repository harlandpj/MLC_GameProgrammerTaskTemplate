using Gpt4All;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SmartPawnCombatResolver : MonoBehaviour
{
    public const string BATTLE_PROMPT_TEMPLATE = "{0} attacks {1} with {2}. {1} defends with {3}.";
    public LlmManager manager;

    public enum BattleResultValue
    {
        AllLive,
        AttackerDies,
        DefenderDies,
    }

    public struct BattleResult
    {
        public BattleResultValue result;
        public SmartPawn attacker;
        public SmartPawn defender;
    }

    public void ResolveBattle(
        SmartPawn pawnAttacking,
        SmartPawn pawnDefending,
        System.Action<BattleResult> onResolved)
    {
        var task = Task.Run(async () => await ResolveBattleAsync(
            pawnAttacking,
            pawnDefending));

        onResolved(task.Result);
    }

    public async Task<BattleResult> ResolveBattleAsync(
        SmartPawn pawnAttacking,
        SmartPawn pawnDefending)
    {
/*        return new BattleResult()
        {
            attacker = pawnAttacking,
            defender = pawnDefending,
            result = BattleResultValue.DefenderDies,
        };
*/

/*        var randomAttackItem = "Fists.";
        if (pawnAttacking.itemsOwned.Count > 0)
            randomAttackItem = pawnAttacking.itemsOwned[Random.Range(0, pawnAttacking.itemsOwned.Count)];

        var randomDefendItem = "Fists.";
        if (pawnDefending.itemsOwned.Count > 0)
            randomDefendItem = pawnDefending.itemsOwned[Random.Range(0, pawnDefending.itemsOwned.Count)];
*/

        manager.maxTokensPredict = 128;

        string battlePromptInput = string.Format(
            BATTLE_PROMPT_TEMPLATE,
            pawnAttacking.characterName,
            pawnDefending.characterName,
            pawnAttacking.characterWeapon,
            pawnDefending.characterWeapon);

        var battlePrompt = new PromptFormatter(
            new List<string>
            {
                // person attacker, person defender, weapon attacker weapon defender
                string.Format(BATTLE_PROMPT_TEMPLATE, "Gabro", "Doox", "Sword", "Shield"),
                string.Format(BATTLE_PROMPT_TEMPLATE, "Fux", "Ned", "Fists", "Fists"),
                string.Format(BATTLE_PROMPT_TEMPLATE, "Harold", "Betsy", "Pike", "Fists"),

                // ok - don't know IF i need to have the attacker being the other way round too
                // for the LLM, added in case I do!
                string.Format(BATTLE_PROMPT_TEMPLATE, "Doox", "Gabro", "Shield", "Sword"),
                string.Format(BATTLE_PROMPT_TEMPLATE, "Ned", "Fux", "Fists", "Fists"),
                string.Format(BATTLE_PROMPT_TEMPLATE, "Betsy", "Harold", "Fists", "Pike"),
            },
            new List<string>
            {
                "Doox blocks.",
                "Ned dies.",
                "Betsy is dies.",

                // and the other way round
                "Gabro blocks.",
                "Fux dies.",
                "Harold is dies."
            });

        // need to mock the LLM input and processing/output result here due to time taken by LLM
        // so could just set up an array of strings and choose a result at random, or
        // could make it switchable via a UI button / or simply a bool flag settable in editor window
        // and mock it e.g if (myBoolSet) return new BattleResult(){ attacker = pawnAttacking,
        //                         defender = pawnDefending,
        //                           result = BattleResultValue.AttackerDies,}; 

        string battleResult = await battlePrompt.Prompt(manager, battlePromptInput);

        // As I am guessing as to how the LLM calculates the result of a battle, added the case of the
        // attacker dying
        if (battleResult.Contains("die")
            || battleResult.Contains("dead")
            || battleResult.Contains("kill"))
        {
            if (battleResult.Contains(pawnAttacking.characterName))
            {
                // attacking chessman died
                return new BattleResult()
                {
                    attacker = pawnAttacking,
                    defender = pawnDefending,
                    result = BattleResultValue.AttackerDies,
                };
            }
            else
            {
                // defending chessman died
                return new BattleResult()
                {
                    attacker = pawnAttacking,
                    defender = pawnDefending,
                    result = BattleResultValue.DefenderDies,
                };
            }
        }

        // ok - nobody died
       return new BattleResult()
       {
           attacker = pawnAttacking,
           defender = pawnDefending,
           result = BattleResultValue.AllLive,
       };
    }
}
