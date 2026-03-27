using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    [Header("UI Canvases")]
    public GameObject menuCanvas;
    public GameObject infoCanvas1;
    public GameObject infoCanvas2;
    public GameObject blackScreenCanvas;
    public GameObject gameCanvas;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip info1Clip;
    public AudioClip info2Clip;
    public AudioClip transClip;
    public AudioClip arenaClip;

    [Header("Character Spawner")]
    public CharacterSpawner spawner;

    [Header("SonicEXE")]
    public GameObject sonicPrefab;
    public Transform sonicSpawn;

    [Header("Info Canvas Controller")]
    public InfoCanvasController infoController;

    private GameObject playerInstance;
    private GameObject sonicInstance;

    void Awake()
    {
        Instance = this;
    }

    // --- START FLOW ---
    public void StartInfo1()
    {
        if (spawner == null)
        {
            Debug.LogError("Spawner is NULL");
            return;
        }

        if (infoController == null)
        {
            Debug.LogError("InfoController is NULL");
            return;
        }

        if (string.IsNullOrEmpty(spawner.selectedCharacterName))
        {
            Debug.LogError("No character selected!");
            return;
        }

        menuCanvas.SetActive(false);
        infoCanvas1.SetActive(true);

        // Load + show character-specific info
        infoController.LoadCharacterInfo(spawner.selectedCharacterName);
        infoController.ShowInfo1();

        PlayAudio(info1Clip);
    }

    // --- NEXT: INFO1 -> INFO2 ---
    public void NextInfo1()
    {
        if (infoController == null)
        {
            Debug.LogError("InfoController is NULL");
            return;
        }

        infoCanvas1.SetActive(false);
        infoCanvas2.SetActive(true);

        infoController.ShowInfo2();

        PlayAudio(info2Clip);
    }

    // --- NEXT: INFO2 -> GAME ---
    public void NextInfo2()
    {
        infoCanvas2.SetActive(false);
        StartCoroutine(TransitionToGame());
    }

    // --- TRANSITION ---
    private IEnumerator TransitionToGame()
    {
        if (blackScreenCanvas != null)
            blackScreenCanvas.SetActive(true);

        PlayAudio(transClip);

        yield return new WaitForSeconds(2f);

        if (blackScreenCanvas != null)
            blackScreenCanvas.SetActive(false);

        StartGame();
    }

    // --- START GAME ---
    private void StartGame()
    {
        if (gameCanvas != null)
            gameCanvas.SetActive(true);

        if (spawner == null)
        {
            Debug.LogError("Spawner missing!");
            return;
        }

        // Spawn player
        spawner.SpawnCharacter();
        playerInstance = GameObject.FindWithTag("Player");

        if (playerInstance == null)
        {
            Debug.LogError("Player not found after spawning!");
            return;
        }

        // Spawn SonicEXE
        if (sonicPrefab != null && sonicSpawn != null)
        {
            sonicInstance = Instantiate(sonicPrefab, sonicSpawn.position, Quaternion.identity);

            SonicEXE exe = sonicInstance.GetComponent<SonicEXE>();
            if (exe != null)
            {
                exe.player = playerInstance.transform;
            }
            else
            {
                Debug.LogError("SonicEXE script missing on prefab!");
            }
        }
        else
        {
            Debug.LogError("Sonic prefab or spawn missing!");
        }

        PlayAudio(arenaClip);
    }

    // --- AUDIO ---
    private void PlayAudio(AudioClip clip)
    {
        if (clip == null || audioSource == null) return;

        audioSource.clip = clip;
        audioSource.Play();
    }
}