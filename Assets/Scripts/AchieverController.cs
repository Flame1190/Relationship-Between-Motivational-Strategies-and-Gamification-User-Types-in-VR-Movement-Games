using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class AchieverController : ExperienceController
{

    [SerializeField]
    TMP_Text _scoreText;

    [SerializeField]
    TMP_Text _currRankText;

    [SerializeField]
    Image _currRankImage;

    [SerializeField]
    TMP_Text _nextRankText;

    [SerializeField]
    TMP_Text _highScoreText;

    SphereSpawner _sphereSpawn;

    int _nextGoalThreshold = 5;
    int _currGoalIndex = 0;

    [SerializeField]
    ScoreGoal[] _scoreGoals;

    [SerializeField]
    int _highScore;


    #region Singleton Setup
    public static AchieverController _instance;
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
    #endregion

    private void Start()
    {
        _sphereSpawn = GetComponent<SphereSpawner>();
        _nextGoalThreshold = _scoreGoals[_currGoalIndex+1].ScoreThreshold;
        _currRankText.text = "Current Rank: " + _scoreGoals[_currGoalIndex].GoalName;
        _currRankImage.sprite = _scoreGoals[_currGoalIndex].GoalIcon;
        if (_currGoalIndex < _scoreGoals.Length - 1)
            _nextRankText.text = "Next Rank: " + _scoreGoals[_currGoalIndex+1].GoalName + " (" + _scoreGoals[_currGoalIndex + 1].ScoreThreshold + " points)";
        else
            _nextRankText.text = "";

        _highScoreText.text = "High Score: " + _highScore.ToString();
    }

    public void OnScore()
    {
        CurrentScore++;
        _scoreText.text = "Current Score: " + CurrentScore.ToString();
        _sphereSpawn.SpawnObject();
        if (CurrentScore > _highScore)
        {
            _highScoreText.text = "High Score: " + CurrentScore.ToString();
        }
        if (CurrentScore >= _nextGoalThreshold)
        {
            
            

            if (_currGoalIndex < _scoreGoals.Length - 1)
            {
                _currGoalIndex++;
                _nextGoalThreshold = _scoreGoals[_currGoalIndex+1].ScoreThreshold;

                _nextRankText.text = "Next Rank: " + _scoreGoals[_currGoalIndex + 1].GoalName + " (" + _scoreGoals[_currGoalIndex + 1].ScoreThreshold + " points)";
            }
            else
            {
                _nextRankText.text = "Try beat the highscore of " + _highScore.ToString() + "!";
                _currRankText.color = Color.yellow;
            }
            _currRankText.text = "Current Rank: " + _scoreGoals[_currGoalIndex].GoalName;
            _currRankImage.sprite = _scoreGoals[_currGoalIndex].GoalIcon;
        }
    }
}


[System.Serializable]
public class ScoreGoal
{
    public string GoalName;

    public int ScoreThreshold;
    public Sprite GoalIcon;
}
