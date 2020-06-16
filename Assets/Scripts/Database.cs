using Proyecto26;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour
{
    public static string databaseUrl = "https://pozharka-3206f.firebaseio.com/";

    public static void SendToDatabase(UserData userData, string separator)
    {
        RestClient.Put<UserData>(databaseUrl + separator + ".json", userData);
    }
}
