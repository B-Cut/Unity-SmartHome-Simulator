using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Define as funções básicas de um sensor qualquer
//Tem como função ser um componente a ser colocado em um objeto


/* %%%%%Funções Básicas$$$$$
 -Estado(0/1)
 -referencia ao RB
 -filtro de camada
 -Funcções comuns a todos os sensores
*/
public class BaseSensorScript : MonoBehaviour
{
    public bool state = false;
    private Rigidbody sensorRB = new Rigidbody();
    
    public LayerMask layerFilter = new LayerMask();
    
    public bool getState(){
        return this.state;
    }
}
