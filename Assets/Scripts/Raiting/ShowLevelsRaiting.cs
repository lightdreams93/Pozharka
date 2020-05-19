using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ShowLevelsRaiting : MonoBehaviour
{
    [SerializeField] private int _numPart;
    
    [SerializeField] private GameObject _levelsContainer;

    [SerializeField] private Sprite _activeStar;
    [SerializeField] private Sprite _disableStar;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("Raiting")) return;

        string data = PlayerPrefs.GetString("Raiting");
        LevelsRaiting lvlsRaiting = JsonUtility.FromJson<LevelsRaiting>(data);

        List<LevelRaiting> lvlRaitingList = lvlsRaiting.levelsRaitingList.Where(x => x.numPart == _numPart).ToList();

        for (int i = 0; i < lvlRaitingList.Count; i++)
        {
            for (int j = 0; j < lvlRaitingList[i].countStars; j++)
            {
                _levelsContainer.transform.GetChild(lvlRaitingList[i].numLevel - 1).GetChild(1).transform.GetChild(j).GetChild(1).GetComponent<Image>().sprite = _activeStar;
                _levelsContainer.transform.GetChild(lvlRaitingList[i].numLevel - 1).GetChild(1).transform.GetChild(j).GetChild(0).gameObject.SetActive(true);
            }
        }
    }
}
