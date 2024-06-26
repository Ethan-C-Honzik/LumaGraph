﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using nodeSys2;
using Newtonsoft.Json;
using SFB;
using UnityEngine;
using System;

public class SaveLoadManager : MonoBehaviour
{
    GlobalData globalData;
    private GUIGraph guiGraph;
    //if empty the current project has not been saved
    string currentPath = "";
    private void Start()
    {
        guiGraph = GetComponent<GUIGraph>();
        if (File.Exists(Application.persistentDataPath + "\\save.json"))
        {
            Debug.Log("Loading save file from: " + Application.persistentDataPath + "\\save.json");
            // deserialize JSON directly from a file
            StreamReader file = File.OpenText(Application.persistentDataPath + "\\save.json");
            JsonSerializer serializer = new JsonSerializer();
            globalData = (GlobalData)serializer.Deserialize(file, typeof(GlobalData));
        }
        else
        {
            Debug.Log("Save does not exist, creating new one at: " + Application.persistentDataPath + "\\save.json");
            globalData = new GlobalData();
            SaveGlobalData();
        }
        if (globalData.GetRecentlyOpened().Count > 0)
        {
            if (File.Exists(globalData.GetRecentlyOpened()[0]))
            {
                OpenProject(File.ReadAllText(globalData.GetRecentlyOpened()[0]));
            }
        }
    }

    private void OnEnable()
    {
        GlobalInputDelagates.Save += SaveProject;
    }

    private void OnDisable()
    {
        GlobalInputDelagates.Save -= SaveProject;
    }

    //saves last saved root project
    public void SaveProject()
    {
        if (currentPath == "")
        {
            SaveAs();
        }
        else
        {
            Debug.Log("Overwrite saving at: " + currentPath);
            File.WriteAllText(currentPath, guiGraph.GetRootGraphJson());
        }
        globalData.AddRecentlyOpened(currentPath);
        SaveGlobalData();
    }

    //saves root node graph
    public void SaveAs()
    {
        string path = StandaloneFileBrowser.SaveFilePanel("Save File", "", "", "Json");
        try
        {
            File.WriteAllText(path, guiGraph.GetRootGraphJson());
        }
        catch (Exception e)
        {
            Debug.LogWarning("Error Writing File: " + e.ToString());
            return;
        }
        Debug.Log("Created new save at: " + path);
        currentPath = path;
        globalData.AddRecentlyOpened(currentPath);
        SaveGlobalData();
    }

    //used to save node groups
    public void SaveCurrentGraph()
    {
        string path = StandaloneFileBrowser.SaveFilePanel("Save File", "", "", "Json");
        try
        {
            File.WriteAllText(path, guiGraph.GetCurrentJson());
        }
        catch (Exception e)
        {
            Debug.LogWarning("Error Writing File: " + e.ToString());
            return;
        }
        Debug.Log("Created new save at: " + path);
    }

    public void OpenProject()
    {
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "Json", false);
        string json;
        try
        {
            json = File.ReadAllText(paths[0]);
        }
        catch (Exception e)
        {
            Debug.LogWarning("Error opening File: " + e.ToString());
            return;
        }
        OpenProject(json);
        currentPath = paths[0];
        globalData.AddRecentlyOpened(paths[0]);
        SaveGlobalData();
    }

    public void AppendProject()
    {
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "Json", false);
        string json;
        try
        {
            json = File.ReadAllText(paths[0]);
        }
        catch (Exception e)
        {
            Debug.LogWarning("Error Writing File: " + e.ToString());
            return;
        }
        guiGraph.AppendGraph(GraphSerialization.JsonToGraph(json));
    }

    private void OpenProject(string json)
    {
        if (json != "")
        {
            guiGraph.SetRootGraph(GraphSerialization.JsonToGraph(json));
        }
        else
        {
            Debug.Log("Invalid path when opening project");
        }
    }

    public void NewProject()
    {
        currentPath = "";
        guiGraph.CreateNewGraph();
    }

    private void SaveGlobalData()
    {
        File.WriteAllText(Application.persistentDataPath + "\\save.json", JsonConvert.SerializeObject(globalData));
    }
}

[System.Serializable]
public class GlobalData
{
    [JsonProperty]
    private List<string> recentProjects = new List<string>();

    public List<string> GetRecentlyOpened()
    {
        return recentProjects;
    }
    public void AddRecentlyOpened(string path)
    {
        //if it's already in the list remove it and add it back to the front
        if (!recentProjects.Contains(path))
        {
            recentProjects.Remove(path);
        }
        recentProjects.Insert(0, path);
        if (recentProjects.Count > 5)
        {
            recentProjects.RemoveAt(recentProjects.Count - 1);
        }

    }

}
