
public interface IObserver<T>
{
    //observers should have ability to (un)subscribe by themselves to target class if they have a reference
    public void UpdateObserver();
    public void StartObserver(T reference);
    public void StopObserver();
}

