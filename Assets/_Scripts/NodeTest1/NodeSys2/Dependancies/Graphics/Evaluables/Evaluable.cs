﻿public class Evaluable
{
    Evaluable offset;
    Evaluable scale;
    Evaluable rot;
    Evaluable pivot;

    public virtual ColorVec EvaluateColor(float x, float y, float z, float w)
    {
        return new ColorVec(0);
    }

    public virtual float EvaluateValue(float x, float y, float z, float w)
    {
        return 0;
    }

    //used to get copy of Evaluable object so references arn't being passed between nodes. Reference passing would entangle
    //node states and create problems
    public virtual Evaluable GetCopy()
    {
        return new Evaluable();
    }

    //to be used for vector transformations in subclasses
    protected ColorVec TransformVector(ColorVec vector)
    {
        //NOT IMPLEMENTED
        return new ColorVec(0,0,0,0);
    }
}