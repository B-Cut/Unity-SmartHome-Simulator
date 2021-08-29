using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

///<summary>Essa classe controla a rotina do usuário.</summary>
public class FixedRoutines : MonoBehaviour
{
    //Numero total de atividades diarias
    public static int numberOfActivities = new int();

    public TimeManagement timeManagement;
    public Pessoa pessoa;

    public int counter = 0;

    private XmlDocument xmlRoutine = new XmlDocument();
    public XmlActivitiesInterpreter xmlInterpreter;
    XmlNodeList activities;
    XmlNode nextActivity;
    int nextActivityTime = new int();
    
    void Start()
    {
        pessoa = GetComponent<Pessoa>();
        xmlRoutine.Load(Application.dataPath + "/Scripts/Routine.xml");
        activities = xmlRoutine.SelectSingleNode("routine").ChildNodes;
        nextActivity = activities.Item(counter);
        nextActivityTime = ActivityTimeToSecond(nextActivity.Attributes["time"].Value);
    }

    void Update()
    {
        if(timeManagement.getTime() == nextActivityTime){
            if(xmlInterpreter.getPriority(nextActivity.InnerText.Trim()) > xmlInterpreter.currentActivityPriority
            && xmlInterpreter.isExecutingActivity){
                xmlInterpreter.higherPriorityActivity = nextActivity.InnerText.Trim();
                xmlInterpreter.executeHigherPriority = true;
            }
            else{
                pessoa.pushToQueue(nextActivity.InnerText.Trim());
            }
            
            counter++;
            if(counter == activities.Count){
                counter = 0;
            }
            nextActivity = activities.Item(counter);
            nextActivityTime = ActivityTimeToSecond(nextActivity.Attributes["time"].Value);
        }

        if(pessoa.getActivityCount() != 0 && !xmlInterpreter.isExecutingActivity && !xmlInterpreter.isExecutingHighPriorityActivity){
            pessoa.startActivity();
        }


    }
    ///<summary>Lê a string no formato <c>"hh:mm"</c> e retorna seu valor em segundos</summary>
    private int ActivityTimeToSecond(string time){
        string[] splitTime = time.Split(':');
        int hour = int.Parse(splitTime[0]);
        int minute = int.Parse(splitTime[1]);
        return (int) timeManagement.ToSecond(hour, minute);
    }
}
