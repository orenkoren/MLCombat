using MiddleAges.Database;
using MiddleAges.Events;
using UnityEngine;
using UnityEngine.UI;

public class AbilityKeybindHandler : MonoBehaviour
{
    public Abilities abilities;
    public GameObject[] abilityKeybindTextBoxes;

    void Start()
    {
        for (var i = 0; i < abilityKeybindTextBoxes.Length; i++)
        {
            abilityKeybindTextBoxes[i].GetComponent<Text>().text = abilities.GetAbilityBySlotNumber(i).Keybind.ToString();
        }
        GlobalEvents.KeybindChangedListeners += ChangeKeybinds;
    }

    void ChangeKeybinds(object sender, AbilityEventArgs abilityArgs)
    {
        AbilityData abilityData = abilities.GetAbility(abilityArgs.AbilityName);
        int keybindIndex = abilities.GetActionSlotNumber(abilityArgs.AbilityName);
        abilityKeybindTextBoxes[keybindIndex].GetComponent<Text>().text = abilityData.Keybind.ToString();
    }

    private void OnDestroy()
    {
        GlobalEvents.KeybindChangedListeners -= ChangeKeybinds;
    }
}
