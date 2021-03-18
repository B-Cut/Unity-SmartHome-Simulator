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
        
        foreach(string str in interruptedActivities){
            lista += str.ToString() + '\n';
        }
        interruptedText.text = lista; 
    }

    
    public void addToActivityStack(string activity){
        this.interruptedActivities.Push(activity);
    }

    public void popFromActivityStack(){
        if(interruptedActivities.Count == 0){
            Debug.Log("Stack vazia");
            return;
        }
        else{
            this.interruptedActivities.Pop();
            lista = "";
        }
        
    }
}
