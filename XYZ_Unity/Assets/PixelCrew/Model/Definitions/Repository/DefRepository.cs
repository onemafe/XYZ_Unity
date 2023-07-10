using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefRepository<TDefType> : ScriptableObject where TDefType : IHaveId //Ограничили посылаемые объекты
{
    [SerializeField] protected TDefType[] _collection;

    public TDefType Get(string id)
    {
        if (string.IsNullOrEmpty(id))
            return default;

        foreach (var ThrowableDef in _collection)
        {
            if (ThrowableDef.Id == id)
                return ThrowableDef;
        }

        return default;
    }
}
