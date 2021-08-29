using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.IO;
using UnityEngine;

///<summary>Controla a escrita dos dados dos sensores para o arquivo CSV.</summary>

//A fazer:
    //Adicionar caminho customizável

public class ExportSensorData : MonoBehaviour
{
    public TimeManagement timeManagement;

    public XmlActivitiesInterpreter xmlActivitiesInterpreter;
    
    //de quanto em quanto segundos escrever os dados do sensor no CSV
    public float writeInterval = 10f;
    //private char lineSeparator = '\n';
    //private char fieldSeparator = ',';
    //private Dictionary<string, string> data;

    private string csvPath;
    private StreamWriter writer;

    private BaseSensorScript[] sensorsInScene;
    void Start()
    {
        string csvPath = getPath();
        
        sensorsInScene = FindObjectsOfType<BaseSensorScript>();

        writer = new StreamWriter(csvPath);
        writer.WriteLine("Date,Hour,Name,State,UserActivity");
        writer.Flush();

        InvokeRepeating("getSensorData", 0f, writeInterval);
    }

    void OnApplicationQuit(){ 
        writer.Flush();
        writer.Close();    
    }

    private void getSensorData(){
        foreach(BaseSensorScript sensor in sensorsInScene){
            writer.WriteLine($"{timeManagement.getDate()},{Math.Round(timeManagement.getTime())},{sensor.name},{sensor.getState()},{xmlActivitiesInterpreter.currentActivity}");
        }    
        writer.Flush();
    }

    private string getPath(){
        return Application.dataPath + "/DadosExternos/DadosSensor.csv";
    }
}
