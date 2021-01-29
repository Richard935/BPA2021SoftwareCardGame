using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class LogSystemEvents : MonoBehaviour
{
    public List<string> eventLog; 

    public void logEvents()
    {
        //Logs game events to text file.
        StreamWriter writer = new StreamWriter("Assets/Resources/CrashReports.txt");

        foreach (var e in eventLog)
        {
            writer.WriteLine(e);
            writer.WriteLine();
        }
        writer.Close();
        //AssetDatabase.Refresh();
    }
}
