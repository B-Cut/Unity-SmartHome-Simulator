using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SensorGas : MonoBehaviour
{
    public float gasTreshhold = 0.25f;//limite para o alarme disparar. Medido de 0.0 a 1.0
    public float triggerChance = 0.2f;//chance de a quantidade de gás aumentar
    public float gasIncrement = 0.05f;//incremento na quantidade de gás
    public float gasDecrement = 0.02f;//decremento na quantidade de gás
    public float gasAmount = 0.1f;
    public float gasBase = 0.1f;//valor para o alarme parar de tocar

    private Renderer rend;
    private Material gasSensorMaterial;
    void Start(){
        rend = GetComponent<Renderer>();
        InvokeRepeating("incrementGas", 20f, 10f);
        gasSensorMaterial = rend.material;
    }
    // Update is called once per frame
    void Update()
    {
        if(gasAmount >= gasTreshhold){
            CancelInvoke("incrementGas");
            StartCoroutine("alarm");
        }
    }

    void incrementGas(){
        if(UnityEngine.Random.Range(0f, 1f) <= triggerChance){
            gasAmount += gasIncrement;
        }
    }

    IEnumerator alarm(){
        bool materialSwitcher = false; //switching between 2 materials
        Debug.Log("Too much gas");
        while(gasAmount > gasBase){
            if(materialSwitcher){
                gasSensorMaterial.color = Color.red;
            }
            else gasSensorMaterial.color = Color.white;
            
            materialSwitcher = !materialSwitcher;
            gasAmount -= gasDecrement;
            yield return new WaitForSeconds(1f);
        }
        gasSensorMaterial.color = Color.white;
        InvokeRepeating("incrementGas", 5f, 10f);
    }
}
