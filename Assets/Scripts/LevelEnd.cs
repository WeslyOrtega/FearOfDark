using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    public LevelManager levelManager;
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name == "Kid")
        {
            levelManager.LoadNextLevel();
        }
    }
}
