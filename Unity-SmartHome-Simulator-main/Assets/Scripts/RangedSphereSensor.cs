using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>Essa classe detecta se há objetos pertencentes a camada designada dentro da área definida.</summary>
public class RangedSphereSensor : BaseSensorScript
{
    public float range = new float();


    static int maxCollider = 10;
    int collidedObjs = 0;
    public Collider[] colliderBuffer = new Collider[maxCollider];
    RaycastHit hit = new RaycastHit();
    void FixedUpdate()
    {
        //layerFilter é um membro de BaseSensorScript
        //Retorna array contendo todos os colisores
        this.colliderBuffer = Physics.OverlapSphere(this.transform.position, this.range, this.layerFilter, QueryTriggerInteraction.Ignore);
        
        if(this.colliderBuffer.Length == 0){
            this.state = false;
        }
        else{
            foreach(Collider obj in this.colliderBuffer){
                Debug.DrawLine(this.transform.position, obj.transform.position);
                if(Physics.Linecast(this.transform.position, obj.transform.position, out hit)
                && (this.layerFilter & (1 << obj.gameObject.layer)) != 0){//Essa expressão testa se camada está dentro do filtro


                    if((this.layerFilter & (1 << hit.transform.gameObject.layer)) != 0){
                        
                        this.state = true;
                        break;
                    }
                    else{
                        this.state = false;
                    }
                }
                else{
                    this.state = false;
                }
                
            }
        }
        
    }

    void OnDrawGizmos(){
        if(this.state == true){
            Gizmos.color = Color.green;
        }else{
            Gizmos.color = Color.magenta;
        }      
        Gizmos.DrawWireSphere(this.transform.position, range); 
    }
}
