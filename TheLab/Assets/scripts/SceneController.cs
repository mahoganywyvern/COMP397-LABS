using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    public void ChangeSceneName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void ChangeScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}