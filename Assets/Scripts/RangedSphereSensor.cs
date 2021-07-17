using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedSphereSensor : BaseSensorScript
{
    public float range = 10f;

    

    // Start is called before the first frame update
    void Start()
    {   
    }

    // Update is called once per frame

    static int maxCollider = 10;
    static int collidedObjs = 0;
    Collider[] colliderBuffer = new Collider[maxCollider];
    void FixedUpdate()
    {
        collidedObjs = Physics.OverlapSphereNonAlloc(this.transform.position, range, colliderBuffer, layerFilter, QueryTriggerInteraction.Ignore);
        if(collidedObjs > 0){
            this.state = true;
        }
        else{
            this.state = false;
        }
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(this.transform.position, range);
        
    }
}
