using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>Mostra a atividade atual na UI</summary>
public class showCurretActivity : MonoBehaviour
{

    public XmlActivitiesInterpreter xmlActivities;
    private Text currentText;

    void Start(){
        currentText = GetComponent<Text>();
    }
    void Update()
    {
        currentText.text = $"{xmlActivities.currentActivity}|{xmlActivities.currentStep}";
    }
}
