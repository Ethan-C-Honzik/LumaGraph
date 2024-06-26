﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI.Extensions;
using UnityEngine.UI;

[RequireComponent(typeof(AutoCompleteComboBox))]
public class AutoCompAddons : MonoBehaviour
{
    //this event is made to match the OnValueChanged event to update the options
    public class valChangedEvent : UnityEvent<string, bool> { }
    public valChangedEvent onMoved = new valChangedEvent();
    public Transform optionsParent;
    public InputField inputField;
    public Text placeholder;
    public Text IFText;
    public Text MoveText;

    private bool moving;

    string[] _panelItems;
    int index = 0;
    private void OnMove(Vector2 dir)
    {
        _panelItems = GetItems();
        if (_panelItems.Length == 0) return;
        //string deb = "";
        //foreach (var item in _panelItems)
        //{
        //    deb += ("\t " + item);
        //}
        //Debug.Log(deb + _panelItems.Length);

        IFText.color = new Color(IFText.color.r, IFText.color.g, IFText.color.b, 0);
        MoveText.color = new Color(MoveText.color.r, MoveText.color.g, MoveText.color.b, 1);
        placeholder.color = new Color(placeholder.color.r, placeholder.color.g, placeholder.color.b, 0);

        if (moving)
        {
            MoveText.text = "";
        }

        //this will stop it from rebuilding the menu and list based on search terms
        //SetPaused()true;
        if (_panelItems.Length > 0)
        {
            //if (!box.pauseUpdate)
            //    index = 0;
            //box.pauseUpdate = true;
            //box.OnValueChanged(_panelItems[0]);
            //MoveText.text = _panelItems[0];
        }
        if (dir.y < 0)
        {
            //int index = _panelItems.FindIndex(x => x == box.Text);
            if (index < _panelItems.Length - 1)
            {
                index++;
                if (!moving)
                    index = 0;
                moving = true;
                onMoved.Invoke(_panelItems[index], false);
                MoveText.text = _panelItems[index];
            }
            else
            {
                index = 0;
                moving = true;
                onMoved.Invoke(_panelItems[index], false);
                MoveText.text = _panelItems[index];
            }
        }
        else
        {
            //int index = _panelItems.FindIndex(x => x == box.Text);
            if (index > 0)
            {
                index--;
                if (!moving)
                    index = 0;
                moving = true;
                onMoved.Invoke(_panelItems[index], false);
                MoveText.text = _panelItems[index];
            }
            else
            {
                index = _panelItems.Length - 1;
                moving = true;
                onMoved.Invoke(_panelItems[index], false);
                MoveText.text = _panelItems[index];
            }            
        }
    }

    public void SetInputField()
    {
        if (moving)
        {
            moving = false;
            inputField.text = MoveText.text;
            inputField.caretPosition = MoveText.text.Length + 1;
            GlobalInputDelagates.select.Invoke();
        }
    }

    void OnBack()
    {
        if (MoveText.text.Length > 0)
        {
            MoveText.text = MoveText.text.Substring(0, MoveText.text.Length - 1);
        }
        SetInputField();
    }

    private string[] GetItems()
    {
        Text[] items = optionsParent.GetComponentsInChildren<Text>(false);
        string[] names = new string[items.Length];
        for (int i = 0; i < items.Length; i++)
        {                        
            names[i] = items[i].text;
        }
        return names;
    }

    private void OnEnable()
    {
        index = 0;
        GlobalInputDelagates.move += OnMove;
        GlobalInputDelagates.select += SetInputField;
        GlobalInputDelagates.back += OnBack;
    }
    private void OnDisable()
    {
        GlobalInputDelagates.move -= OnMove;
        GlobalInputDelagates.select -= SetInputField;
        GlobalInputDelagates.back -= OnBack;
    }

    private void Update()
    {
        inputField.ActivateInputField();
        inputField.Select();
    }
}
