using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitable : MonoBehaviour, IObservable<Hitable>
{
    [SerializeField]
    protected Rigidbody2D rigidBody;

    public Rigidbody2D GetRigidBody { get => rigidBody; private set => rigidBody = value; }

    List<IObserver<Hitable>> observers = new List<IObserver<Hitable>>();

    protected void Notify()
    {
        if (observers.Count < 1)
            return;
        foreach (var observer in observers)
            observer.UpdateObserver();
    }

    protected void ClearObservers()
    {
        if (observers.Count < 1)
            return;
        foreach (var observer in observers)
            observer.StopObserver();
        observers.Clear();
    }

    public void AddObserver(IObserver<Hitable> observer)
    {
        observers.Add(observer);
        observer.StartObserver(this);
    }

    public void RemoveObserver(IObserver<Hitable> observer)
    {
        observer.StopObserver();
        observers.Remove(observer);
    }
}
