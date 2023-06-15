using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class FlatScreenCanvasController : MonoBehaviour
{
    string _alienName = "Unidentified Alien";
    [SerializeField]
    TMP_Text _alienNameText; 
    public void UpdateAlienName(string name)
    {
        _alienName = name;
        _alienNameText.text = _alienName;
    }
    public void ToggleVisibility(GameObject toToggle)
    {
        toToggle.SetActive(!toToggle.activeSelf);
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}
