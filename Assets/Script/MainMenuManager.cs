using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{    public GameObject[] menuBackgrounds;

    void Start()
    {
        for (int i = 0; i < menuBackgrounds.Length; i++)
        {
            if (menuBackgrounds[i] != null)
                menuBackgrounds[i].SetActive(false);
        }
        if (menuBackgrounds.Length > 0)
        {
            int randomIndex = Random.Range(0, menuBackgrounds.Length);
            menuBackgrounds[randomIndex].SetActive(true);
        }
    }
    public void SceneChange(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}