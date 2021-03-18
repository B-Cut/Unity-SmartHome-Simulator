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

    private XmlActivitiesInterpreter xmlActivities;
    //Variaveis relacionadas à rotina do usuário serão substituidas pelo os valores do arquivo CSV


    public TimeManagement timeManagement;

    private Activities activities;
    public FixedRoutines fixedRoutines;
    public IntervalRoutine intervalRoutine;
    private Destination destination;

    public int sleepTimeHour = 22;
    public int sleepTimeMinute = 0;

    public int wakeUpTimeHour = 8;
    public int wakeUpTimeMinute = 0;
    
    public bool sleepInQueue = false;

    public bool isRelaxing = false;
    
    private int today = new int();
    public bool useFixedRoutine = false;

    

    

    public float chanceToForget = 0.01f;//chance do usuário esquecer de fazer algo 
    void Start()
    {
        timeManagement.setTime(wakeUpTimeHour, wakeUpTimeMinute);//começa a simulação quando a pessoa acorda.
        today = timeManagement.getDate();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        destination = transform.parent.GetComponentInChildren<Destination>();

        activities = GetComponentInChildren<Activities>();
        XmlActivitiesInterpreter.activityEnded += startActivity;
        xmlActivities = GetComponent<XmlActivitiesInterpreter>();
        //activityQueue.Enqueue("sleep");
        if(useFixedRoutine){
            intervalRoutine.enabled = false;
            fixedRoutines.enabled = true;

        }
        
        else{
            intervalRoutine.enabled = true;
            fixedRoutines.enabled = false;
        }
    }
    void Update(){
    }


    //essa função serve para ser acionada pelo evento e realizar a proxima atividade na fila
    public void startActivity(){
        try{
            this.xmlActivities.StartCoroutine("ExecuteActivity", activityQueue.Dequeue());
        } catch(Exception e){
            Debug.Log($"Tried to Dequeue from empty queue ${e}");
            //this.activities.StartCoroutine("relax");
        }
    } 
    public int getActivityCount(){
        return this.activityQueue.Count;
    }
    public bool changeDestination(Vector3 destPos){
        if(!agent.SetDestination(destPos)){
            return false;
        }
        destination.moveTo(destPos);
        anim.ResetTrigger("GoIdle");
        anim.SetTrigger("Walk");
        return true;
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
