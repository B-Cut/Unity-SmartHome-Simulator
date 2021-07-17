using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[SerializeField]
public class EditorScriptCustomObjects : MonoBehaviour
{
    

    static Vector3 sensorPlaneSize = new Vector3(0.01f, 1f, 0.004f);

    [MenuItem("GameObject/3D Object/SphereBaseSensor")]
    private static void createSphereBaseSensor(){
        GameObject sensor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        sensor.name = "SpherePresenceSensor";
        sensor.AddComponent<BaseSensorScript>();
        sensor.AddComponent<SphereCollider>();
        sensor.AddComponent<Rigidbody>();
        DestroyImmediate(sensor.GetComponent<MeshCollider>());

        sensor.transform.localScale = sensorPlaneSize;

        SphereCollider collider = sensor.GetComponent<SphereCollider>();
        collider.isTrigger = true;
        collider.radius = 1;

        Rigidbody rigidbody = sensor.GetComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        rigidbody.useGravity = false;
    }

    [MenuItem("GameObject/3D Object/BoxBaseSensor")]
    private static void createBoxBaseSensor(){
        GameObject sensor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        sensor.name = "BoxPresenceSensor";
        sensor.AddComponent<BaseSensorScript>();
        sensor.AddComponent<BoxCollider>();
        sensor.AddComponent<Rigidbody>();
        DestroyImmediate(sensor.GetComponent<MeshCollider>());

        sensor.transform.localScale = sensorPlaneSize;

        BoxCollider collider = sensor.GetComponent<BoxCollider>();
        collider.isTrigger = true;
        collider.size = new Vector3(11f, 0.1f, 9f);

        Rigidbody rigidbody = sensor.GetComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        rigidbody.useGravity = false;
    }
}
