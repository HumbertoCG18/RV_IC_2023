using System;

public interface ITriggableObject
{
    void SetCallback(Action callback);
    void Trigger();
}