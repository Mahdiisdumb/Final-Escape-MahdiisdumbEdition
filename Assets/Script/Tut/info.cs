using UnityEngine;

public class InfoCanvasController : MonoBehaviour
{
    [Header("Character Info Prefabs")]
    public CharacterInfoEntry[] characterInfos; // assign in inspector

    [Header("Parent for Info Canvas")]
    public Transform canvasParent; // empty GameObject where prefab will be instantiated

    private GameObject currentInfoInstance;

    // Call this to show info for a selected character
    public void ShowInfo(string characterName)
    {
        // Remove previous info if exists
        if (currentInfoInstance != null)
            Destroy(currentInfoInstance);

        // Find info prefab for the selected character
        CharacterInfoEntry entry = null;
        foreach (var info in characterInfos)
        {
            if (info.characterName == characterName)
            {
                entry = info;
                break;
            }
        }

        if (entry == null)
        {
            Debug.LogError("No info prefab found for character: " + characterName);
            return;
        }

        // Instantiate the info prefab as child of canvasParent
        currentInfoInstance = Instantiate(entry.infoPrefab, canvasParent);
    }
}

[System.Serializable]
public class CharacterInfoEntry
{
    public string characterName;
    public GameObject infoPrefab; // prefab showing controls and evade tips
}