using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuManager : MonoBehaviour
{
    public GameObject[] menuBackgrounds;
    void Start()
    {
        Debug.Log($"Initializing {Application.productName}");
        Debug.Log($"Version {Application.version}");
        Debug.Log($"Running on {Application.platform}");
        Debug.Log("===================================");
        Debug.Log("=========FINAL=ESCAPE==============");
        Debug.Log("=========BY:=MAHDIISDUMB===========");
        Debug.Log("===================================");
        for (int i = 0; i < menuBackgrounds.Length; i++)
        {
            if (menuBackgrounds[i] != null)
                menuBackgrounds[i].SetActive(false);
        }
        if (menuBackgrounds.Length > 0)
        {
            int randomIndex = Random.Range(0, menuBackgrounds.Length);
            menuBackgrounds[randomIndex].SetActive(true);
            Debug.Log($"BG Loaded: {menuBackgrounds[randomIndex].name}");
        }
    }
    public void SceneChange(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}