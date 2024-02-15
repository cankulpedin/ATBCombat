using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    private List<IPauseObserver> observers = new List<IPauseObserver>();

    // Dictionary<Outcome, bool> worldEvents;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Instantiate all world events false initially
        // TODO get world events from save file
        
    }

    private void Update()
    {
        /*
        foreach(var pair in worldEvents)
        {
            Debug.Log($"Key: {pair.Key}, Value: {pair.Value}");
        }*/
    }

    public void RegisterObserver(IPauseObserver observer)
    {
        observers.Add(observer);
    }

    public void UnregisterObserver(IPauseObserver observer)
    {
        observers.Remove(observer);
    }

    private void NotifyGamePause()
    {
        foreach (var observer in observers)
        {
            observer.NotifyPause();
        }
    }

    private void NotifyGameUnpause()
    {
        foreach (var observer in observers)
        {
            observer.NotifyUnpause();
        }
    }

    public void PauseGame()
    {
        NotifyGamePause();
    }

    public void UnpauseGame()
    {
        NotifyGameUnpause();
    }
}
