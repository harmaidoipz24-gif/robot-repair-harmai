using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour
{
    public void DisplayDialog()
    {
        UIHandler.instance.DisplayDialogue();
    }
}