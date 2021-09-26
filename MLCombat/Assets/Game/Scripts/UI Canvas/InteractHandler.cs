using MiddleAges.Events;
using TMPro;
using UnityEngine;

public class InteractHandler : MonoBehaviour
{
    public TextMeshProUGUI interactText;

    void Start()
    {
        GlobalEvents.InteractEnteredRangeListeners += DisplayInteractName;
        GlobalEvents.InteractLeftRangeListeners += RemoveInteractName;
    }

    private void DisplayInteractName(object sender, GameObject gameObject)
    {
        if (gameObject.name != "")
            interactText.text = "<F>";
    }
    private void RemoveInteractName(object sender, GameObject gameObject)
    {
        interactText.text = "";
    }

    private void OnDestroy()
    {
        GlobalEvents.InteractEnteredRangeListeners -= DisplayInteractName;
        GlobalEvents.InteractLeftRangeListeners -= RemoveInteractName;
    }

}
