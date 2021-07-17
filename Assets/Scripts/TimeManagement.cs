using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManagement : MonoBehaviour
{

    public float second = 0;
    public int hour, minute = 0;
    public int month = 1;
    public int day = 1;
    public int year = 2020;

    // Start is called before the first frame update
    public float timeScaleIncrement = 1; // dobra a timescale
    public float minTimeScale = 0.01f;
    public float maxTimeScale = 100f;

    private float fixedDeltaTime;

    void Awoke(){
        fixedDeltaTime = Time.fixedDeltaTime;
    }
    // Update is called once per frame
    void Update()
    {
        
        //timeScale não pode ser menor que zero, logo deve ser multiplicado
            /*if(Input.GetAxis("SpeedChange") > 0){
                Time.timeScale += timeScaleIncrement;               
            }
            else if(Input.GetAxis("SpeedChange") < 0){
                Time.timeScale -= timeScaleIncrement; 
            }*/
        //A simulação possui diferentes velocidades a depender do valor dado a timescale
        //Pausa
        if(Input.GetKeyDown(KeyCode.Keypad0)){
            Time.timeScale = 0f;
        }
        //Normal
        else if(Input.GetKeyDown(KeyCode.Keypad1)){
            Time.timeScale = 1.0f;
        }
        //Rápido
        else if(Input.GetKeyDown(KeyCode.Keypad2)){
            Time.timeScale = 2.0f;
        }
        //Mais Rápido
        else if(Input.GetKeyDown(KeyCode.Keypad3)){
            Time.timeScale = 5.0f;
        }
        else if(Input.GetKeyDown(KeyCode.Keypad4)){
            Time.timeScale = 10f;
        }
        else if(Input.GetKeyDown(KeyCode.Keypad5)){
            Time.timeScale = 15f;
        }
    }

    public void changeTimeScale(){
        return;
    }
    public float ToSecond(int hour = 0, int minute = 0){
        return (hour*60*60) + (minute*60);
    }

    public void passSecond(){
        second += 1 * Time.timeScale * Time.deltaTime;//o atributo timeScale controla a velocidade da simulação.
        if(second >= 60){
            second = 0;
            minute++;
        }
        if(minute >= 60){
            minute = 0;
            hour++;
        }
        if(hour >= 24){
            day++;
            hour = 0;
        }
        if(day >= 30){
            day = 1;
            month++;
        }
        if(month >=12){
            month = 1;
            year++;
        }
    }

    public float getTime(){
        return ToSecond(hour, minute) + second;
    }
    public int getDate(){
        return (year*10000)+(month*100)+day;//retorna a data como ano/mês/dia
    }

    public bool isInInterval(int startHour, int startMinute, int endHour, int endMinute){
        return !(getTime() < ToSecond(startHour, startMinute)
        && getTime() > ToSecond(endHour, endMinute));
    }

    public void setTime(int newHour, int newMinute){
        hour = newHour;
        minute = newMinute;
    }
}
