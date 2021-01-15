using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntervalRoutine : MonoBehaviour
{
    public TimeManagement timeManagement;
    public Pessoa pessoa;

    public int mealInterval = 3;//hours
    public int bathroomInterval = 4;//hours
    public bool emptyQueue = false;
    private bool bathOnQueue = false;
    private string nextActivity = "";
    public bool isRelaxing = false;

    public int cookingTimeHour = 12;
    public int cookingTimeMinute = 0;

    public int bathroomTimeHour = 8;
    public int bathroomTimeMinute = 0;

    public int bathTimeHour = 14;
    public int bathTimeMinute = 0;


    public int sleepTimeHour = 22;
    public int sleepTimeMinute = 0;

    public int wakeUpTimeHour = 8;
    public int wakeUpTimeMinute = 0;
    public bool sleepInQueue = false;
    private int today = new int();
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if(timeManagement.getTime() >= timeManagement.ToSecond(cookingTimeHour, cookingTimeMinute)){
                pessoa.pushToQueue("cook");
                cookingTimeHour += 3;
                //if(cookingTimeHour >= 24) cookingTimeHour -= 24;
            }

            if(timeManagement.getTime() >= timeManagement.ToSecond(bathroomTimeHour, bathroomTimeMinute)){
                pessoa.pushToQueue("useBathroom");
                bathroomTimeHour += bathroomInterval;
                //if(bathroomTimeHour >= 24) bathroomTimeHour -= 24;
            }

            if(timeManagement.getTime() >= timeManagement.ToSecond(bathTimeHour, bathroomTimeMinute) && !bathOnQueue){
                pessoa.pushToQueue("takeBath");
                bathOnQueue = true;
            }

            if(timeManagement.getTime() >= timeManagement.ToSecond(sleepTimeHour, sleepTimeMinute) && !sleepInQueue){
                pessoa.pushToQueue("sleep");
                sleepInQueue = true;
            }
            //Novo Dia
            if(timeManagement.getDate() > today){
                if(cookingTimeHour >= 24) cookingTimeHour = wakeUpTimeHour;
                if(bathroomTimeHour >= 24) bathroomTimeHour = wakeUpTimeHour;
                today = timeManagement.getDate();
            }

            if(emptyQueue && !isRelaxing){ 
                pessoa.pushToQueue("relax");
                isRelaxing = true;
            }
            if(pessoa.getActivityCount() > 0){
                emptyQueue = false;
            }  
    }
}
