using UnityEngine;
using UnityEngine.SceneManagement;
public class OtherManager : MonoBehaviour
{
    public void SceneChange(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}