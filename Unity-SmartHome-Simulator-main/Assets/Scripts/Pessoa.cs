using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

///<summary>Classe responável por controlar e coordenas as ações gerais do usuário.</summary>///

public class Pessoa : MonoBehaviour
{
    private string currentActivity = "";
    private NavMeshAgent agent;
    private Queue<string> activityQueue = new Queue<string>();
    private Animator anim;

    private XmlActivitiesInterpreter xmlActivities;


    public TimeManagement timeManagement;

    public FixedRoutines fixedRoutines;
    private Destination destination;

    public int sleepTimeHour = 22;
    public int sleepTimeMinute = 0;

    public int wakeUpTimeHour = 8;
    public int wakeUpTimeMinute = 0;
    
    public bool sleepInQueue = false;

    public bool isRelaxing = false;
    
    private int today = new int();
    
    void Start()
    {
        timeManagement.setTime(wakeUpTimeHour, wakeUpTimeMinute);//começa a simulação quando a pessoa acorda.
        today = timeManagement.getDate();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        destination = transform.parent.GetComponentInChildren<Destination>();

        XmlActivitiesInterpreter.activityEnded += startActivity;
        xmlActivities = GetComponent<XmlActivitiesInterpreter>();
    }


    ///<summary>Inicia uma nova atividade caso haja atividade na fila. Caso contrário, inicia a atividade <example>relax</example>.</summary>
    public void startActivity(){
        try{
            this.xmlActivities.StartCoroutine("ExecuteActivity", activityQueue.Dequeue());
        } catch(Exception e){
            Debug.Log($"Tried to Dequeue from empty queue ${e}");
            //Quando a fila está vazia a atividade relax é iniciada
            xmlActivities.StartCoroutine("relax");
        }
    } 
    ///<summary>Retorna o numero de atividades em <paramref name="activityQueue"/></summary>
    public int getActivityCount(){
        return this.activityQueue.Count;
    }
    ///<summary>Move a o usuário para o local indicado.</summary>
    public bool changeDestination(Vector3 destPos){
        if(!agent.SetDestination(destPos)){
            return false;
        }
        destination.moveTo(destPos);
        anim.ResetTrigger("GoIdle");
        anim.SetTrigger("Walk");
        return true;
    } 

    ///<summary>Retorna um array com os itens em <paramref name="activityQueue"/>.</summary>
    public string[] getActivities(){
        return activityQueue.ToArray();
    }

    ///<summary>Muda o conteúdo de <paramref name="currentActivity"/>.</summary>
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
