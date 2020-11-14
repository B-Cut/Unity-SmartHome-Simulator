using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SensorRFID : MonoBehaviour
{
    public bool state = false;
    public float range = 10;
    private MeshRenderer sensorRenderer;
    private Material sensorMaterial;
    
    public Transform readerPos; 
    public Collider readerCollider;
    //public Transform thisPos;
    
    RaycastHit hit;
    // Start is called before the first frame update
    void Start(){
        //Pegar o material para mudar a cor do sensor
        sensorRenderer = GetComponent<MeshRenderer>();
        sensorMaterial = sensorRenderer.material;

        
    }

    //#if UNITY_EDITOR
        private void onDrawGizmos() {
            
            Gizmos.DrawWireSphere(transform.position, range);
            Gizmos.color = Color.yellow;
        }
    //#endif



    // Update is called once per frame-
    void FixedUpdate(){
        if(Vector3.Distance(transform.position, readerPos.position) <= range){//checa se o leitor está no alcance
            if(Physics.Linecast(transform.position, readerPos.position, out hit)){//checa se há algum obstaculo entre a posição do sensor e a do leitor
                if(hit.collider == readerCollider){
                    state = true;
                    sensorMaterial.color = Color.green;
                    //Debug.Log("Hit reader");
                }
                else{
                    //Debug.Log("Blocked");
                    //Debug.Log(hit.collider);
                    state = false;
                    sensorMaterial.color = Color.red;
                }
            }    
        }
        else{
            state = false;
            sensorMaterial.color = Color.red;
        } 
    }

}
