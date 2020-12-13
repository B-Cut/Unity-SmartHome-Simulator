using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private bool camMovLocked = false;//Trava a camera

    //public Camera activeCamera;
    public float verticalSpeed;//Velocidade para o movimento puramente vertical
    public float flySpeed;//Velocidade do vôo da camera

    //Sensitividade do movimento da camera
    public float sensitivity;


    //variáveis para o movimento local da camera(Frente, trás, lados)
    private float hMovement;//lados
    private float vMovement;//frente/trás

    //variavéis para rotacionar a camera de acordo com o mouse
    private float hRotation;
    private float vRotation;
    void Start()
    {
        Debug.Log("Hello World");
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("tab")){
            camMovLocked = !camMovLocked;
            Cursor.lockState = CursorLockMode.None;
        }
        if(!camMovLocked){
            Cursor.lockState = CursorLockMode.Locked;
            if(Input.GetKey("e")){
                transform.Translate(0, verticalSpeed * Time.fixedDeltaTime, 0, Space.World);
            }
            else if(Input.GetKey("q")){
                transform.Translate(0, -verticalSpeed * Time.fixedDeltaTime, 0, Space.World);
            }
            else{
                hMovement = Input.GetAxis("Horizontal") * Time.fixedDeltaTime * flySpeed;
                vMovement = Input.GetAxis("Vertical") *Time.fixedDeltaTime * flySpeed;

                transform.Translate(hMovement, 0, vMovement);
            }


            hRotation = sensitivity * Input.GetAxis("Mouse Y") * -1;
            vRotation = sensitivity * Input.GetAxis("Mouse X") ;

            transform.Rotate(hRotation, vRotation, 0);
            float z = transform.eulerAngles.z;
            transform.Rotate(0, 0, -z);
        }
    }
}
