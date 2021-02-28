using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;

public class XmlActivitiesInterpreter : MonoBehaviour
{
    // Start is called before the first frame update

    //public Dictionary<string, Queue> = new Dictionary<string, Queue>();


    public delegate void finishedStep();
    public static event finishedStep stepEnded;

    public delegate void finishedActivity();
    public static event finishedActivity activityEnded;



    private bool atDestination = false;
    public string currentStep = "";
    public string currentActivity = "";
    public Hashtable activitiesTable = new Hashtable();
    
    XmlDocument xmlActivities = new XmlDocument();
    XmlDocument xmlPlaces = new XmlDocument();
    private Pessoa pessoa;
    private Destination destination;

    public TimeManagement timeManagement;

    public int currentActivityPriority = new int();

    public bool executeHigherPriority = false;
    public string higherPriorityActivity = "";

    public bool isExecutingActivity = false;
    //XmlNode placesRoot;

    void Start()
    {
        pessoa = this.transform.GetComponentInParent<Pessoa>();
        destination = this.transform.GetComponentInParent<Destination>();

        
        xmlActivities.Load(Application.dataPath + "/Scripts/Atividades.xml");
        xmlPlaces.Load(Application.dataPath + "/Scripts/Lugares.xml");
        XmlNode activityRoot = xmlActivities.SelectSingleNode("atividades");

        Destination.arrived += OnArrived;
        //StartCoroutine("ExecuteActivity", "1at");
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnArrived(){
        this.atDestination = true;
    }

    public Hashtable getActivitiesTable(){
        return activitiesTable;
    } 

    IEnumerator to(string place){
        currentStep = "to";       
        Vector3 dest = getPlaceFromXml(place);
        pessoa.changeDestination(dest);
        while(this.atDestination == false){
            if(this.executeHigherPriority){
                Debug.Log("Atividade Interrompida" + this.currentActivity);
                string interruptedActivity = this.currentActivity;
                yield return StartCoroutine("ExecuteHigherPriorityActivity", this.higherPriorityActivity);
                pessoa.changeDestination(dest);
                this.currentActivity = interruptedActivity;
                this.currentStep = "to";
            }
            yield return new WaitForEndOfFrame();
        }
        //yield return new WaitUntil(() => atDestination == true);
        /*if(stepEnded != null){
            stepEnded();
        }*/
    }

    IEnumerator wait(string time){
        this.currentStep = "wait";
        int waitTime = getTimeInSeconds(time);
        int endTime = (int) timeManagement.getTime() + waitTime;
        if(endTime > timeManagement.ToSecond(24, 00)){
            endTime -= (int) timeManagement.ToSecond(24, 00);
        }
        while(timeManagement.getTime() <= endTime){
            if(executeHigherPriority){
                Vector3 previousPosition = pessoa.transform.position;
                Debug.Log("Atividade Interrompida" + this.currentActivity);
                string interruptedActivity = this.currentActivity;
                yield return StartCoroutine("ExecuteHigherPriorityActivity", this.higherPriorityActivity);
                this.currentActivity = interruptedActivity;
                this.currentStep = "wait";
                pessoa.changeDestination(previousPosition);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator relax(){
        Debug.Log("relax in queue");
        this.isExecutingActivity = false;
        pessoa.changeDestination(getPlaceFromXml("sofa"));
        while(atDestination == false){
            if(pessoa.getActivityCount() > 0){
                break;//não é necessario outra ação pois a condição do while támbem retornará false
            }
        }
        while(pessoa.getActivityCount() == 0){
            yield return new WaitForEndOfFrame();
        }
    }
    int getTimeInSeconds(string time){
        int totalTime = 0;
        /*char[] separators = {'h', 'm', 's', ' '};
        string[] separatedTime = time.Split(separators, StringSplitOptions.RemoveEmptyEntries);*/
        string[] separatedTime = new string[3];
        for(int i = 0; i<time.Length; i++){
            int substringStartIndex = 0;
            int substringCounter = 0;
            if(Char.IsLetter(time[i])){
                separatedTime[substringCounter] = time.Substring(substringStartIndex, i - substringStartIndex + 1);
                substringStartIndex = i;
                substringCounter++;
            }
            if(substringCounter >= separatedTime.Length){
                Debug.Log("Maximum number of values hit");
                break;
            }
        }
        foreach(string value in separatedTime){
            if(value == null) break;

            int lastIndex = value.Length - 1;
            string newValue = "";
            switch(value[lastIndex]){
                case 'h':
                    newValue = value.Remove(lastIndex);
                    totalTime += int.Parse(newValue)*60*60;// number=>minute=>seconds
                    break;
                case 'm':
                    newValue = value.Remove(lastIndex);
                    totalTime += int.Parse(newValue)*60;//minute=>seconds
                    break;
                case 's':
                    newValue = value.Remove(lastIndex);
                    totalTime += int.Parse(newValue);
                    break;
            }
        }
        return totalTime;
    }

    IEnumerator ExecuteActivity(string name){
        Debug.Log("Executando " + name);
        this.isExecutingActivity = true;
        XmlNode activityRoot = xmlActivities.SelectSingleNode("atividades");
        executeHigherPriority = false;
        foreach(XmlNode activity in activityRoot.ChildNodes){
            if(activity.Attributes["name"].Value == name){
                currentActivityPriority = int.Parse(activity.Attributes["prioridade"].Value);
                currentActivity = name;
                foreach(XmlNode step in activity.ChildNodes){
                    string[] splitArgument = step.InnerText.Split(' ');
                    yield return StartCoroutine(splitArgument[0], splitArgument[1]);
                }
                if(activityEnded != null){
                    activityEnded();
                }
                break;
                
            }
        }
    }

    IEnumerator ExecuteHigherPriorityActivity(string name){
        this.isExecutingActivity = true;
        string interruptedStep = currentStep;
        Debug.Log("Começando atividade com prioridade " + name);
        XmlNode activityRoot = xmlActivities.SelectSingleNode("atividades");
        this.executeHigherPriority = false;
        foreach(XmlNode activity in activityRoot.ChildNodes){
            if(activity.Attributes["name"].Value == name){
                this.currentActivity = name;
                this.currentActivityPriority = int.Parse(activity.Attributes["prioridade"].Value);
                foreach(XmlNode step in activity.ChildNodes){
                    string[] splitArgument = step.InnerText.Split(' ');
                    yield return StartCoroutine(splitArgument[0], splitArgument[1]);
                }
                break;
            }
        }
    }

    Vector3 getPlaceFromXml(string place){
        XmlNode placesRoot = xmlPlaces.SelectSingleNode("lugares");
        Vector3 pos = new Vector3();

        foreach(XmlNode xmlPlace in placesRoot.ChildNodes){
            if(xmlPlace.Attributes["name"].Value == place){
                foreach(XmlNode coordinate in xmlPlace){
                    switch(coordinate.Name){
                        case "x":
                            pos.x = float.Parse(coordinate.InnerText, CultureInfo.InvariantCulture.NumberFormat);
                            break;
                        case "y":
                            pos.y = float.Parse(coordinate.InnerText, CultureInfo.InvariantCulture.NumberFormat);
                            break;
                        case "z":
                            pos.z = float.Parse(coordinate.InnerText, CultureInfo.InvariantCulture.NumberFormat);
                            break;
                        default:
                            Debug.Log("No valid coordinate found!");
                            break;
                    }
                }
                break;
            }
        }
        return pos;
    }

    public int getPriority(string activityName){
        XmlNode activityRoot = xmlActivities.SelectSingleNode("atividades");
        foreach(XmlNode activity in activityRoot.ChildNodes){
            if(activity.Attributes["name"].Value == activityName){
                return int.Parse(activity.Attributes["prioridade"].Value);
            }
        }
        return 0;
    }
}
