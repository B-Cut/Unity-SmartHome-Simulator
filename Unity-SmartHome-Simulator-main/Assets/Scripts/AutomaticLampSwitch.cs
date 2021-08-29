using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>Liga e desliga lampadas a depender da entrada do usuário no colisor</summary>
public class AutomaticLampSwitch : MonoBehaviour
{

    public Light myLight;
    private RaycastHit hit = new RaycastHit();

    public bool state = false;

    private string user = "NA";
    public string sensorType = "Lamp";
    // Start is called before the first frame update
    void Start()
    {
        //myLight = GetComponent<Light>();
        myLight.enabled = false;
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other){
        Physics.Linecast(this.transform.position, other.transform.position, out hit);
        if(hit.collider.tag == "Player"){
            myLight.enabled = true;
            this.state = true;
            this.user = hit.transform.parent.name;
        }   
        else{
            myLight.enabled = false;
            this.state = false;
            this.user = "NA";
        }
    }

    void OnTriggerStay(Collider other){
        Physics.Linecast(this.transform.position, other.transform.position, out hit);
        if(hit.collider.tag == null){
            return;
        }
        if(hit.collider.tag == "Player"){
            myLight.enabled = true;
            this.state = true;
            this.user = hit.transform.parent.name;
        }
        else{
            myLight.enabled = false;
            this.state = false;
            this.user = "NA";
        }
    }

    void OnTriggerExit(Collider other){
        if(other.tag == "Player"){
            myLight.enabled = false;
            this.state = false;
            this.user = "NA";
        }
    }

    void OnDrawGizmos(){
        if(myLight.enabled){
            Gizmos.DrawIcon(transform.position, "LampOnAwesomeIcon", true);
            Gizmos.color = Color.white;
        }
        else{
            Gizmos.DrawIcon(transform.position, "LampOffAwesomeIcon", true);
            Gizmos.color = new Color(0f, 0f, 0f, 1f);
        }
    }

    public bool getState(){
        return this.state;
    }

    public string getUser(){
        return this.user;
    }
}
