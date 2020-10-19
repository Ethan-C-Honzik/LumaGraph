﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using nodeSys2;

public class Property
{
    //this class will serve to wrap the constants, constant discriptions, and ports, all in one class.
    //these will be stored in a list in the node class and will be gotten via string lookups similar to get component
    //they will return the property object so object caching can take place

    //string identifier for lookups
    public string ID;
    //data that gets returned
    [JsonProperty]
    private object data;
    //Determines whether the port is input or output
    public bool isInput;
    //data discription. This will be used for port and editor discriptions. If not provided it will fallback to the ID
    public string disc;
    //Determines i this property has a port
    public bool connectable;
    //determines if editor is read only. Same as setting a viewable vs constant in old system
    public bool interactable;
    //will determine the rect transform height
    public float height;
    //this port will be shown if the connectable flag is set to true.
    //Something important to note: The index of this port will be used when routing the data back to this property
    public Port dataPort;

    public Property(string ID, bool isInput, bool connectable, object DefaultData)
    {
        dataPort = new Port();
        dataPort.portDisc = ID;
        data = DefaultData;
        this.isInput = isInput;
        //NOTE: all output properties should be connectable
        this.connectable = connectable;
        this.ID = ID;
        this.disc = ID;
    }

    //delagates can't be serialized so they need to be re-assigned here
    public void Setup()
    {
        if (isInput)
        {
            //remove first because if it's not already there it doesn't do anything. 
            dataPort.portDel -= Handle;
            dataPort.portDel += Handle;
        }
    }

    public void Handle(object data)
    {
        this.data = data;
    }

    public object GetData()
    {
        if (isInput)
        {
            return data;
        }
        else
        {
            Debug.Log("Attempted to get data from output property");
            return new object();
        }
    }
}