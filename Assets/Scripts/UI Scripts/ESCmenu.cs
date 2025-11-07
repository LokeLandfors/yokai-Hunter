using UnityEngine;

public class ESCmenu : MonoBehaviour
{
    public GameObject menu;

    [SerializeField] private bool isMenuOpen = false;

    void Start()
    {
        if (menu == null)
        {
            return;
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f; 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            print("Hej");
            isMenuOpen = !isMenuOpen;
        }

        
        if (isMenuOpen)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
        }

        menu.SetActive(isMenuOpen);
    }

    }
