﻿using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using nodeSys2;

public class InstanceInfoNode : InfoNode
{
    [JsonProperty] private Property indexProp, countProp, ratioProp;
    [JsonProperty] private Property indexPropOut, countPropOut, ratioPropOut;

    public InstanceInfoNode(ColorVec pos) : base(pos)
    {
        base.nodeDisc = "Instance Info";
        indexProp = CreateInputProperty("Info_index", false, new EvaluableFloat(0));
        indexProp.visible = false;
        RegisterInfoInputProperty(indexProp);
        indexPropOut = CreateOutputProperty("Instance Index");

        countProp = CreateInputProperty("Info_count", false, new EvaluableFloat(0));
        countProp.visible = false;
        RegisterInfoInputProperty(countProp);
        countPropOut = CreateOutputProperty("Total Instance Count");

        ratioProp = CreateInputProperty("Info_ratio", false, new EvaluableFloat(0));
        ratioProp.visible = false;
        RegisterInfoInputProperty(ratioProp);
        ratioPropOut = CreateOutputProperty("Instance Ratio");
    }

    public override void Handle()
    {
        indexPropOut.Invoke(indexProp.GetData());
        countPropOut.Invoke(countProp.GetData());
        ratioPropOut.Invoke(ratioProp.GetData());
    }
}
