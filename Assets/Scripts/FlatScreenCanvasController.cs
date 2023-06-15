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

    [SerializeField]
    TMP_InputField _userIDInput;

    private void Start()
    {
        if (_userIDInput != null)
        _userIDInput.text = UserInformation.UserID.ToString(); 
    }
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

    public void ChangeUSERID (string userID)
    {
        UserInformation.UserID = int.Parse(userID);
        print("UserID" + int.Parse(userID).ToString());
    }

}
