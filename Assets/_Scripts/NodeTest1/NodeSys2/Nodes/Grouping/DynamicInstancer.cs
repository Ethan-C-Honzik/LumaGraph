﻿using Newtonsoft.Json;
using nodeSys2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//overrides the handle of static instancer such that only one instance is fed data at a time when this node is pulsed 
public class DynamicInstancer : StaticInstancer
{
    [JsonProperty] private Property instanceTrigger;
    private int currentInstance;

    public DynamicInstancer(ColorVec pos) : base(pos)
    {
        nodeDisc = "Dynamic Instancer";
        instanceTrigger = CreateInputProperty("Trigger", true, new Pulse(false), 0);
    }

    public override void Init()
    {
        base.Init();
        currentInstance = 0;
    }

    public override void Handle()
    {
        if (((Pulse)instanceTrigger.GetData()).PulsePresent())
        {
            //Debug.Log("InstanceRunning: " + currentInstance + "\t Group Count:" + groups.Count);
            foreach (Property prop in groupInputs)
            {
                if (groups[currentInstance] != null)
                {
                    groups[currentInstance].PublishToGraph(prop.ID, prop.GetData());
                }                
            }
            groups[currentInstance].PulseGraph();
            currentInstance = (currentInstance + 1) % groups.Count;            
        }
    }
}