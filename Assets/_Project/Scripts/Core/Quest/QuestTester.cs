using Quest;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuestTester : MonoBehaviour
{
    public QuestSo testQuest;

    public void GiveQuest(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        QuestManager.Instance.AcceptQuest(testQuest);
    }

    public void ProgressQuest(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        EventBus.RequestUpdateQuestObjective("Slime", 1);
    }
}
