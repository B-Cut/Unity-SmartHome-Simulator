using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticTVonoff : MonoBehaviour
{
    public Material tvLigada;
    public Material tvDesligada;
    private Renderer render;

    void Start(){
        render = GetComponent<Renderer>();
        render.material = tvDesligada;
    }
    private bool isPersonInside = false;
    void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            isPersonInside = true;
            Invoke("turnOnTV", 5f);
        }
    }

    void OnTriggerExit(Collider other){
        if(other.tag == "Player"){
            CancelInvoke("turnOnTV");
            isPersonInside = false;
            render.material = tvDesligada;
        }
    }
    void turnOnTV(){
        render.material = tvLigada;
    }
}
