using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SensorRFID : MonoBehaviour
{
    public bool state = false;
    public float range = 10f;
    private MeshRenderer sensorRenderer = new MeshRenderer();
    private Material sensorMaterial;

    public string sensorType = "Sensor RFID";

    private string user = "NA";
    
    public Transform readerPos; 
    public Collider readerCollider;
    //public Transform thisPos;
    public LayerMask layerMask; //No editor selecione tudo menos as camadas que você quer ignorar
                                //Ignore Raycast tem que ser deselecionada manualmente
    
    RaycastHit hit = new RaycastHit();
    // Start is called before the first frame update
    void Start(){
        //Pegar o material para mudar a cor do sensor
        sensorRenderer = GetComponent<MeshRenderer>();
        sensorMaterial = sensorRenderer.material;        
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, (float) range);  
    }



    // Update is called once per frame-
    void FixedUpdate(){
        if(Vector3.Distance(this.transform.position, this.readerPos.position) <= this.range){//checa se o leitor está no alcance         
            if(Physics.Linecast(this.transform.position, readerPos.position, out this.hit, this.layerMask)){//checa se há algum obstaculo entre a posição do sensor e a do leitor
                if(this.hit.collider == readerCollider){
                    Debug.DrawLine(this.transform.position, readerPos.position, Color.white, 10f);
                    this.user = this.hit.transform.parent.name;
                    this.state = true;
                    this.sensorMaterial.color = Color.green;
                    //Debug.Log(this.name + ": Hit reader");
                }
                else{
                    //Debug.Log("Blocked by" + this.hit.collider.name);
                    //Debug.Log(hit.collider);
                    this.user = "NA";
                    this.state = false;
                    this.sensorMaterial.color = Color.red;
                }
            }    
        }
        else{
            this.user = "NA";
            this.state = false;
            this.sensorMaterial.color = Color.red;
        } 
    }

    public bool getState(){
        return this.state;
    }

    public string getUser(){
        return this.user;
    }
}
