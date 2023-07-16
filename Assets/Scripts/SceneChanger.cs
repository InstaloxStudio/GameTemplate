using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    public string sceneName;
    public Button PlayButton;

    void Awake()
    {
        PlayButton.onClick.AddListener(() => ChangeScene(sceneName));
    }
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}