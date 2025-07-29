using System.Collections;
using UnityEngine;

public class CoroutineRunner : MonoBehaviour 
{
    private static CoroutineRunner _instance;
    public static CoroutineRunner Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        
        Debug.Log($"_instance: "+ _instance);
    }

    public void RunCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}
