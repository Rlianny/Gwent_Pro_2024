using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Subject : MonoBehaviour
{
    protected List<IObserver> observers = new List<IObserver>();

    public void AddObserver(IObserver observer)
    {
        observers.Add(observer);
    } 

    public void RemoveObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    protected void NotifyObservers(System.Enum action, Card card = null)
    {
        foreach(IObserver observer in observers)
        {
            observer.OnNotify(action, card);
        }
    }
}
