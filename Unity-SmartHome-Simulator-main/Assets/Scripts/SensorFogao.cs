using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SensorFogao : MonoBehaviour
{
    public TimeManagement timeManagement;
    public bool ovenOn = false;
    public int sensorWaitMinutes = 5;//Contado em minutos
    // Start is called before the first frame update
    private Renderer rend;
    private Material ovenSensorMaterial;
    void Start(){
        rend = GetComponent<Renderer>();
        ovenSensorMaterial = rend.material;
    }

    void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            CancelInvoke("Alarm");
        }
    }

    void OnTriggerExit(Collider other){
        if(other.tag == "Player" && ovenOn){
            Invoke("Alarm", timeManagement.ToSecond(0, sensorWaitMinutes));//converter minuto para segundo
        }
    }

    void Alarm(){
        if(!ovenOn){
            return;
        }
        ovenOn = false;
        StartCoroutine(changeColor(Color.red, 5));
    }

    IEnumerator changeColor(Color color, int waitTimeMinutes){
        Color originalColor = ovenSensorMaterial.color;
        ovenSensorMaterial.color = color;
        yield return new WaitForSeconds(timeManagement.ToSecond(0, waitTimeMinutes));
        ovenSensorMaterial.color = originalColor;
    }
    
    public void turnOvenOn() => ovenOn = true;

    public void turnOvenOff() => ovenOn = false;
}
