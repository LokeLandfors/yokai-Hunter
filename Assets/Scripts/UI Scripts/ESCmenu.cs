using UnityEngine;

public class ESCmenu : MonoBehaviour
{
    [Header("Assign your Canvas GameObject here")]
    public GameObject menuCanvas;

    private bool isMenuOpen = false;

    void Start()
    {
         
        if (menuCanvas != null)
            menuCanvas.SetActive(false);
        Time.timeScale = isMenuOpen ? 0f : 1f;


        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;

        if (menuCanvas != null)
            menuCanvas.SetActive(isMenuOpen);

        if (isMenuOpen)
        {
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
