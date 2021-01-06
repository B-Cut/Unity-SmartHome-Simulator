using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.IO;
using UnityEngine;

public class ExportSensorData : MonoBehaviour
{
    public TimeManagement timeManagement;
    
    //de quanto em quanto segundos escrever os dados do sensor no CSV
    public float writeInterval = 10f;
    //private char lineSeparator = '\n';
    //private char fieldSeparator = ',';
    //private Dictionary<string, string> data;

    private string csvPath;
    private StreamWriter writer;
    // Start is called before the first frame update
    void Start()
    {
        string csvPath = getPath();
        writer = new StreamWriter(csvPath);
        writer.WriteLine("Date,Hour,Name,Type,State,User");
        writer.Flush();

        InvokeRepeating("getSensorData", 0f, writeInterval);
    }

    void OnApplicationQuit(){ 
        writer.Flush();
        writer.Close();    
    }

    private void getSensorData(){
        SensorRFID[] rfidsInScene = GetComponentsInChildren<SensorRFID>();
        AutomaticLampSwitch[] lampsInScene = GetComponentsInChildren<AutomaticLampSwitch>();
        foreach(SensorRFID sensor in rfidsInScene){
            writer.WriteLine($"{timeManagement.getDate()},{Math.Round(timeManagement.getTime())},{sensor.name},{sensor.sensorType}" +
                            $",{sensor.getState()},{sensor.getUser()}");
        }    
        foreach(AutomaticLampSwitch lamp in lampsInScene){
            writer.WriteLine($"{timeManagement.getDate()},{Math.Round(timeManagement.getTime())},{lamp.name},{lamp.sensorType}" +
                            $",{lamp.getState()},{lamp.getUser()}");
        }
        writer.Flush();
    }


    private string getPath(){
        return Application.dataPath + "/DadosExternos/DadosSensor.csv";
    }
}
