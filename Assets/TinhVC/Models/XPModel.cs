using UnityEngine;
using System.Collections;

public class XPModel {

    private string point = "0";
    private string level = "1";
    private string type = "";
    private string medal = "";
    private string nextLevel = "1";
    private string currentLevel = "0";
    private string persent = "0";

    public string Persent
    {
        get { return persent; }
        set { persent = value; }
    }

    public string NextLevel
    {
        get { return nextLevel; }
        set { nextLevel = value; }
    }
    

    public string CurrentLevel
    {
        get { return currentLevel; }
        set { currentLevel = value; }
    }
   
    public string Point
    {
        get { return point; }
        set { point = value; }
    }

    public string Level
    {
        get { return level; }
        set { level = value; }
    }

    public string Type
    {
        get { return type; }
        set { type = value; }
    }

    public string Medal
    {
        get { return medal; }
        set { medal = value; }
    }

}
