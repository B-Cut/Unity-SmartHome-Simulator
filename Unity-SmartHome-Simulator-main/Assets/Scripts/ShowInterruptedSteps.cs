using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowInterruptedSteps : MonoBehaviour
{
    public XmlActivitiesInterpreter XmlInterpreter;
    private Text interruptedText;
    private Stack<string> interruptedActivities = new Stack<string>();
    private string lista = "";
    // Start is called before the first frame update
    void Start()
    {
        interruptedText = this.transform.GetComponent<Text>();    
    }
    

    // Update is called once per frame
    void Update()
    {
        if(XmlInterpreter.getStack().Count == 0){
            lista = "";
        }
        else{
            foreach(string str in XmlInterpreter.getStack()){
                lista += str.ToString() + '\n';
            }
        }
        
        interruptedText.text = lista; 
    }

}
