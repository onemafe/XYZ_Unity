using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogData
{
    [SerializeField] private string[] _sentences;
    public string[] Sentences => _sentences;
}
