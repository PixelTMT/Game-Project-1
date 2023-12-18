using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void ChangeScene(string name)
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(name, LoadSceneMode.Single);
    }
}
