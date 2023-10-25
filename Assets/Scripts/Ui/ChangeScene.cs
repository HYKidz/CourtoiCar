using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    // [SerializeField] private string _scene;
    public void Change(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
