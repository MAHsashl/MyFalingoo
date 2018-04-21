﻿using System;
using Backtory.Core.Public;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using Backtory.InAppPurchase.Public;

public class Signup_Panel : MonoBehaviour
{

    const string usernameKey = "userKey";
    const string emailKey = "emailKey";
    const string passKey = "passKey";
    const string alreadyRegistered = "alreadyRegisteredKey";

    // Signup Panel
    public InputField usernameInputreg;
    public InputField emailInputreg;
    public InputField passwordInputreg;
    public Dropdown ageDropDown;
    public Dropdown sexDropDown;

    // Login Panel
    public InputField usernameInputlog;
    public InputField passwordInputlog;

    public void Start()
    {
#if UNITY_EDITOR
        PlayerPrefs.DeleteAll();
#endif

       // if(olduser){
          //  LoginProcess();
     //   }

    }


    public void onRegisterClick()
    {

        // First create a user and fill his/her data
        BacktoryUser newUser = new BacktoryUser
        {

            Username = usernameInputreg.text,
            Email = emailInputreg.text,
            Password = passwordInputreg.text,

        };
        // Registring user to backtory (in background)
        newUser.RegisterInBackground(response =>
        {
            // Checking result of operation
            if (response.Successful)
            {
                // save local
                PlayerPrefs.SetString(usernameKey, newUser.Username);
                PlayerPrefs.SetString(emailKey, newUser.Email);
                PlayerPrefs.SetString(passKey, newUser.Password);

                // register complated and we should login now
                LoginProcess(newUser.Username, newUser.Password,true);


            }
            else if (response.Code == (int)BacktoryHttpStatusCode.Conflict)
            {
                // Username is invalid
                Debug.Log("Bad username; a user with this username already exists.");
            }
            else
            {
                // General failure
                Debug.Log("Registration failed; for network or some other reasons.");
            }
        });
    }

    public void LogInClick()
    {
        string username = usernameInputlog.text; // TODO: Get username from loginUsernameInputField
        string pass = passwordInputlog.text; // TODO: Get username from loginUsernameInputField
        LoginProcess(username, pass, false);
    }

    public void LoginProcess(string username, string password,bool newUser)
    {
        BacktoryUser.LoginInBackground(username, password, loginResponse =>
        {

            // Login operation done (fail or success), handling it:
            if (loginResponse.Successful)
            {
                Debug.Log("Login Successful.");
                //We have UserId and if it is the first time that he logs in, we should send age and gender to Backtory.
                if (PlayerPrefs.GetInt(alreadyRegistered) != 1)
                {
                    if (newUser)
                    {
                        saveAgegen();
                    }
                    //If the user is a member of service and because of exchanging his phone or clearing his playerprefs' data,
                    //we can read his age/gen data locally. 
                    else
                    {
                       //TODO: LoadAgeGen()
                    }
                }

            }
            else if (loginResponse.Code == (int)BacktoryHttpStatusCode.Unauthorized)
            {
                // Username 'mohx' with password '123456' is wrong
                Debug.Log("Either username or password is wrong.");
            }
            else
            {
                // Operation generally failed, maybe internet connection issue
                Debug.Log("Login failed for other reasons like network issues.");
            }
        });

    }

    public void saveAgegen()
    {

        BacktoryObject genderage = new BacktoryObject("GenderAge");
        genderage["gender"] = sexDropDown.value;
        genderage["age"] = ageDropDown.value;
        genderage["userID"] = BacktoryUser.CurrentUser.UserId;


        genderage.SaveInBackground(response =>
        {
            if (response.Successful)
            {
                PlayerPrefs.SetInt(alreadyRegistered, 1);
                // successful save. good place for Debug.Log function.

            }
            else
            {
                // see response.Message to know the cause of error
            }
        });
    }
}
