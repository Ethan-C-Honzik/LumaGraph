﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nodeSys2
{
    public class Node
    {
        public delegate void NetworkDelagate(NetworkMessage message);
        public delegate void FloatDelagate(float num);
        [JsonIgnore]
        //Frame delagate is a delage that belongs to all nodes. It is called each frame by whatever host is currently running the graph. 
        public static FloatDelagate frameDelagate;

        [JsonIgnore]
        //Frame delagate is a delage that belongs to all nodes. It is called each frame by whatever host is currently running the graph. 
        public static NetworkDelagate nodeNetDelagate;

        //GUI info
        public float xPos, yPos;
        public float xScale = 250, yScale = 10;
        public bool expanded = false;

        public List<Property> inputs = new List<Property>();
        public List<Property> outputs = new List<Property>();


        //the node discription for identification in JSON and GUI 
        [JsonProperty] protected string nodeDisc;

        [JsonIgnore]public bool MarkedForDeletion = false;
        //tracks if the handle method has already run this frame
        [JsonIgnore] private bool hasRan = false;

        //this method is called by input ports before they invoke portdel.
        public bool Runnable()
        {
            //if it's already run then return false
            if (hasRan)
            {
                return false;
            }
            else
            {
                //if it has yet to run mark as true and return true
                hasRan = true;
                return hasRan;
            }
        }

        public void ResetRunnable()
        {
            hasRan = false;
        }

        public Node()
        {

        }
        
        //ensures that all ports delagates are connected to each other and references to parents are properly set
        public void InitProperties()
        {
            for (int i = 0; i < inputs.Count; i++)
            {
                inputs[i].SetupRefs(this);
                inputs[i].dataPort.Reconnect();
            }
            for (int i = 0; i < outputs.Count; i++)
            {
                outputs[i].SetupRefs(this);
                outputs[i].dataPort.Reconnect();
            }
        }

        //creates a property and adds it to the list. Also returns the created property to optionally be used for caching
        public Property CreateInputProperty(string ID, bool connectable, object defaultData)
        {
            //===============DUPLICATE CHECKING MIGHT BE BUSTED================================
            //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

            //this is done both to prevent accidental duplicates and duplicates from the constructor running on deserialization.
            if (inputs.TrueForAll(prop => prop.ID != ID))
            {
                Property tempRef = new Property(this, ID, true, connectable, defaultData, typeof(object));
                inputs.Add(tempRef);
                return tempRef;
            }
            else
            {
                Debug.Log("Duplicate property found, proably serialization. Returning old ");
                return inputs.FindAll(prop => prop.ID == ID)[0];
            }
        }

        public Property CreateInputProperty(string ID, bool connectable, object defaultData, Type type)
        {
            if (inputs.TrueForAll(prop => prop.ID != ID))
            {
                Property tempRef = new Property(this, ID, true, connectable, defaultData, type);
                inputs.Add(tempRef);
                return tempRef;
            }
            else
            {
                Debug.Log("Duplicate property found, proably serialization. Returning old ");
                return inputs.FindAll(prop => prop.ID == ID)[0];
            }
        }

        public Property CreateOutputProperty(string ID)
        {
            if (outputs.TrueForAll(prop => prop.ID != ID))
            {
                Property tempRef = new Property(this, ID, false, true, null, typeof(object));
                outputs.Add(tempRef);
                return tempRef;
            }
            else
            {
                Debug.Log("Duplicate property found, proably serialization. Returning old ");
                return outputs.FindAll(prop => prop.ID == ID)[0];
            }
        }

        //removes property from list by reference. Returns true if reference was found and removed, false otherwise
        //also sets input reference to null
        public bool RemoveProperty(Property prop)
        {
            for (int i = 0; i < inputs.Count; i++)
            {
                if(inputs[i] == prop)
                {
                    inputs[i].SetConnectable(false);
                    inputs.RemoveAt(i);                    
                    return true;
                }
            }
            for (int i = 0; i < outputs.Count; i++)
            {
                if (outputs[i] == prop)
                {
                    outputs[i].SetConnectable(false);
                    outputs.RemoveAt(i);                    
                    return true;
                }
            }
            return false;
        }

        public void CleanUp()
        {
            frameDelagate -= Frame;
        }

        public string GetName()
        {
            return nodeDisc;
        }

        //to be called on every node after all nodes are instantiated and connected. Usefull for sending constants down the graph
        //on startup. 
        public virtual void Init()
        {

        }

        //called on every "frame" if running in unity these frames are called each unity frame. 
        //If running standalone then a main class will invoke on a while loop. In order for this to work Base.Init must be called at some point to register the node
        public virtual void Frame(float deltaTime)
        {

        }

        //gets registered with the network receive delagate. This will be called each time data from the network is received. It's up to each node to 
        //decide if they want to use the data or not
        public virtual void ReceiveData(NetworkMessage message)
        {

        }

        //the main meathod that handles data. The index specifies the port from which the node is receiving data and the
        //object contains that data that needs to be pattern matched in order to use
        public virtual void Handle()
        {

        }
    }
}