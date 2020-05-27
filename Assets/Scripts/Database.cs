using Proyecto26;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour
{
    public static string _database = "https://pozharka-3206f.firebaseio.com/";

    public static void SendToDatabase(UserData userData, string separator)
    {
        RestClient.Put<UserData>(_database + separator + ".json", userData);
    }

    public static UserData ReceiveFromDatabase()
    {
        RestClient.Get<UserData>(_database + Auth.userEmail + ".json").Then(response => {
            return response;
        });

        return null;
    }
}
