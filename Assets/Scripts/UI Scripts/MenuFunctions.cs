using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctions : MonoBehaviour
{
    [Header("Menu related")]
    [SerializeField] GameObject selfMenu; //parent menu
    [SerializeField] GameObject menuToOpen; //desired menu to swap too

    [Header("Level related)")]
    [SerializeField] int levelToLoad;

    public void ChangeActiveMenu()
    {
        selfMenu.SetActive(false);
        menuToOpen.SetActive(true);
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
