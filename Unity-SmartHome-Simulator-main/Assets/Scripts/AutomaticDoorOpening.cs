using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<Summary>Altera a rotação da porta quando o colisor é ativado.</summary>
public class AutomaticDoorOpening : MonoBehaviour
{
    
    // Start is called before the first frame update
    public GameObject door;
    private float defaultDoorRotation;
    private float openDoorRotation;
    private float doorOpening = -90f;//90graus na direção oposta
    //public bool open = false;
    void Start()
    {
        defaultDoorRotation = door.transform.localRotation.y;
        openDoorRotation = door.transform.localRotation.y + doorOpening;
    }

    void OnTriggerEnter(Collider other){
        door.transform.eulerAngles = new Vector3(door.transform.eulerAngles.x, openDoorRotation, door.transform.eulerAngles.z);
    }
    void OnTriggerExit(Collider other){
        door.transform.eulerAngles = new Vector3(door.transform.eulerAngles.x, defaultDoorRotation, door.transform.eulerAngles.z);
    }
}
