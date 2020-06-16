using Proyecto26;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadScene(int numScene)
    {
        SceneManager.LoadScene(numScene);
    }

    public void ExitGame()
    {
        
        if (Auth.localID != null)
        {
            RestClient.Get<UserData>(Database.databaseUrl + Auth.localID + ".json", GetUserDataCallback);
        }
        else
        {
            Application.Quit();
        }
    }

    private void GetUserDataCallback(RequestException arg1, ResponseHelper arg2, UserData data)
    {
        try
        {
            int receivedTime = data.timeGame;
            receivedTime += Mathf.RoundToInt(TimeInGame.currentTime);
            data.timeGame = receivedTime;
            RestClient.Put<UserData>(Database.databaseUrl + Auth.localID + ".json", data, sendDataCallback);
        }
        catch (Exception)
        {
            Application.Quit();
        }
    }

    private void sendDataCallback(RequestException arg1, ResponseHelper arg2, UserData data)
    {
        try
        {
            string id = data.localID;
            TimeInGame.currentTime = 0;
            Application.Quit();
        }
        catch (Exception)
        {
            Application.Quit();
        }
    }

    public void LoadPage(string url)
    {
        Application.OpenURL(url);
    }
}
