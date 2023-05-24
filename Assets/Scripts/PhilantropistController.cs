using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PhilantropistController : ExperienceController
{

    [SerializeField]
    string[] _potentialTools;
    public string CorrectTool;
    [SerializeField]
    GameObject[] _potentialToolsGameObjects;
    GameObject[] _currSelectionTools = new GameObject[3];

    [SerializeField]
    MeshRenderer _speechBubble;

    public Animator DisplayAstronaut;

    public static PhilantropistController _instance;
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


    /// <summary> Returns an array of random unique elements from the specified array. </summary>
    public static T[] GetRandomArray<T>(T[] array, int size)
    {
        List<T> list = new List<T>();
        T element;
        int tries = 0;
        int maxTries = array.Length;

        while (tries < maxTries && list.Count < size)
        {
            element = array[UnityEngine.Random.Range(0, array.Length)];

            if (!list.Contains(element))
            {
                list.Add(element);
            }
            else
            {
                tries++;
            }
        }

        if (list.Count > 0)
        {
            return list.ToArray();
        }
        else
        {
            return null;
        }
    }


    private void Start()
    {
        CorrectAnswer();
    }

    public void CorrectAnswer()
    {
        DisplayAstronaut.SetTrigger("Correct");
        CurrentScore++;
        if (_currSelectionTools.Length > 0)
        {
            for (int i = 0; i < _currSelectionTools.Length; i++)
            {
                Destroy(_currSelectionTools[i]);
            }
        }
        //_currSelectionTools = GetRandomArray<GameObject>(_potentialToolsGameObjects, 3);
        GameObject[] tempArray = _potentialToolsGameObjects;
        Shuffle<GameObject>(tempArray);
        Array.Copy(tempArray, 0, _currSelectionTools, 0, 3);
       // Shuffle<GameObject>(_currSelectionTools);
        for (int i = -1; i < _currSelectionTools.Length-1; i++)
        {
            GameObject newObject = Instantiate(_currSelectionTools[i+1], new Vector3(0.375f * i, 1, .25f), Quaternion.identity);
            newObject.name = _currSelectionTools[i+1].name;
            _currSelectionTools[i + 1] = newObject;
        }
        GameObject correctObject = _currSelectionTools[(int)UnityEngine.Random.Range(0, _currSelectionTools.Length)];
        CorrectTool = correctObject.name;

        _speechBubble.material.SetTexture("_MainTex", correctObject.GetComponent<FruitController>().SpeechBubbleTexture);
    }

    public void Shuffle<T>(T[] toShuffle)
    {
        for (int i = 0; i < toShuffle.Length; i++)
        {
            int rnd = UnityEngine.Random.Range(0, toShuffle.Length);
            T tempGO = toShuffle[rnd];
            toShuffle[rnd] = toShuffle[i];
            toShuffle[i] = tempGO;
        }
        
    }

}
