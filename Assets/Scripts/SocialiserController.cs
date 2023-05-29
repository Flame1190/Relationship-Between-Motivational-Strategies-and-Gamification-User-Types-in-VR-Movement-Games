using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SocialiserController : ExperienceController
{
    [SerializeField]
    TMP_Text _scoreText;




    public static SocialiserController _instance;
    private void Awake()
    {
        if (_instance == null || _instance == this)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    public void OnScore()
    {
        CurrentScore++;
        _scoreText.text = "Current Score: " + CurrentScore.ToString();
    }
    
}
