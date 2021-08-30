﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluableMixRGB : Evaluable
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MixType
    {
        Add, Mix
    }
    public MixType mixType = MixType.Add;
    //the factor controls to what extent the mix type is applied. Every add operation is multiplied by the factor and mix 
    //interpolates between elements based on the factor. 
    public Evaluable factor;
    public List<Evaluable> elements;

    public EvaluableMixRGB(Evaluable factor)
    {
        this.factor = factor;
        elements = new List<Evaluable>();
    }

    public void AddElement(Evaluable element)
    {
        elements.Add(element);
    }

    public override ColorVec EvaluateColor(ColorVec vector)
    {        
        return base.EvaluateColor(vector);
    }

    public override float EvaluateValue(ColorVec vector)
    {
        return 0;
    }
}
