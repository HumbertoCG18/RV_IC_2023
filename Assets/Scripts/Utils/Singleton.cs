using UnityEngine;

// Script to create Singleton Classes
// Date: 01/03/2023
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T _Instance;
    public static T Instance
    {
        get
        {
            if (_Instance == null)
                _Instance = FindObjectOfType<T>();

            return _Instance;
        }
    }
}