using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeSettings : MonoBehaviour
{
    public static bool onlineMode;

    public void SetOnlineMode()
    {
        onlineMode = true;
    }

    public void SetOfflineMode()
    {
        onlineMode = false;
    }
}
