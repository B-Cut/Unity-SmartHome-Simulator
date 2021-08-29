using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///<summary>Essa classe controla o movimento do usuário</summary>

public class Destination : MonoBehaviour
{
    public delegate void arrivedOnDestination();
    public static event arrivedOnDestination arrived;
    public Pessoa pessoa;

    void OnTriggerEnter(Collider col){
        //Emite o sinal arrived quando o usuário entra na área do colisor.
        Debug.Log(col.name);
        Debug.Log(col.GetComponent<Pessoa>());
        if(arrived != null){ 
            arrived();
        }
    }

    ///<summary>Esse metodo gera um destino aleátorio para o usuário dentro da área da casa</summary>
    void RandomDestination(){
        Vector3 newDestination = new Vector3(Random.Range(-48f, 50f), 31f, Random.Range(-39f, 45f));//31f é aproximadamente a altura do chão
        transform.position = newDestination;
        pessoa.changeDestination(newDestination);
    }

    ///<summary>Esse método gera um destino aleatório dentro de um raio <paramref name="radius"/> com centro <paramref name="center"/></summary>
    public void RandomDestinationInArea(float radius, Vector3 center){
        Vector3 newDestination = center;
        //Gera um valor de x e z a partir de um centro e até uma distância definida do centro
        newDestination.x = Random.Range(center.x - radius, center.x + radius);
        newDestination.z = Random.Range(center.z - radius, center.z + radius);
        this.pessoa.changeDestination(newDestination);
    }
    ///<summary>altera a posição do colisor</summary>
    public void moveTo(Vector3 coord){
        this.transform.position = coord;
    }


}
