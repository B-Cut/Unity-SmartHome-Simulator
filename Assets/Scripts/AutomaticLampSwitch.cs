using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticLampSwitch : MonoBehaviour
{
    public Light myLight;
    private RaycastHit hit;
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
        }   
        else{
            myLight.enabled = false;
        }
    }

    void OnTriggerStay(Collider other){
        Physics.Linecast(this.transform.position, other.transform.position, out hit);
        if(hit.collider.tag == "Player"){
            myLight.enabled = true;
        }
        else{
            myLight.enabled = false;
        }
    }

    void OnTriggerExit(Collider other){
        if(other.tag == "Player"){
            myLight.enabled = false;
            
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
}
