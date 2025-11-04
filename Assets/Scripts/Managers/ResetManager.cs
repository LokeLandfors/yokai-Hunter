using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetManager : MonoBehaviour
{
    [SerializeField] GameObject[] resetObjects;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ResetScene()
    {
        for (int i = 0; i < resetObjects.Length; i++)
        {
            if (resetObjects[i] != null)
            {
                resetObjects[i].SendMessage("Reset");
            }
            
        }
    }
}
