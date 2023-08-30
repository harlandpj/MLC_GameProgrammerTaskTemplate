using Gpt4All;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class PromptFormatter
{
    List<string> exampleInputs;
    List<string> exampleOutputs;

    public PromptFormatter(
        List<string> exampleInputs,
        List<string> exampleOutputs)
    {
        this.exampleInputs = exampleInputs;
        this.exampleOutputs = exampleOutputs;
    }

    public string Build(string input)
    {
        Debug.Assert(exampleInputs.Count == exampleOutputs.Count);
        var prompt = "";
        for(int i = 0; i < exampleInputs.Count; ++i)
        {
            prompt += "INPUT:\n" + exampleInputs[i];
            prompt += "\nOUTPUT:\n" + exampleOutputs[i];
            prompt += "\n\n";
        }

        prompt += "INPUT:\n" + input;
        prompt += "\nOUTPUT:";
        return prompt;
    }

    public async Task<string> Prompt(LlmManager manager, string input)
    {
        string prompt = Build(input);
        Debug.Log("PROMPT: " + prompt);
        string result = await manager.Prompt(prompt);
        Debug.Log("RESULT: " + result);

        if (result == string.Empty)
            return result;
        else
            return result.Split('\n').First(x => x != string.Empty).Trim();
    }
}
