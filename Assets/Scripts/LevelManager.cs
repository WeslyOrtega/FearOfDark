using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Animator transition;
    public float transition_time = 1f;

    public void ResetScene()
    {
        StartCoroutine(_LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    public void LoadNextLevel()
    {
        StartCoroutine(_LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadScene(string levelName)
    {
        StartCoroutine(_LoadLevel(levelName));
    }

    IEnumerator _LoadLevel(int levelIndex)
    {
        transition.SetTrigger("startTransition");

        yield return new WaitForSeconds(this.transition_time);

        SceneManager.LoadScene(levelIndex);
    }

    IEnumerator _LoadLevel(string levelName)
    {
        transition.SetTrigger("startTransition");

        yield return new WaitForSeconds(this.transition_time);

        SceneManager.LoadScene(levelName);
    }
}
