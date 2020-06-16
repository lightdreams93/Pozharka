using Proyecto26;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeInGame : MonoBehaviour
{
    private static TimeInGame instance;

    public static float currentTime; 

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            if (instance != this)
                Destroy(gameObject);
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        { 
            if (Auth.localID != null)
            {
                RestClient.Get<UserData>(Database.databaseUrl + Auth.localID + ".json", GetUserDataCallback);
            }
        }   
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        { 
            if (Auth.localID != null)
            {
                RestClient.Get<UserData>(Database.databaseUrl + Auth.localID + ".json", GetUserDataCallback);
            }
        }
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
    }

   
    private void GetUserDataCallback(RequestException arg1, ResponseHelper arg2, UserData data)
    {
        try
        {
            int receivedTime = data.timeGame;
            receivedTime += Mathf.RoundToInt(currentTime);
            data.timeGame = receivedTime;
            RestClient.Put<UserData>(Database.databaseUrl + Auth.localID + ".json", data, sendDataCallback);
        }
        catch (Exception)
        {
            throw;
        }
    }

    private void sendDataCallback(RequestException arg1, ResponseHelper arg2, UserData data)
    {
        try
        {
            string id = data.localID;
            currentTime = 0;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
