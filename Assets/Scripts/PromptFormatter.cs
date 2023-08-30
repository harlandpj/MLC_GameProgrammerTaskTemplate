using System.Collections;
using System.Collections.Generic;
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
}
