using System.Collections;
using UnityEngine;

public static class InvokeHelper
{
    public static Coroutine InvokeAfterSeconds(MonoBehaviour behaviour, System.Action action, float seconds)
    {
        return behaviour.StartCoroutine(InvokeCoroutine(action, seconds));
    }

    private static IEnumerator InvokeCoroutine(System.Action action, float seconds)
    {
         WaitForSeconds wait = new WaitForSeconds(seconds);
        yield return wait;
        action?.Invoke();
    }
}
