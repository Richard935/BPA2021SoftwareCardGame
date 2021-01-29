using UnityEngine;
using System.IO;
using UnityEditor;

public class Crashes : MonoBehaviour
{

    void OnGUI()
    {
        var reports = CrashReport.reports;
        
        //Writes crash reports to a text file.
        if(reports.Length > 0)
        {
            StreamWriter writer = new StreamWriter("Assets/Resources/CrashReports.txt");
        
            foreach (var r in reports)
            {
                writer.WriteLine("Crash: " + r.time);
                writer.WriteLine(r.text);
                writer.WriteLine();
            }
            writer.Close();

            //Refreshes unity asset database.
            //AssetDatabase.Refresh();
        }

    }
}