using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "User Info", menuName = "User/UserInfo")]
public class UserInfo : ScriptableObject {

    public string userName;
    
    public ulong userID;

    /// <summary>
    /// If the user has an avatar configured
    /// </summary>
    public bool hasAvatarConfigured;

    public void SetHasNonDefaultAvatar(bool newValue) {
        hasAvatarConfigured = newValue;
    }

    public void SetUserName(string newName) {
        userName = newName;
    }
    
    public void SetUserID(ulong newID) {
        userID = newID;
    }

    /// <summary>
    /// When no info is provided, user name is randomized and id is set to 0
    /// </summary>
    public void OnNoInfoProvided() {
        userName = "User" + Random.Range(0,1000);
        userID = 0;
        hasAvatarConfigured = false;
    }
}
