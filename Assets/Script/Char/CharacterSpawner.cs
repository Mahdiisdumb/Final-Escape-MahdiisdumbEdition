using UnityEngine;

[System.Serializable]
public class CharacterEntry
{
    public string characterName;
    public GameObject prefab;
}

public class CharacterSpawner : MonoBehaviour
{
    [Header("Character List")]
    public CharacterEntry[] characters;

    [Header("Spawn Point")]
    public Transform spawnPoint;

    [Header("Selected Character")]
    public string selectedCharacterName = "";

    private GameObject playerInstance;

    public void SpawnCharacter()
    {
        if (string.IsNullOrEmpty(selectedCharacterName))
        {
            Debug.LogError("No character selected!");
            return;
        }

        CharacterEntry entry = null;
        foreach (var c in characters)
        {
            if (c.characterName == selectedCharacterName)
            {
                entry = c;
                break;
            }
        }

        if (entry == null)
        {
            Debug.LogError("Selected character not found: " + selectedCharacterName);
            return;
        }

        playerInstance = Instantiate(entry.prefab, spawnPoint.position, Quaternion.identity);
    }
}