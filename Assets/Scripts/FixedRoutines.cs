using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedRoutines : MonoBehaviour
{
    //Numero total de atividades diarias
    public static int numberOfActivities = new int();

    public TimeManagement timeManagement;
    public Pessoa pessoa;
    //Cria novo array com todas as atividades que o usuário realizara pelo dia
    public string[] activityTimes = new string[numberOfActivities];
    public string[] activities = new string[numberOfActivities];
    public string nextActivity = "";
    public int nextActivityTime = new int();

    public int counter = 0;
    // Start is called before the first frame update
    void Start()
    {
        nextActivityTime = ActivityTimeToSecond(activityTimes[counter]);
        nextActivity = activities[counter];
    }

    // Update is called once per frame
    void Update()
    {
        if(timeManagement.getTime() == nextActivityTime){
            pessoa.pushToQueue(nextActivity);
            counter++;
            if(counter == activities.Length){
                counter = 0;
            }
            
            nextActivityTime = ActivityTimeToSecond(activityTimes[counter]);
            nextActivity = activities[counter];
        }
    }

    private int ActivityTimeToSecond(string time){
        string[] splitTime = time.Split(':');
        int hour = int.Parse(splitTime[0]);
        int minute = int.Parse(splitTime[1]);
        return (int) timeManagement.ToSecond(hour, minute);
    }
}
