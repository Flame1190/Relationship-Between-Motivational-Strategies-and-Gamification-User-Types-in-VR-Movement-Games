using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ExperienceController : MonoBehaviour
{
    [SerializeField]
    bool _writeResultsToFile;


    public float CurrentScore;
    float _userID;

    public string _experienceType = "TEST";
    private void Start()
    {
        SetUserID();
    }

    public void SetUserID()
    {

        _userID = UserInformation.UserID;
    }

    public void FinishExperience()
    {
        print("Experience Complete");

        // SAVE DATA
        if (_writeResultsToFile)
        {
            SaveExperienceData();
        }
    }

    void SaveExperienceData()
    {
        string filePath = getPath()[0];
        string fileName = getPath()[1];
        //check if directory doesn't exit
        if (!Directory.Exists(filePath))
        {
            //if it doesn't, create it
            Directory.CreateDirectory(filePath);

        }
        if (!File.Exists(filePath + fileName))
        {
            string fileHeader = "User ID" + "," + "Score" + "," + "Experience Type" +"," + "User Height" + "," + "User Reach" + Environment.NewLine;

            File.WriteAllText(filePath + fileName, fileHeader);
        }
        print("user id on close:" + UserInformation.UserID.ToString());

        //StreamWriter writer = new StreamWriter(filePath + fileName);
        File.AppendAllText(filePath + fileName, Environment.NewLine + UserInformation.UserID.ToString() + "," + CurrentScore.ToString() + "," + _experienceType + "," + UserInformation.height + "," + UserInformation.reach);
        //writer.WriteLine("User ID,Score");
        //writer.WriteLine(_userID.ToString() + "," + CurrentScore.ToString());
        //writer.Flush();
        //writer.Close();
        Destroy(this);
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }

   
    void OnDestroy()
    {
        print("Scene Unloaded");

        FinishExperience();
    }
    private string[] getPath()
    {
#if UNITY_EDITOR
        return new string[] { Application.dataPath + "/Data/", "ParticipantInformation.csv" };
        //"Participant " + "   " + DateTime.Now.ToString("dd-MM-yy   hh-mm-ss") + ".csv";
#elif UNITY_ANDROID
        return new string[] { Application.persistentDataPath+"ParticipantInformation.csv"};
#elif UNITY_IPHONE
        return new string[] { Application.persistentDataPath+"/"+"ParticipantInformation.csv"};
#else
        return new string[] { Application.dataPath +"/","ParticipantInformation.csv"};
#endif
    }
}
