using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    private int sceneSkipIndex;

    private void Awake()
    {
        sceneSkipIndex = 0;

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        
        TMP_Dropdown dropdown = FindAnyObjectByType<TMP_Dropdown>();
        if (dropdown != null)
        {
            Debug.Log(dropdown.gameObject.name, dropdown.gameObject);

            dropdown.onValueChanged.AddListener(newValue =>
            {
                sceneSkipIndex = newValue;
                Debug.Log($"value changed!: {newValue}");
            });
        }

    }

    public void NextLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void SkipToLevel()
    {
        // 0 is lvl 1, 1 is lvl 2, etc

        LoadScene($"level{sceneSkipIndex + 1}");
    }
}
