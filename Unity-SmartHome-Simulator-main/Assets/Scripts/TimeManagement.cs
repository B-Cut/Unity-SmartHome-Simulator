using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///<summary>Essa classe controla e gerencia o tempo da simulação</summary>
public class TimeManagement : MonoBehaviour
{

    public float second = 0;
    public int hour, minute = 0;
    public int month = 1;
    public int day = 1;
    public int year = 2020;
    public float timeScaleIncrement = 1;
    public float minTimeScale = 0.01f;
    public float maxTimeScale = 100f;

    private float fixedDeltaTime = new float();

    void Awoke(){
        fixedDeltaTime = Time.fixedDeltaTime;
    }
    void Update()
    {
        
        //A simulação possui diferentes velocidades a depender do valor dado a timescale
        //Pausa
        if(Input.GetKeyDown(KeyCode.Keypad0)){
            Time.timeScale = 0f;
        }
        //Normal
        else if(Input.GetKeyDown(KeyCode.Keypad1)){
            Time.timeScale = 1.0f;
        }
        //2x
        else if(Input.GetKeyDown(KeyCode.Keypad2)){
            Time.timeScale = 2.0f;
        }
        //5x
        else if(Input.GetKeyDown(KeyCode.Keypad3)){
            Time.timeScale = 5.0f;
        }
        //10x
        else if(Input.GetKeyDown(KeyCode.Keypad4)){
            Time.timeScale = 10f;
        }
        //15x
        else if(Input.GetKeyDown(KeyCode.Keypad5)){
            Time.timeScale = 15f;
        }
        passSecond();
    }

    ///<summary>Esse metodo recebe valores de <paramref name="hour"/> e <paramref name="minute"/>
    /// e retorna seu valor em segundos</summary>
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

    ///<summary>Retorna o tempo atual da simulação em segundos</summary>
    public float getTime(){
        return ToSecond(hour, minute) + second;
    }

     
    public int getDate(){
        return (year*10000)+(month*100)+day;//retorna a data como ano/mês/dia
    }
    ///<summary>Determina se hora atual está no intervalo definido por <paramref name="startHour"/>:<paramref name="startMinute"/>
    /// e <paramref name="endHour"/>:<paramref name="startMinute"/></summary>
    public bool isInInterval(int startHour, int startMinute, int endHour, int endMinute){
        return !(getTime() < ToSecond(startHour, endMinute)
        && getTime() > ToSecond(endHour, endMinute));
    }

    public void setTime(int newHour, int newMinute){
        hour = newHour;
        minute = newMinute;
    }
}
