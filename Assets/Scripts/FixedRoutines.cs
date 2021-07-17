using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class FixedRoutines : MonoBehaviour
{
    //Numero total de atividades diarias
    public static int numberOfActivities = new int();

    public TimeManagement timeManagement;
    public Pessoa pessoa;
    //Cria novo array com todas as atividades que o usuário realizara pelo dia
    //public string[] activityTimes = new string[numberOfActivities];
    //public string[] activities = new string[numberOfActivities];
    //public string nextActivity = "";
    //public int nextActivityTime = new int();

    public int counter = 0;

    private XmlDocument xmlRoutine = new XmlDocument();
    public XmlActivitiesInterpreter xmlInterpreter;
    XmlNodeList activities;
    XmlNode nextActivity;
    int nextActivityTime = new int();
    // Start is called before the first frame update
    void Start()
    {
        //xmlInterpreter = GetComponent<XmlActivitiesInterpreter>();
        pessoa = GetComponent<Pessoa>();
        xmlRoutine.Load(Application.dataPath + "/Scripts/Routine.xml");
        activities = xmlRoutine.SelectSingleNode("routine").ChildNodes;
        nextActivity = activities.Item(counter);
        nextActivityTime = ActivityTimeToSecond(nextActivity.Attributes["time"].Value);
    }

    // Update is called once per frame
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
        /*if(timeManagement.getTime() == nextActivityTime){
            pessoa.pushToQueue(nextActivity);
            counter++;
            if(counter == activities.Length){
                counter = 0;
            }
            
            nextActivityTime = ActivityTimeToSecond(activityTimes[counter]);
            nextActivity = activities[counter];
        }*/

        /*if(pessoa.getActivityCount() == 0 && !xmlInterpreter.isExecutingActivity){
            xmlInterpreter.StartCoroutine("relax");
        }*/

        if(pessoa.getActivityCount() != 0 && !xmlInterpreter.isExecutingActivity && !xmlInterpreter.isExecutingHighPriorityActivity){
            pessoa.startActivity();
        }


    }

    private int ActivityTimeToSecond(string time){
        string[] splitTime = time.Split(':');
        int hour = int.Parse(splitTime[0]);
        int minute = int.Parse(splitTime[1]);
        return (int) timeManagement.ToSecond(hour, minute);
    }
}
