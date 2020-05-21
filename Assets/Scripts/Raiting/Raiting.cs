using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Raiting : MonoBehaviour
{
    [SerializeField] TodoList _todoList;

    [SerializeField] private GameObject _stars;

    [SerializeField] private Sprite _activeStar;
    [SerializeField] private Sprite _disabledStar;

    private float _countStars;

    private void Start()
    {
        _countStars = _stars.transform.childCount;

        TodoList.OnTaskFailed += TodoList_OnTaskFailed;
        TodoList.OnAllTaskDone += TodoList_OnAllTaskDone;
    }

    private void TodoList_OnAllTaskDone()
    {
        DisplayStars();
        SaveRaiting();
    }

    private void OnDestroy()
    {
        TodoList.OnTaskFailed -= TodoList_OnTaskFailed;
        TodoList.OnAllTaskDone -= TodoList_OnAllTaskDone;
    }

    private void TodoList_OnTaskFailed(ItemConfig obj)
    {
        DecreaseStar();
    }

    private void DecreaseStar()
    {
        float temp = _countStars - 1 / (float)_todoList.TodoListArr.Length;
        if (temp > 0)
            _countStars = _countStars - 1 / (float)_todoList.TodoListArr.Length;
        else
            _countStars = 0;
    }

    private void DisplayStars()
    {
        for (int i = 0; i < _stars.transform.childCount; i++)
        {
            GameObject starObj = _stars.transform.GetChild(i).gameObject;
            Image star = starObj.transform.GetChild(1).GetComponent<Image>();
            
            if (i < Mathf.FloorToInt(_countStars))
            {
                starObj.transform.GetChild(0).gameObject.SetActive(true);
                star.sprite = _activeStar;
            }
            else
            {
                starObj.transform.GetChild(0).gameObject.SetActive(false);
                star.sprite = _disabledStar;
            }
        }
    }

    private void SaveRaiting()
    {
        LevelsRaiting levelsRaiting = GetLevelsRaiting();

        string sceneName = SceneManager.GetActiveScene().name;
        string[] arr = sceneName.Split('-');

        int numPart = int.Parse(arr[0]);
        int numLevel = int.Parse(arr[1]);

        LevelRaiting lvlRaiting = GetLevelRaiting(levelsRaiting.levelsRaitingList, numPart, numLevel);

        if (lvlRaiting != null)
        {
            if (Mathf.FloorToInt(_countStars) > lvlRaiting.countStars)
                lvlRaiting.countStars = Mathf.FloorToInt(_countStars);
        }
        else
        {
            levelsRaiting.levelsRaitingList.Add(new LevelRaiting(numPart, numLevel, Mathf.FloorToInt(_countStars)));
        }

        string data = JsonUtility.ToJson(levelsRaiting);
        PlayerPrefs.SetString("Raiting", data);
    }

    private LevelRaiting GetLevelRaiting(List<LevelRaiting> levelsRaitingList, int numPart, int numLevel)
    {
        for (int i = 0; i < levelsRaitingList.Count; i++)
        {
            if (levelsRaitingList[i].numPart == numPart)
            {
                if (levelsRaitingList[i].numLevel == numLevel)
                {
                    return levelsRaitingList[i];
                }
            }
        }
        return null;
    }

    private LevelsRaiting GetLevelsRaiting()
    {
        if (PlayerPrefs.HasKey("Raiting"))
            return JsonUtility.FromJson<LevelsRaiting>(PlayerPrefs.GetString("Raiting"));

        return new LevelsRaiting();
    }

}


[System.Serializable]
public class LevelsRaiting
{
    public List<LevelRaiting> levelsRaitingList;

    public LevelsRaiting()
    {
        levelsRaitingList = new List<LevelRaiting>();
    }
}


[System.Serializable]
public class LevelRaiting
{
    public int numPart;
    public int numLevel;
    public int countStars;

    public LevelRaiting(int numPart, int numLevel, int countStars)
    {
        this.numPart = numPart;
        this.numLevel = numLevel;
        this.countStars = countStars;
    }
}
