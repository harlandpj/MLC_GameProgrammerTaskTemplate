using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SmartPawnView : MonoBehaviour
{
    [SerializeField] public SmartPawn selectedSmartPawn;

    [Header("UI")]
    [SerializeField] TMP_Text textCharacterName;
    [SerializeField] TMP_Text textCharacterDescription;
    [SerializeField] TMP_Text textCurrentBeliefState;
    [SerializeField] TMP_Text textItemsOwned;
    [SerializeField] TMP_Text textCurrentHungerState;
    [SerializeField] TMP_Text textAwardsOwned;
    [SerializeField] TMP_Text textCurrentEmotionalState;

    private void Update()
    {
        if (selectedSmartPawn == null)
        {
            ResetText();
            return;
        }

        textCharacterName.text = selectedSmartPawn.characterName;

        textCharacterDescription.text = selectedSmartPawn.characterDescription;

        textCurrentBeliefState.text = selectedSmartPawn.currentBeliefState;

        textItemsOwned.text = "";
        foreach (var item in selectedSmartPawn.itemsOwned)
            textItemsOwned.text += item + "\n";

        textCurrentHungerState.text = selectedSmartPawn.currentHungerState;

        textAwardsOwned.text = "";
        foreach (var item in selectedSmartPawn.awardsOwned)
            textAwardsOwned.text += item + "\n";

        textCurrentEmotionalState.text = selectedSmartPawn.currentEmotionalState;
    }

    private void ResetText()
    {
        textCharacterName.text = "";
        textCharacterDescription.text = "";
        textCurrentBeliefState.text = "";
        textItemsOwned.text = "";
        textCurrentHungerState.text = "";
        textAwardsOwned.text = "";
        textCurrentEmotionalState.text = "";
    }

    public void OnHoverEnter(GameObject hovered)
    {
        var smartPawn = hovered.GetComponent<SmartPawn>();
        if (smartPawn == null)
            return;

        selectedSmartPawn = smartPawn;
    }

    public void OnHoverExit(GameObject hovered)
    {
        var smartPawn = hovered.GetComponent<SmartPawn>();
        if (smartPawn == null)
            return;

        if(smartPawn == selectedSmartPawn)
            selectedSmartPawn = null;
    }
}
