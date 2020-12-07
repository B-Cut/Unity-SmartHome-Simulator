using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Responsavel por adicionar múltiplos usuários


public class UserCount : MonoBehaviour
{
    //numero de usuarios EXTRAS adicionados a simulação
    public uint numberOfUsers = 0;
    private GameObject user;

    //SpawnOffset move o ponto de spawn a cada Instancia gerada
    //para Evitar que dois objetos se sobreponham
    public float XspawnOffset;
    public float ZspawnOffset;
    private Vector3 spawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        user = this.transform.Find("Usuario").gameObject; 
        spawnPoint = user.transform.position;
        var rotation = user.transform.rotation;//Uso de var temporario
        for(uint i = 0; i < numberOfUsers; i++){
            Instantiate(user, spawnPoint, rotation);
            spawnPoint.x += XspawnOffset;
            spawnPoint.z += ZspawnOffset; 
        }
    }
}
