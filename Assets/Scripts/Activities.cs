using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


//Essa classe define todas as atividades da rotina do usuário

public class Activities : MonoBehaviour
{
    //Esse evento será disparado ao final de uma atividade
    public delegate void finishedActivity();
    public static event finishedActivity activityEnded;


    //Locations NÃO são a posição exata do objeto, elas servem para navegar o usuário até esse objetos
    public Pessoa pessoa;
    public Vector3 fogaoLocation;
    public Vector3 chuveiroLocation;
    public Vector3 sofaLocation;
    public Vector3 mesaJantarLocation;
    public Vector3 camaLocation;
    public Vector3 vasoLocation;
    public Vector3 geladeiraLocation;

    public Vector3 armarioLocation;

    //Duração das atividades
    public int sleepDurationHour = 8;
    public int sleepDurationMinute = 30;
    public int cookDurationHour = 0;
    public int cookDurationMinute = 30;
    public int bathroomDurationHour = 0;
    public int bathroomDurationMinute = 2;
    public int bathDurationHour = 0;
    public int bathDurationMinute = 30;
    public int eatDurationHour = 0;
    public int eatDurationMinute = 30;


    public TimeManagement timeManagement;
    public SensorFogao sensorFogao;
    private float timeNow;

    
    public Destination destination;
    private bool atDestination = false;

    public Animator anim;
    
    void Start(){
        Destination.arrived += OnArrived;       
    }   

    void OnArrived(){
        atDestination = true;
        anim.ResetTrigger("Walk");
        anim.SetTrigger("GoIdle");
    }

    //Esses metodos serão definidos como IEnumerators para serem usados como coroutines
    IEnumerator WaitForEvent(){
        atDestination = false;
        yield return new WaitUntil(() => atDestination == true);//condição deve ser passada como uma função Lambda
        
    }
    IEnumerator cook(){
        pessoa.setCurrentActivity("Caminhando");
        pessoa.changeDestination(fogaoLocation);
        yield return StartCoroutine(WaitForEvent());//Espera o usuário chegar ao fogão
        //no fogão
        sensorFogao.turnOvenOn();
        pessoa.setCurrentActivity("Cozinhando");
        timeNow = timeManagement.getTime();
        anim.SetBool("isIdle", true);
        yield return StartCoroutine(customWaitForSeconds(cookDurationHour, cookDurationMinute));
        if(!pessoa.forgot()){
            sensorFogao.turnOvenOff();
        }

        pessoa.pushToQueue("eat");

        if(activityEnded != null){
            activityEnded();
        }
    }

    IEnumerator useBathroom(){
        pessoa.changeDestination(vasoLocation);
        pessoa.setCurrentActivity("Caminhando");
        yield return StartCoroutine(WaitForEvent());
        pessoa.setCurrentActivity("Usando o Banheiro");
        timeNow = timeManagement.getTime();
        float endBathroom = timeNow + timeManagement.ToSecond(bathroomDurationHour, bathroomDurationMinute);
        if(endBathroom >= timeManagement.ToSecond(24)){
            endBathroom -= timeManagement.ToSecond(24);
        }
        //yield return new WaitUntil(() => timeManagement.getTime() >= endBathroom);
        yield return StartCoroutine(customWaitForSeconds(bathroomDurationHour, bathroomDurationMinute));
        if(activityEnded != null){
            activityEnded();
        }
    }

    IEnumerator takeBath(){
        pessoa.changeDestination(chuveiroLocation);
        pessoa.setCurrentActivity("Caminhando");
        yield return StartCoroutine(WaitForEvent());
        pessoa.setCurrentActivity("Tomando Banho");
        timeNow = timeManagement.getTime();
        yield return StartCoroutine(customWaitForSeconds(bathDurationHour, bathDurationMinute));
        //yield return StartCoroutine("trocarDeRoupa");
        pessoa.changeDestination(armarioLocation);
        pessoa.setCurrentActivity("Caminhando");
        yield return StartCoroutine(WaitForEvent());//Esse return faz com que a função termine prematuramente se usada dentro de outra atividade
        pessoa.setCurrentActivity("Trocando de roupa");
        yield return StartCoroutine(customWaitForSeconds(0, 2));
        if(activityEnded != null){
            activityEnded();
        }
    }

    IEnumerator sleep(){
        pessoa.setCurrentActivity("Caminhando");
        pessoa.changeDestination(camaLocation);
        yield return StartCoroutine(WaitForEvent());
        pessoa.setCurrentActivity("Dormindo");
        timeNow = timeManagement.getTime();
        yield return new WaitWhile(() => timeManagement.isInInterval(pessoa.sleepTimeHour, pessoa.sleepTimeMinute,
                                                                pessoa.wakeUpTimeHour, pessoa.wakeUpTimeMinute));
        //yield return new WaitUntil(() => timeManagement.getTime() >= timeNow + timeManagement.ToSecond(sleepDurationHour, sleepDurationMinute));
        pessoa.sleepInQueue = false;
        if(activityEnded != null){
            activityEnded();
        }
    }

    IEnumerator relax(){
        if(pessoa.getActivityCount() != 0){
            if(activityEnded != null){
                activityEnded();
            }
            pessoa.isRelaxing = false;
            yield break;//Para a função caso uma outra função entre na fila de atividades
        }
        pessoa.changeDestination(sofaLocation);
        pessoa.setCurrentActivity("Caminhando");
        yield return StartCoroutine(WaitForEvent());
        pessoa.setCurrentActivity("Relaxando");
        yield return new WaitUntil(() => pessoa.getActivityCount() != 0);
        pessoa.isRelaxing = false;
        if(activityEnded != null){
            activityEnded();
        }
    }    
    /*IEnumerator trocarDeRoupa(){
        pessoa.changeDestination(armarioLocation);
        pessoa.setCurrentActivity("Caminhando");
        yield return StartCoroutine(WaitForEvent());//Esse return faz com que a função termine prematuramente se usada dentro de outra atividade
        pessoa.setCurrentActivity("Trocando de roupa");
        yield return StartCoroutine(customWaitForSeconds(0, 2));
        //Não invocar activityEnded(), pois essa atividade irá ocorrer dentro de outras.
    }*/
    //A função WaitForSeconds do unity não estava escalando com a timeScale, então criei uma função propria com o mesmo objetivo

    IEnumerator eat(){
        pessoa.setCurrentActivity("Caminhando");
        pessoa.changeDestination(geladeiraLocation);
        yield return StartCoroutine(WaitForEvent());
        pessoa.changeDestination(mesaJantarLocation);
        yield return StartCoroutine(WaitForEvent());
        pessoa.setCurrentActivity("Comendo");
        yield return StartCoroutine(customWaitForSeconds(eatDurationHour, eatDurationMinute));
        if(activityEnded != null){
            activityEnded();
        }
    }

    IEnumerator customWaitForSeconds(int waitHour, int waitMinute){
        float secondsPassed = 0f;
        while(secondsPassed <= timeManagement.ToSecond(waitHour, waitMinute)){
            secondsPassed +=  1.0f * Time.timeScale * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }


}
