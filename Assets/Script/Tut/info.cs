using UnityEngine;

public class InfoCanvasController : MonoBehaviour
{
    [Header("Character Info Prefabs")]
    public CharacterInfoEntry[] characterInfos;

    [Header("Parents")]
    public Transform info1Parent;
    public Transform info2Parent;

    private GameObject currentInfo1Instance;
    private GameObject currentInfo2Instance;

    private CharacterInfoEntry currentEntry;

    public void LoadCharacterInfo(string characterName)
    {
        currentEntry = null;

        foreach (var info in characterInfos)
        {
            if (info.characterName == characterName)
            {
                currentEntry = info;
                break;
            }
        }

        if (currentEntry == null)
        {
            Debug.LogError("No info found for character: " + characterName);
        }
    }

    public void ShowInfo1()
    {
        if (currentEntry == null)
        {
            Debug.LogError("Character info not loaded!");
            return;
        }

        if (currentInfo1Instance != null)
            Destroy(currentInfo1Instance);

        currentInfo1Instance = Instantiate(currentEntry.info1Prefab, info1Parent);
    }

    public void ShowInfo2()
    {
        if (currentEntry == null)
        {
            Debug.LogError("Character info not loaded!");
            return;
        }

        if (currentInfo2Instance != null)
            Destroy(currentInfo2Instance);

        currentInfo2Instance = Instantiate(currentEntry.info2Prefab, info2Parent);
    }
}

[System.Serializable]
public class CharacterInfoEntry
{
    public string characterName;
    public GameObject info1Prefab; // controls
    public GameObject info2Prefab; // evade tips
}