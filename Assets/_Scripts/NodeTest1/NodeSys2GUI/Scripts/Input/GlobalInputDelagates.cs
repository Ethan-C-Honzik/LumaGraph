﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalInputDelagates
{
    public delegate void Vector2Del(Vector2 vector);
    public delegate void FloatDel(float num);
    public delegate void TriggerDel();

    public static FloatDel scroll;
    public static TriggerDel openMenu;
    public static TriggerDel back;
    public static TriggerDel delete;
    public static TriggerDel escape;
    public static TriggerDel select;
    public static Vector2Del move;
    public static TriggerDel panStart;
    public static TriggerDel pan;
    public static TriggerDel Undo;
    public static TriggerDel Redo;
    public static TriggerDel Save;
    public static TriggerDel Copy;
    public static TriggerDel Cut;
    public static TriggerDel Paste;
    public static TriggerDel Group;

    public static void InvokeScroll(float val)
    {
        if (scroll != null)
        {
            scroll.Invoke(val);
        }
    }
    public static void InvokeOpenMenu()
    {
        if (openMenu != null)
        {
            openMenu.Invoke();
        }
    }

    public static void InvokeBack()
    {
        if (back != null)
        {
            back.Invoke();
        }
    }

    public static void InvokeSelect()
    {
        if (select != null)
        {
            select.Invoke();
        }
    }

    public static void InvokeMove(Vector2 val)
    {
        if (move != null)
        {
            move.Invoke(val);
        }
    }

    public static void InvokePan()
    {
        if (pan != null)
        {
            pan.Invoke();
        }
    }

    public static void InvokePanStart()
    {
        if (panStart != null)
        {
            panStart.Invoke();
        }
    }

    public static void InvokeEscape()
    {
        if (escape != null)
        {
            escape.Invoke();
        }
    }

    public static void InvokeDelete()
    {
        if (delete != null)
        {
            delete.Invoke();
        }
    }

    public static void InvokeUndo()
    {
        if (Undo != null)
        {
            Undo.Invoke();
        }
    }

    public static void InvokeRedo()
    {
        if (Redo != null)
        {
            Redo.Invoke();
        }
    }

    public static void InvokeSave()
    {
        if (Save != null)
        {
            Save.Invoke();
        }
    }

    public static void InvokeCopy()
    {
        if (Copy != null)
        {
            Copy.Invoke();
        }
    }
    public static void InvokeCut()
    {
        if (Cut != null)
        {
            Cut.Invoke();
        }
    }

    public static void InvokePaste()
    {
        if (Paste != null)
        {
            Paste.Invoke();
        }
    }

    public static void InvokeGroup()
    {
        if(Group != null)
        {
            Group.Invoke();
        }
    }

}
