using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonUI : MonoBehaviour
{
    [SerializeField] GameObject winnerUI;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame

    public void Winner()
    {
        winnerUI.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Quit()
    {
        Application.Quit();
    }

    void Update()
    {
        
    }
}
