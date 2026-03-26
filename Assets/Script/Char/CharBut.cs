using UnityEngine;

public class CharacterButton : MonoBehaviour
{
    [Header("Spawner Reference")]
    public CharacterSpawner spawner;

    [Header("Character Name")]
    public string characterName; // must match spawner entry

    public void SelectCharacter()
    {
        if (spawner == null)
        {
            Debug.LogError("Spawner not assigned!");
            return;
        }

        spawner.selectedCharacterName = characterName;
        Debug.Log("Selected: " + characterName);

        // Start the tutorial/info flow
        TutorialManager.Instance.StartInfo1();
    }
}