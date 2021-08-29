using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///<summary>Sensor baseado no sistema de colisões do Unity</summary>
public class PresenceSensor : BaseSensorScript
{
    private SphereCollider sphereCollider;
    private BoxCollider boxCollider;
    void Awake()
    {
        sphereCollider = this.GetComponent<SphereCollider>();
        boxCollider = this.GetComponent<BoxCollider>();
    }
    
    //Para o OnTrigger funcionar um dos objetos deve ter um RigidBody e Collider.isTrigger verdadeiro
    void OnTriggerEnter(Collider collider){
        if(Physics.Linecast(this.transform.position, collider.transform.position, this.layerFilter, QueryTriggerInteraction.Ignore)){
            this.state = true;
            Debug.DrawLine(this.transform.position, collider.transform.position, Color.white, 1f);
            Debug.Log($"{collider.name} entrou no colisor");
        }
    }
    void OnTriggerStay(Collider collider){
        if(Physics.Linecast(this.transform.position, collider.transform.position, this.layerFilter)){
            this.state = true;
        }
        else {
            this.state = false;
        }
    }

    void OnTriggerExit(Collider collider){
        this.state = false;
        Debug.Log($"{collider.name} saiu do colisor");
    }


    void OnDrawGizmos(){
        if(boxCollider != null){
            Gizmos.DrawWireCube(boxCollider.transform.position, boxCollider.bounds.size);
            Gizmos.color = Color.cyan;
        }
        else if(sphereCollider != null){
            Gizmos.DrawWireSphere(sphereCollider.transform.position, sphereCollider.radius);
            Gizmos.color = Color.cyan;
        }
    }

    void OnDrawGizmosSelected(){
        Gizmos.color = new Color(0f, 0f, 0f, 0f);//Transparente quando selecionado
    }

}
