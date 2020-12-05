﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using nodeSys2;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

public class TransformNode : Node
{
    public Property setTypeProp, inputData, localOffset, scale, pivot, rot, globalOffset, output;

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SetType {OverWrite, Add, Subtract}
    private SetType setType;

    public TransformNode(bool x)
    {
        base.nodeDisc = "Transform Node";
        setTypeProp = CreateInputProperty("Mix Type", false, new SetType());
        setTypeProp.interactable = true;
        inputData = CreateInputProperty("Input", true, new Evaluable(), typeof(Evaluable));
        localOffset = CreateInputProperty("Local Offset", true, new EvaluableFloat(0), typeof(Evaluable));
        localOffset.interactable = true;
        scale = CreateInputProperty("Scale", true, new EvaluableFloat(1), typeof(Evaluable));
        scale.interactable = true;
        pivot = CreateInputProperty("pivot", true, new EvaluableFloat(0), typeof(Evaluable));
        pivot.interactable = true;
        rot = CreateInputProperty("Rotation", true, new EvaluableFloat(0), typeof(Evaluable));
        rot.interactable = true;
        globalOffset = CreateInputProperty("Global Offset", true, new EvaluableFloat(0), typeof(Evaluable));
        globalOffset.interactable = true;
        output = CreateOutputProperty("Output");
    }

    public override void Init()
    {
        base.Init();
        ProcessEnums();
        ManipulateTransform();
    }

    public override void Handle()
    {
        base.Handle();
        ManipulateTransform();
        output.Invoke(((Evaluable)inputData.GetData()).GetCopy());
    }

    private void ManipulateTransform()
    {
        Evaluable data = (Evaluable)inputData.GetData();        
        switch (setType)
        {
            case SetType.OverWrite:
                data.localOffset = ((Evaluable)(localOffset.GetData())).EvaluateColor(0,0,0,0);
                data.scale = ((Evaluable)(scale.GetData())).EvaluateColor(0,0,0,0);
                data.pivot = ((Evaluable)(pivot.GetData())).EvaluateColor(0,0,0,0);
                data.rot = ((Evaluable)(rot.GetData())).EvaluateColor(0,0,0,0);
                data.globalOffset = ((Evaluable)(globalOffset.GetData())).EvaluateColor(0,0,0,0);
                break;
            case SetType.Add:
                data.localOffset += ((Evaluable)(localOffset.GetData())).EvaluateColor(0,0,0,0);
                data.scale += ((Evaluable)(scale.GetData())).EvaluateColor(0,0,0,0);
                data.pivot += ((Evaluable)(pivot.GetData())).EvaluateColor(0, 0, 0, 0);
                data.rot += ((Evaluable)(rot.GetData())).EvaluateColor(0, 0, 0, 0);
                data.globalOffset += ((Evaluable)(globalOffset.GetData())).EvaluateColor(0, 0, 0, 0);
                break;
            case SetType.Subtract:
                data.localOffset -= ((Evaluable)(localOffset.GetData())).localOffset;
                data.scale -= ((Evaluable)(scale.GetData())).scale;
                data.pivot -= ((Evaluable)(pivot.GetData())).pivot;
                data.rot -= ((Evaluable)(rot.GetData())).rot;
                data.globalOffset -= ((Evaluable)(globalOffset.GetData())).globalOffset;
                break;
            default:
                break;
        }
    }

    private void ProcessEnums()
    {
        if(setTypeProp.GetData().GetType() == typeof(string))
        {
            setTypeProp.SetData((SetType)Enum.Parse(typeof(SetType), (string)setTypeProp.GetData()));
        }
        setType = (SetType)setTypeProp.GetData();        
    }
}