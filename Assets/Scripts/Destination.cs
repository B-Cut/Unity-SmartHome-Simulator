using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Essa classe serve para controlar se o usuário entrou no Collider do seu destino

public class Destination : MonoBehaviour
{
    public delegate void arrivedOnDestination();
    public static event arrivedOnDestination arrived;
    // Start is called before the first frame update
    public Pessoa pessoa;

    /*void Start(){
        pessoa = GetComponentInParent<Pessoa>();
    }*/
    void OnTriggerEnter(){
        if(arrived != null){
            arrived();
        }   
    }

    void RandomDestination(){
        Vector3 newDestination = new Vector3(Random.Range(-48f, 50f), 31f, Random.Range(-39f, 45f));//31 é aproximadamente a altura do chão
        transform.position = newDestination;
        pessoa.changeDestination(newDestination);
    }

    public void RandomDestinationInArea(float radius, Vector3 center){
        Vector3 newDestination = center;
        //Gera um valor de x e z a partir de um centro e até uma distância definida do centro
        newDestination.x = Random.Range(center.x - radius, center.x + radius);
        newDestination.z = Random.Range(center.z - radius, center.z + radius);
        this.pessoa.changeDestination(newDestination);
    }
    public void moveTo(Vector3 coord){
        this.transform.position = coord;
    }


}
