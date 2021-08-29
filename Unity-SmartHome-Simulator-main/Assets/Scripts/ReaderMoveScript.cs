using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaderMoveScript : MonoBehaviour
{
    public Transform readerTransform;
    private Rigidbody sensorRigidBody;
    // Start is called before the first frame update
    void Start()
    {
        sensorRigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        sensorRigidBody.MovePosition(readerTransform.position);
    }
}
