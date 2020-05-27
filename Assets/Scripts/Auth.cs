using Proyecto26;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Auth : MonoBehaviour
{
    [SerializeField] private TMP_InputField _signInEmail;
    [SerializeField] private TMP_InputField _signInPassword;

    [SerializeField] private TMP_InputField _signUpSurname;
    [SerializeField] private TMP_InputField _signUpName;
    [SerializeField] private TMP_InputField _signUpEmail;
    [SerializeField] private TMP_InputField _signUpPassword;

    [SerializeField] private TMP_InputField _forgotEmail;

    [SerializeField] private Text _logText;

    [SerializeField] private UnityEvent OnSignIn;

    private string AUTH_KEY = "AIzaSyAoSC7Am0AA7_vX4asj0psO2lx4xmCJhzc";
    
    public static string localID;
    public static string userEmail;

    public void SignIn()
    {
        string email = _signInEmail.text;
        string pass = _signInPassword.text;

        SignIn(email, pass);
    }


    private void SignIn(string email, string pass)
    {
        string data = "{\"email\":\"" + email + "\",\"password\":\"" + pass + "\",\"returnSecureToken\":true}";
        userEmail = email;
        Debug.Log($"Email {userEmail}");

        RestClient.Post<AuthData>($"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={AUTH_KEY}", data, SignInCallback);
    }

    private void SignInCallback(RequestException exception, ResponseHelper helper, AuthData data)
    {
        try
        {
            localID = data.localId;
            _logText.text = "Success";
            RestClient.Get<UserData>(Database._database + localID + ".json", RequestCallback);
            OnSignIn?.Invoke();
        }
        catch (Exception)
        {
            _logText.text = exception.Message;
        }
    }

    private void RequestCallback(RequestException arg1, ResponseHelper arg2, UserData data)
    {
        try
        {
            data.localID = localID;
            Database.SendToDatabase(data, localID);
        }
        catch (Exception)
        {

             
        }
    }

    public void SignUp()
    {
        string email = _signUpEmail.text;
        string pass = _signUpPassword.text;

        string data = "{\"email\":\"" + email + "\",\"password\":\"" + pass + "\",\"returnSecureToken\":true}";

        RestClient.Post<AuthData>($"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={AUTH_KEY}", data, SignUpCallback);
    }

    private void SignUpCallback(RequestException exception, ResponseHelper helper, AuthData data)
    {
        try
        {
            string email = _signUpEmail.text;
            string pass = _signUpPassword.text;

            UserData userData = new UserData(_signUpName.text, _signUpSurname.text);
            userData.email = email;

            Database.SendToDatabase(userData, data.localId);

            SignIn(email, pass);
        }
        catch (Exception)
        {
            _logText.text = exception.Message;
        }
    }
}

public class AuthData
{
    public string localId;
}
