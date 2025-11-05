using UnityEngine;
using UnityEngine.SceneManagement;

public class ESCbutton : MonoBehaviour
{
    [Header("Assign the main menu canvas here")]
    public GameObject mainMenuCanvas;

    [Header("Optional canvases (assign later)")]
    public GameObject statsCanvas;
    public GameObject skillsCanvas;

    
    public void OnPlayPressed()
    {
        if (mainMenuCanvas != null)
            mainMenuCanvas.SetActive(false);

        
        Time.timeScale = 1f;

        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    
    public void OnQuitPressed()
    {
        
        SceneManager.LoadScene("Main Menu");
    }

    
    public void OnStatsPressed()
    {
        if (mainMenuCanvas != null)
            mainMenuCanvas.SetActive(false);

        if (statsCanvas != null)
            statsCanvas.SetActive(true);
        else
            Debug.LogWarning("Stats Canvas not assigned in MenuButtons script!");
    }

    
    public void OnSkillsPressed()
    {
        if (mainMenuCanvas != null)
            mainMenuCanvas.SetActive(false);

        if (skillsCanvas != null)
            skillsCanvas.SetActive(true);
        else
            Debug.LogWarning("Skills Canvas not assigned in MenuButtons script!");
    }
}
