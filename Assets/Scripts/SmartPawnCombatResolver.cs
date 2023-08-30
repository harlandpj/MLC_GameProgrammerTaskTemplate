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

        string battlePromptInput = string.Format(
            BATTLE_PROMPT_TEMPLATE,
            pawnAttacking.characterName,
            pawnDefending.characterName,
            pawnAttacking.characterWeapon,
            pawnDefending.characterWeapon);

        string battlePrompt = new PromptFormatter(
            new List<string>
            {
                string.Format(BATTLE_PROMPT_TEMPLATE, "Gabro", "Doox", "Sword", "Shield"),
                string.Format(BATTLE_PROMPT_TEMPLATE, "Fux", "Ned", "Fists", "Fists"),
                string.Format(BATTLE_PROMPT_TEMPLATE, "Harold", "Betsy", "Pike", "Fists"),
            },
            new List<string>
            {
                "Doox blocks.",
                "Ned dies.",
                "Betsy is dies."
            }).Build(battlePromptInput);
        Debug.Log(battlePrompt);

        string battleResult = await manager.Prompt(battlePrompt);

        if (battleResult.Contains("die")
            || battleResult.Contains("dead")
            || battleResult.Contains("kill"))
        {
            return new BattleResult()
            {
                attacker = pawnAttacking,
                defender = pawnDefending,
                result = BattleResultValue.DefenderDies,
            };
        }
        Debug.Log("Battle result: " + battleResult);

        return new BattleResult()
        {
            attacker = pawnAttacking,
            defender = pawnDefending,
            result = BattleResultValue.AllLive,
        };
    }
}
