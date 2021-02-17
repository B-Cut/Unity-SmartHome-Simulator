using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class showCurretActivity : MonoBehaviour
{

    public XmlActivitiesInterpreter xmlActivities;
    private Text currentText;

    // Start is called before the first frame update
    void Start(){
        currentText = GetComponent<Text>();
    }
    // Update is called once per frame
    void Update()
    {
        currentText.text = $"{xmlActivities.currentActivity}\n{xmlActivities.currentStep}";
    }
}
