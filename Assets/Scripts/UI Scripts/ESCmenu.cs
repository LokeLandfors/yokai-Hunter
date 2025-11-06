using UnityEngine;

public class ESCmenu : MonoBehaviour
{
    [Header("Assign your menu canvas or panel here")]
    public GameObject menu;

    private bool isMenuOpen = false;

    void Start()
    {
        if (menu == null)
        {
            Debug.LogError("⚠️ ESCmenu: No menu assigned in the Inspector!");
            return;
        }

        menu.SetActive(false); // Hide at start
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f; // Normal gameplay speed
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
        menu.SetActive(isMenuOpen);

        if (isMenuOpen)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f; // Pause game
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f; // Resume game
        }
    }
}
