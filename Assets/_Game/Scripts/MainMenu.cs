using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject levelSelectPanel;

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenLevelSelect()
    {
        if (levelSelectPanel != null)
        {
            levelSelectPanel.SetActive(true);
        }
    }

    public void CloseLevelSelect()
    {
        if (levelSelectPanel != null)
        {
            levelSelectPanel.SetActive(false);
        }
    }

    public void LoadLevelByName(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}