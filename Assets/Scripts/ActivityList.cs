using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivityList : MonoBehaviour
{
    public Pessoa pessoa;
    private Text activityText;
    // Start is called before the first frame update
    void Start()
    {
        activityText = GetComponent<Text>();
    }

    // Update is called once per frame
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
