using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    
    public GameObject mainMenuCanvas;

   
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
    }

    
    public void OnSkillsPressed()
    {
        if (mainMenuCanvas != null)
            mainMenuCanvas.SetActive(false);

        if (skillsCanvas != null)
            skillsCanvas.SetActive(true);
    }
}
