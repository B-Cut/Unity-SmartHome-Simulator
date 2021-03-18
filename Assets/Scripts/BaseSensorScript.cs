using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Define as funções básicas de um sensor qualquer
//Tem como função ser um componente a ser colocado em um objeto
public class BaseSensorScript : MonoBehaviour
{
    public bool active = new bool();

    public float sensorRange = new float();
    private Rigidbody sensorRB = new Rigidbody();
    private SphereCollider sensorActiveArea = new SphereCollider();

    

    void Reset(){//No momento que o script é colocado em cena, cria esses dois objetos
        this.gameObject.AddComponent<Rigidbody>();
        this.gameObject.AddComponent<SphereCollider>();
        
        this.sensorRB = this.GetComponent<Rigidbody>();
        this.sensorActiveArea = this.GetComponent<SphereCollider>();

        sensorRB.constraints = RigidbodyConstraints.FreezeAll;//impede que o sensor se mova.
        sensorRB.useGravity = false;

        
    }
    
    void OnValidate(){
        sensorActiveArea.radius = sensorRange;
    }
    void OnDrawGizmos(){//Desenha o alcance do sensor
        Gizmos.DrawWireSphere(this.transform.position, this.sensorRange);
    }

    
}
