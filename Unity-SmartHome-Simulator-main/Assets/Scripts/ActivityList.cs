using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


///<summary>Lista as atividades na UI</summary>
public class ActivityList : MonoBehaviour
{
    public Pessoa pessoa;
    private Text activityText;
    void Start()
    {
        activityText = GetComponent<Text>();
    }
    void Update()
    {
        string lista = "";
        string[] arrayAtividades = pessoa.getActivities();
        foreach(string activity in arrayAtividades){
            lista += activity + '\n';
        }
        activityText.text = lista;
    }
}
