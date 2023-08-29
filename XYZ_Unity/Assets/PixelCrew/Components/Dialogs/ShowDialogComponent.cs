using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrew.Components;
using UnityEngine;
using UnityEngine.Events;
using static System.Collections.Specialized.BitVector32;

public class ShowDialogComponent : MonoBehaviour
{
    [SerializeField] private Mode _mode;
    [SerializeField] private DialogData _bound;
    [SerializeField] private DialogDef _external;


    private DialogBoxController _dialogBox;



    public void Show()
    {
        if (_dialogBox == null)
            _dialogBox = FindObjectOfType<DialogBoxController>();
        _dialogBox.SetDialogSpeaker(gameObject.GetComponent<InteractableComponent>());
        _dialogBox.ShowDialog(Data);
    }

    public void Show(DialogDef def)
    {
        _external = def;
        Show();
    }


    public DialogData Data
    {
        get
        {
            switch (_mode)
            {
                    case Mode.Bound:
                        return _bound;
                    case Mode.External:
                        return _external.Data;
                    default:
                        throw new ArgumentOutOfRangeException();
            }          

        }
    } 


    public enum Mode
    {
        Bound,
        External
    }
}


