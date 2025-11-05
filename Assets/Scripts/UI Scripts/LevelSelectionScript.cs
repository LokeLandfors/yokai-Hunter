using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class LevelSelectionScript : MonoBehaviour
{
    [SerializeField] TMP_Text LevelName;


    [SerializeField] Button PreviousLevel;
    [SerializeField] Button NextLevel;


    //int scenes = SceneManager.sceneCount;

    int levels;

    int selectedLevel = 1;

    private void Start()
    {
        levels = SceneManager.sceneCountInBuildSettings;
        print(levels);
        PreviousLevel.onClick.AddListener(PreviousLvClick);
        NextLevel.onClick.AddListener(NextLvClick);
        LevelName.text = "Level " + selectedLevel;
    }

    void PreviousLvClick()
    {
        print("prev click");
        if (selectedLevel <= 1)
        {
            return;
        }
        else
        {
            LvChange(-1);
        }
    }

    void NextLvClick()
    {
        print("next click");
        if (selectedLevel >= levels)
        {
            return;
        }
        else
        {
            LvChange(1);
        }
    }

    void LvChange(int amount)
    {
        selectedLevel += amount;
        LevelName.text = "Level " + selectedLevel;
    }

    public void LoadLevel()
    {
        //print(SceneManager.GetSceneAt(selectedLevel));
        SceneManager.LoadScene(selectedLevel);
    }
}
