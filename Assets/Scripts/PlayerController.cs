using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PlayerController : ExperienceController
{

    [SerializeField]
    TMP_Text _scoreText;

    [SerializeField]
    TMP_Text _currRankText;

    [SerializeField]
    Image _currRankImage;

    [SerializeField]
    TMP_Text _nextRankText;


    SphereSpawner _sphereSpawn;

    int _nextGoalThreshold = 5;
    int _previousGoalThreshold = 0;
    int _currGoalIndex = 0;

    [SerializeField]
    ScoreGoal[] _scoreGoals;

    [SerializeField]
    Badge[] _scoreBadges;

    int _currentRightHandScore = 0;
    int _currentLeftHandScore = 0;

    float _timeElapsed = 0;

    float _tenScoreTime;

    float[] _pastTenScoreTimes = new float[10];

    [SerializeField]
    int _timedBadgeIndex;

    [SerializeField]
    Transform _badgesUIParent;

    [SerializeField]
    GameObject _badgeUITemplate;

    [SerializeField]
    Color _badgeLockedColour;

    List<GameObject> _badgeUI = new List<GameObject>();

    [SerializeField]
    Scrollbar _rankProgressUI;

    [SerializeField]
    TMP_Text _rankProgressText;

    #region Singleton Setup
    public static PlayerController _instance;
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


    private void Update()
    {
        _timeElapsed += Time.deltaTime;
    }

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


        foreach (Badge currBadge in _scoreBadges)
        {
            GameObject thisBadge = Instantiate(_badgeUITemplate, _badgesUIParent);
            thisBadge.transform.Find("Title").GetComponent<TMP_Text>().text = currBadge.BadgeName;
            thisBadge.transform.Find("Description").GetComponent<TMP_Text>().text = currBadge.BadgeDescription;
            thisBadge.transform.Find("Image").GetComponent<Image>().color = _badgeLockedColour;

            
            if (currBadge.BadgeIcon != null)
            {
                thisBadge.transform.Find("Image").GetComponent<Image>().sprite = currBadge.BadgeIcon;

            }

            _badgeUI.Add(thisBadge);
        }

    }

    public void OnScore(OVRHand.Hand hand)
    {
        CurrentScore++;
        if (hand == OVRHand.Hand.HandLeft)
        {
            _currentLeftHandScore++;
        } else
        {
            _currentRightHandScore++;
        }
        _scoreText.text = "Current Score: " + CurrentScore.ToString();
        _sphereSpawn.SpawnObject();
     
        
        for (int i = 0; i < _scoreBadges.Length; i++) 
        {
            if (_scoreBadges[i].ScoreThreshold != -1)
            {
                if (_scoreBadges[i].Completed != true && CurrentScore >= _scoreBadges[i].ScoreThreshold && _currentLeftHandScore >= _scoreBadges[i].LeftHandScoreThreshold && _currentRightHandScore >= _scoreBadges[i].RightHandScoreThreshold)
                {
                    _scoreBadges[i].Completed = true;
                    _badgeUI[i].transform.Find("Image").GetComponent<Image>().color = Color.white;
                }
            }
        }

        for (int i = 0; i < 9; i++)
        {
            _pastTenScoreTimes[i] = _pastTenScoreTimes[i + 1];
        }
        _pastTenScoreTimes[9] = _timeElapsed;

        if (CurrentScore >= 10 && !_scoreBadges[_timedBadgeIndex].Completed && _timeElapsed - _pastTenScoreTimes[0] <= 10)
        {
            _scoreBadges[_timedBadgeIndex].Completed = true;
            _badgeUI[_timedBadgeIndex].transform.Find("Image").GetComponent<Image>().color = Color.white;

        }
        
        if (CurrentScore >= _nextGoalThreshold)
        {

            if (_currGoalIndex < _scoreGoals.Length - 1)
            {
                _previousGoalThreshold = _nextGoalThreshold;
                _currGoalIndex++;
                if (_currGoalIndex < _scoreGoals.Length - 1)
                {
                    _nextGoalThreshold = _scoreGoals[_currGoalIndex + 1].ScoreThreshold;
                    _nextRankText.text = "Next Rank: " + _scoreGoals[_currGoalIndex + 1].GoalName + " (" + _scoreGoals[_currGoalIndex + 1].ScoreThreshold + " points)";
                }
                else
                {
                    _nextRankText.text = "Congratulations, you've completed all the ranks!";
                    _currRankText.color = Color.yellow;
                    _rankProgressUI.value = 1;
                    _rankProgressText.text = "100%";

                }
                _currRankText.text = "Current Rank: " + _scoreGoals[_currGoalIndex].GoalName;
                _currRankImage.sprite = _scoreGoals[_currGoalIndex].GoalIcon;
            }
           

        }
        if (_currGoalIndex < _scoreGoals.Length - 1)
        {
            _rankProgressUI.value = ((float)CurrentScore - _previousGoalThreshold) / (_nextGoalThreshold - _previousGoalThreshold);
            _rankProgressText.text = (Mathf.Round((((float)CurrentScore - _previousGoalThreshold) * 100 / (_nextGoalThreshold - _previousGoalThreshold)) * 100) / 100).ToString() + "%";
        }
    }
}


[System.Serializable]
public class Badge
{
    public string BadgeName;
    [TextArea(5, 5)]
    public string BadgeDescription;
    // Score of -1 if this badge is not obtained by score

    public int ScoreThreshold;
    public int RightHandScoreThreshold;
    public int LeftHandScoreThreshold;

    // True if the user has obtained this badge already
    public bool Completed = false;

    public Sprite BadgeIcon;


}
