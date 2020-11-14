using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticAirConditioneer : MonoBehaviour
{
    // Start is called before the first frame update
    public Material arLigado;
    public Material arDesligado;
    public TimeManagement timeManagement;
    private Material[] materialsArray;

    public int turnOnTimeHour = 22;
    public int turnOnTimeMinute = 00;

    public int turnOffTimeHour = 8;
    public int turnOffTimeMinute = 0;

    private Renderer rend;
    void Start()
    {
        rend = GetComponent<Renderer>();

        turnOff();       
    }


    /*void Update(){
        if(timeManagement.getTime() < timeManagement.ToSecond(turnOnTimeHour, turnOnTimeMinute)
        && timeManagement.getTime() > timeManagement.ToSecond(turnOffTimeHour, turnOffTimeMinute))
        {
             turnOff();
        }

        else turnOn();
    }*/
    // Update is called once per frame
    
    void OnTriggerEnter(Collider other){
        if(other.tag == "Player" && isTurnOnTime()){
            CancelInvoke("turnOff");
            Invoke("turnOn", 5f);
        }
    }
    
    void OnTriggerExit(Collider other){
        if(other.tag == "Player"){
            CancelInvoke("turnOn");
            Invoke("turnOff", 5f);
        }
    }
    public void turnOn(){
        rend.material = arLigado;
    }

    public void turnOff(){
        rend.material = arDesligado;
    }

    //função por questão de leitura do codigo, retorna verdadeiro caso a hora esteja dentro do intervalo do ar ligado
    private bool isTurnOnTime(){
        return !(timeManagement.getTime() < timeManagement.ToSecond(turnOnTimeHour, turnOnTimeMinute)
        && timeManagement.getTime() > timeManagement.ToSecond(turnOffTimeHour, turnOffTimeMinute));//checa quando o ar deveria estar ligado e inverte o retorno
    }
}
