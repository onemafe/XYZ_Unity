using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntProperty : PersistentProperty<float>
{
    public IntProperty(float defaultValue) : base(defaultValue)
    {

    }

    protected override float Read(float defaultValue)
    {
        return _value;
    }

    protected override void Write(float value)
    {
        _value = value;
    }

}
