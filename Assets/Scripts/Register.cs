using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Register : MonoBehaviour
{
    public InputField inputname;
    public void GoToScene(string LevelName)
    {
        PlayerPrefs.SetString("UserName", inputname.text);
        SceneManager.LoadScene(LevelName);
    }
}