using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Essa classe é responsável por simular a rotina do Usuário

public class Pessoa : MonoBehaviour
{
    private string currentActivity = "";
    private NavMeshAgent agent;
    private Queue<string> activityQueue = new Queue<string>();
    private Animator anim;

    //Variaveis relacionadas à rotina do usuário serão substituidas pelo os valores do arquivo CSV

    public int mealInterval = 3;//hours
    public int bathroomInterval = 4;//hours
    public bool emptyQueue = false;
    private bool bathOnQueue = false;
    private string nextActivity = "";
    public bool isRelaxing = false;

    public TimeManagement timeManagement;

    private Activities activities;
    private Destination destination;

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

    public float chanceToForget = 0.01f;//chance do usuário esquecer de fazer algo 
    void Start()
    {
        timeManagement.setTime(wakeUpTimeHour, wakeUpTimeMinute);//começa a simulação quando a pessoa acorda.
        today = timeManagement.getDate();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        destination = transform.parent.GetComponentInChildren<Destination>();

        activities = GetComponentInChildren<Activities>();
        Activities.activityEnded += startActivity;
        //activityQueue.Enqueue("sleep");
        startActivity();
    }
    void Update(){
        if(timeManagement.getTime() >= timeManagement.ToSecond(cookingTimeHour, cookingTimeMinute)){
            activityQueue.Enqueue("cook");
            cookingTimeHour += 3;
            //if(cookingTimeHour >= 24) cookingTimeHour -= 24;
        }

        if(timeManagement.getTime() >= timeManagement.ToSecond(bathroomTimeHour, bathroomTimeMinute)){
            activityQueue.Enqueue("useBathroom");
            bathroomTimeHour += bathroomInterval;
            //if(bathroomTimeHour >= 24) bathroomTimeHour -= 24;
        }

        if(timeManagement.getTime() >= timeManagement.ToSecond(bathTimeHour, bathroomTimeMinute) && !bathOnQueue){
            activityQueue.Enqueue("takeBath");
            bathOnQueue = true;
        }

        if(timeManagement.getTime() >= timeManagement.ToSecond(sleepTimeHour, sleepTimeMinute) && !sleepInQueue){
            activityQueue.Enqueue("sleep");
            sleepInQueue = true;
        }
        //Novo Dia
        if(timeManagement.getDate() > today){
            if(cookingTimeHour >= 24) cookingTimeHour = wakeUpTimeHour;
            if(bathroomTimeHour >= 24) bathroomTimeHour = wakeUpTimeHour;
            today = timeManagement.getDate();
        }

        if(emptyQueue && !isRelaxing){ 
            activityQueue.Enqueue("relax");
            isRelaxing = true;
        }
        if(activityQueue.Count > 0){
            emptyQueue = false;
        }  
    }


    //essa função serve para ser acionada pelo evento e realizar a proxima atividade na fila
    void startActivity(){
        try{
            this.activities.StartCoroutine(activityQueue.Dequeue());
        } catch(Exception e){
            Debug.Log($"Tried to Dequeue from empty queue ${e}");
            this.activities.StartCoroutine("relax");
        }
    } 
    public int getActivityCount(){
        return this.activityQueue.Count;
    }
    public void changeDestination(Vector3 destPos){
        agent.SetDestination(destPos);
        destination.moveTo(destPos);
        anim.ResetTrigger("GoIdle");
        anim.SetTrigger("Walk");     
    } 

    public string[] getActivities(){
        return activityQueue.ToArray();
    }
    public bool forgot(){
        return UnityEngine.Random.Range(0, 1) <= chanceToForget;
    }

    public void setCurrentActivity(string newActivity){
        currentActivity = newActivity;
    }

    public void pushToQueue(string command){
        this.activityQueue.Enqueue(command);
    }

    public string getCurrentActivity(){
        return currentActivity;
    }
}
