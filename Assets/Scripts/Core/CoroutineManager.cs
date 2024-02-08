using System.Collections;
using UnityEngine;

public class CoroutineManager
{
    private static CoroutineManager instance;
    private MonoBehaviour coroutineExecutor;

    private CoroutineManager()
    {
        GameObject coroutineObject = new GameObject("CoroutineExecutor");
        GameObject.DontDestroyOnLoad(coroutineObject);
        coroutineExecutor = coroutineObject.AddComponent<CoroutineExecutor>();
    }

    public static CoroutineManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CoroutineManager();
            }
            return instance;
        }
    }

    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return coroutineExecutor.StartCoroutine(routine);
    }

    public void StopCoroutine(Coroutine routine)
    {
        coroutineExecutor.StopCoroutine(routine);
    }
}

public class CoroutineExecutor : MonoBehaviour { }
