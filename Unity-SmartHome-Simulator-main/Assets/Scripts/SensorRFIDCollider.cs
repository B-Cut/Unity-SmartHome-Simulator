using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Essa é a implementação dos sensores RFID que utiliza sphere colliders para o fuincionamento do sensor

public class SensorRFIDCollider : MonoBehaviour
{

    public bool state = false;

    public LayerMask triggerLayers;//camadas que ligam o sensor, selecione todas menos "Ignore Raycast" no inspetor

    private Color sensorOriginalColor;
    private Material sensorMaterial;

    void Start(){
        sensorMaterial = GetComponent<MeshRenderer>().material;
        sensorOriginalColor = sensorMaterial.color;
    }
    void OnTriggerEnter(Collider other){//
        checkHit(other);
    }

    private void OnTriggerStay(Collider other){//enquanto o leitor se manter dentro do alcance do sensor
        checkHit(other);
    }

    private void OnTriggerExit(Collider other){
        if(other.tag == "Reader"){
            this.state = false;//retorna o estado do sensor para falso quando o leitor sair do alcance
        }
    }


    public bool getState(){
        return this.state;
    }
    private void checkHit(Collider other){
        if(other.tag == "Reader"){
            RaycastHit hit;
            Physics.Linecast(transform.position, other.transform.position, out hit);
            Debug.DrawLine(transform.position, other.transform.position, Color.white, 10f);
            
            if(hit.collider.tag == "Reader"){
                this.state = true;
                Debug.Log("Hit Sensor");
                this.sensorMaterial.color = Color.green;
            }
            else{
                this.state = false;
                //Debug.Log($"Hit {hit.collider.name}");
                this.sensorMaterial.color = sensorOriginalColor;
            }
        }
        else{//estado falso para qualquer outro colisor
            this.state = false;
            //Debug.Log(state);
            this.sensorMaterial.color = sensorOriginalColor;
        }
    }
    // Update is called once per frame
}

