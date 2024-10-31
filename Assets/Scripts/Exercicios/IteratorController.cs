using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class IteratorController<T>
{
    [SerializeField] private List<T> _values;

    private int _currentIndex = 0;

    public IteratorController(List<T> values)
    {
        _values = values;
        _currentIndex = 0;
    }

    public bool Next()
    {
        if (Index < Count - 1)
        {
            _currentIndex++;
            return true;
        }

        return false;
    }

    public bool Previous()
    {
        if (Index > 0)
        {
            _currentIndex--;

            return true;
        }

        return false;
    }

    public void Reset()
    {
        _currentIndex = 0;
    }

    public void SetIndex(int index)
    {
        _currentIndex = index;
    }


    public bool IsFirst => Index == 0;
    public bool IsLast => Index == Count - 1;

    public List<T> Values => _values;

    public int Count => _values.Count;

    public int Index => _currentIndex;

    public T Current => _values[_currentIndex];
}
