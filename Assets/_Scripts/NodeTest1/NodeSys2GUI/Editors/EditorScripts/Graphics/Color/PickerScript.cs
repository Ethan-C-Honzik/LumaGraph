﻿using System.Collections;
using System.Collections.Generic;
using nodeSys2;
using UnityEngine;
using UnityEngine.UI;

public class PickerScript : MonoBehaviour
{
    public Text title;
    Property prop;
    public void Setup(Property prop)
    {
        title.text = prop.Disc;
        this.prop = prop;
    }

    public void SetColor(Color color)
    {
        //A color assinment happens the second the color editor is opened before it has a chance to call setup
        if (prop != null)
        {
            ColorVec newColorVec = new ColorVec(color.r, color.g, color.b, color.a);
            prop.SetData(new EvaluableColorVec(newColorVec));
        }
    }

    public void Destroy()
    {
        GUIGraph.updateGraphGUI.Invoke();
        Destroy(gameObject);
    }
}
