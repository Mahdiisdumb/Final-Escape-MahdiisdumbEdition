using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    [Header("UI Canvases")]
    public GameObject menuCanvas;        // character selection
    public GameObject infoCanvas1;       // first info screen container
    public GameObject infoCanvas2;       // second info screen container
    public GameObject blackScreenCanvas; // transition
    public GameObject gameCanvas;        // actual gameplay HUD

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

    // --- Called by character button ---
    public void StartInfo1()
    {
        if (string.IsNullOrEmpty(spawner.selectedCharacterName))
        {
            Debug.LogError("No character selected!");
            return;
        }

        menuCanvas.SetActive(false);

        // Enable canvas first
        infoCanvas1.SetActive(true);

        // Show the right info prefab for the selected character
        if (infoController != null)
        {
            infoController.ShowInfo(spawner.selectedCharacterName);
        }
        else
        {
            Debug.LogError("InfoCanvasController not assigned!");
        }

        PlayAudio(info1Clip);
    }

    // --- Next button for info1 ---
    public void NextInfo1()
    {
        infoCanvas1.SetActive(false);
        infoCanvas2.SetActive(true);
        PlayAudio(info2Clip);
    }

    // --- Next button for info2 ---
    public void NextInfo2()
    {
        infoCanvas2.SetActive(false);
        StartCoroutine(TransitionToGame());
    }

    // --- Black screen transition ---
    private IEnumerator TransitionToGame()
    {
        blackScreenCanvas.SetActive(true);
        PlayAudio(transClip);

        yield return new WaitForSeconds(2f);

        blackScreenCanvas.SetActive(false);
        StartGame();
    }

    // --- Enable gameplay ---
    private void StartGame()
    {
        gameCanvas.SetActive(true);

        // Spawn selected character
        spawner.SpawnCharacter();
        playerInstance = GameObject.FindWithTag("Player"); // assumes prefab has "Player" tag

        // Spawn SonicEXE
        if (sonicPrefab != null && playerInstance != null)
        {
            sonicInstance = Instantiate(sonicPrefab, sonicSpawn.position, Quaternion.identity);
            SonicEXE exe = sonicInstance.GetComponent<SonicEXE>();
            exe.player = playerInstance.transform;
        }

        PlayAudio(arenaClip);
    }

    // --- Play audio helper ---
    private void PlayAudio(AudioClip clip)
    {
        if (clip == null || audioSource == null) return;
        audioSource.clip = clip;
        audioSource.Play();
    }
}