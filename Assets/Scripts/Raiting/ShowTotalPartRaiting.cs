using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ShowTotalPartRaiting : MonoBehaviour
{
    [SerializeField] private Text _totalStarsText;
    [SerializeField] private int _totalStarsCount;
    [SerializeField] private int _numPart;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("Raiting")) {

            _totalStarsText.text = $"0/{_totalStarsCount}";
            return;
        }
        string data = PlayerPrefs.GetString("Raiting");
        LevelsRaiting lvlsRaiting = JsonUtility.FromJson<LevelsRaiting>(data);

        List<LevelRaiting> lvlList =  lvlsRaiting.levelsRaitingList.Where(x => x.numPart == _numPart).ToList();

        int currentCountStars = 0;

        for (int i = 0; i < lvlList.Count; i++)
        {
            currentCountStars += lvlList[i].countStars;
        }

        _totalStarsText.text = $"{currentCountStars}/{_totalStarsCount}";
    }


}
