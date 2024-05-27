using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonobehaviourExtensions : MonoBehaviour
{
    public static MonobehaviourExtensions Instance { get; private set; }

    private void Awake()
    {
        SetupInstance();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public static Coroutine DODelayed(Action action, float delay, bool unscaledTime = false)
    {
        return Instance.StartCoroutine(Instance.Delayed(action, delay, unscaledTime));
    }

    public static Coroutine WaitForInstruction(Action action, YieldInstruction yieldInstruction)
    {
        return Instance.StartCoroutine(Instance.WaitForInstuction(action, yieldInstruction));
    }

    public static Coroutine RunCoroutine<T1>(YieldInstruction yieldInstruction, Predicate<T1> breakInstruction, T1 arg, Action onYieldInstructionUpdateAction = null)
    {
        return Instance.StartCoroutine(Instance.DOCoroutine<T1>(yieldInstruction, breakInstruction, arg, onYieldInstructionUpdateAction));
    }

    public static void StopAllTasks() => Instance.StopAllCoroutines();

    private void SetupInstance()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;

            DontDestroyOnLoad(this);
        }
    }

    private IEnumerator Delayed(Action task, float delay, bool unscaledTime)
    {
        float timer = 0f;

        while (timer < delay)
        {
            timer += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

            yield return null;
        }
        task?.Invoke();
    }

    private IEnumerator WaitForInstuction(Action task, YieldInstruction yieldInstruction)
    {
        yield return yieldInstruction;
        task?.Invoke();
    }

    private IEnumerator DOCoroutine<T>(YieldInstruction yieldInstruction, Predicate<T> breakCondition, T breakArgument, Action onYieldInstructionUpdateAction = null)
    {
        while (!breakCondition(breakArgument))
        {
            onYieldInstructionUpdateAction?.Invoke();
            yield return yieldInstruction;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        base.StopAllCoroutines();
    }
}
