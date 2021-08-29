using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>Define as funções básicas de um sensor qualquer</summary>


/* %%%%%Funções Básicas%%%%%
 -Estado(0/1)
 -referencia ao RB
 -filtro de camada
 -Funções comuns a todos os sensores
*/
public class BaseSensorScript : MonoBehaviour
{
    public bool state = false;
    private Rigidbody sensorRB = new Rigidbody();
    
    ///<summary>Filtros de camadas. Selecione as camadas a serem detectadas no editor do Unity</summary>
    public LayerMask layerFilter = new LayerMask();
    
    public bool getState(){
        return this.state;
    }
}
