using Proyecto26;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class Auth : MonoBehaviour
{
    [SerializeField] private TMP_InputField _signInEmail;
    [SerializeField] private TMP_InputField _signInPassword;

    public TMP_InputField _signUpSurname;
    public TMP_InputField _signUpName;
    public TMP_InputField _signUpKeyword;
    public TMP_InputField _signUpEmail;
    public TMP_InputField _signUpPassword;

    [SerializeField] private TMP_InputField _forgotEmail;

    [SerializeField] private Text _logText;

    [SerializeField] private UnityEvent OnSignIn;

    private string AUTH_KEY = "AIzaSyAoSC7Am0AA7_vX4asj0psO2lx4xmCJhzc";
    
    public static string localID;
    public static string userEmail;

    [SerializeField] private User[] _users; 

    [SerializeField] private bool _generateMode; 

    public User[] Users => _users;

    void Start()
    {
        if(_generateMode)
        {
            StartCoroutine(StartAuth());
        }
    }

    IEnumerator StartAuth()
    {
        for (int i = 0; i < _users.Length; i++)
        {
            _signUpEmail.text = _users[i].email;
            _signUpKeyword.text = "students";
            _signUpSurname.text = _users[i].userSurname;
            _signUpName.text = _users[i].userName;
            _signUpPassword.text = "123456";

            SignUp();

            yield return new WaitForSeconds(5);
        }

        Debug.Log("Успех!!");

        yield return null;
    }


    public void SignIn()
    {
        string email = _signInEmail.text;
        string pass = _signInPassword.text;

        SignIn(email, pass);
    }


    public void SignIn(string email, string pass)
    {
        _logText.text = "Подождите идет вход в аккаунт...";

        string data = "{\"email\":\"" + email + "\",\"password\":\"" + pass + "\",\"returnSecureToken\":true}";
        userEmail = email;
        RestClient.Post<AuthData>($"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={AUTH_KEY}", data, SignInCallback);
    }

    private void SignInCallback(RequestException exception, ResponseHelper helper, AuthData data)
    {
        try
        {
            localID = data.localId;
            _logText.text = "Успешно!";
            RestClient.Get<UserData>(Database.databaseUrl + localID + ".json", RequestCallback);
            if (!_generateMode)
                OnSignIn?.Invoke();
        }
        catch (Exception)
        {
            _logText.text = exception.Message;
        }
    }

    private void RequestCallback(RequestException exception, ResponseHelper arg2, UserData data)
    {
        try
        {
            data.localID = localID;
            Database.SendToDatabase(data, localID);
        }
        catch (Exception)
        {
            _logText.text = exception.Message;
        }
    }

    public void SignUp()
    {
        string email = _signUpEmail.text;
        string pass = _signUpPassword.text;

        string userSurname = _signUpSurname.text;
        string userName = _signUpName.text;
        string keyWord = _signUpKeyword.text;

        bool isEmailEmpty = string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email);
        bool isPassEmpty = string.IsNullOrEmpty(pass) || string.IsNullOrWhiteSpace(pass);
        bool isSurnameEmpty = string.IsNullOrEmpty(userSurname) || string.IsNullOrWhiteSpace(userSurname);
        bool isNameEmpty = string.IsNullOrEmpty(userName) || string.IsNullOrWhiteSpace(userName);
        bool isKeywordEmpty = string.IsNullOrEmpty(keyWord) || string.IsNullOrWhiteSpace(keyWord);

        if (isEmailEmpty || isPassEmpty || isSurnameEmpty || isNameEmpty || isKeywordEmpty)
        {
            _logText.text = "Заполните пожалуйста все поля!";
            return;
        }

        _logText.text = "Подождите идет регистрация...";

        string data = "{\"email\":\"" + email + "\",\"password\":\"" + pass + "\",\"returnSecureToken\":true}";

        RestClient.Post<AuthData>($"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={AUTH_KEY}", data, SignUpCallback);
    }

    private void SignUpCallback(RequestException exception, ResponseHelper helper, AuthData data)
    {
        try
        {
            string email = _signUpEmail.text;
            string pass = _signUpPassword.text;

            string userSurname = _signUpSurname.text;
            string userName = _signUpName.text;
            string keyWord = _signUpKeyword.text;

            UserData userData = new UserData(_signUpName.text, _signUpSurname.text, _signUpKeyword.text);
            userData.email = email;

            if(_generateMode)
            {
                userData.timeGame = UnityEngine.Random.Range(20 * 60, 30 * 60);
            }
            
            Database.SendToDatabase(userData, data.localId);

            SignIn(email, pass);
        }
        catch (Exception ex)
        {
            _logText.text = exception.Message;
        }
    }

}


public class AuthData
{
    public string localId;
}
