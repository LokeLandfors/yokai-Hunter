using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using TMPro;
using UnityEngine.UI;

public class LevelSelectionScript : MonoBehaviour
{
    [SerializeField] TMP_Text LevelName;


    [SerializeField] Button PreviousLevel;
    [SerializeField] Button NextLevel;


    int scenes = SceneManager.sceneCount;

    int levels;

    int selectedLevel = 1;

    private void Start()
    {
        levels = Mathf.Clamp(levels, 1, scenes);
    }

    public void ChangeLevel(int amount)
    {
        selectedLevel += amount;
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(selectedLevel);
    }
}
