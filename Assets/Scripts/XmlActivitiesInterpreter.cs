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
    public Destination destination;

    public TimeManagement timeManagement;

    public int currentActivityPriority = new int();

    public bool executeHigherPriority = false;
    public string higherPriorityActivity = "";

    public bool isExecutingActivity = false;
    public bool isExecutingHighPriorityActivity = false;
    //XmlNode placesRoot;

    private Stack<string> interruptedActivities = new Stack<string>();
    void Start()
    {
        pessoa = this.transform.GetComponentInParent<Pessoa>();

        
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
                Vector3 previousPlace = dest;
                interruptedActivities.Push(interruptedActivity);
                yield return StartCoroutine("ExecuteActivity", this.higherPriorityActivity);
                pessoa.changeDestination(previousPlace);
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
                interruptedActivities.Push(interruptedActivity);
                int remainingTime = endTime - (int) timeManagement.getTime();
                yield return StartCoroutine("ExecuteActivity", this.higherPriorityActivity);
                this.currentActivity = interruptedActivity;
                this.currentStep = "wait";
                pessoa.changeDestination(previousPosition);
                endTime = (int) timeManagement.getTime() + remainingTime;
            }
            yield return new WaitForEndOfFrame();
        }
    }


    //Toma 3 argumentos: raio, tempo de execução e centro
    IEnumerator wander(string argumento){
        string[] argumentosSeparados = argumento.Split(' ');
        if(argumentosSeparados.Length < 3){
            Debug.Log("Not enough arguments in wander step");
            yield break;
        }
        float radius = float.Parse(argumentosSeparados[0], CultureInfo.InvariantCulture);
        float endTime = timeManagement.getTime() + getTimeInSeconds(argumentosSeparados[1]);
        float nextDestinationTime = timeManagement.getTime();
        Vector3 center = getPlaceFromXml(argumentosSeparados[2]);
        //yield return StartCoroutine("to", argumentosSeparados[2]);
        while(timeManagement.getTime() <= endTime){
            if(atDestination == true){
                nextDestinationTime += timeManagement.ToSecond(0, 2);
                atDestination = false;
            }
            if(timeManagement.getTime() >= nextDestinationTime){
                destination.RandomDestinationInArea(radius, center);
            }

            if(this.executeHigherPriority){
                Vector3 currentPos = this.pessoa.transform.position;
                Debug.Log("Atividade Interrompida" + this.currentActivity);
                string interruptedActivity = this.currentActivity;
                interruptedActivities.Push(interruptedActivity);
                yield return StartCoroutine("ExecuteActivity", this.higherPriorityActivity);
                pessoa.changeDestination(currentPos);
                this.currentActivity = interruptedActivity;
                this.currentStep = "wander";
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
        if(executeHigherPriority) isExecutingHighPriorityActivity = true;
        executeHigherPriority = false;
        foreach(XmlNode activity in activityRoot.ChildNodes){
            if(activity.Attributes["name"].Value == name){
                currentActivityPriority = int.Parse(activity.Attributes["prioridade"].Value);
                currentActivity = name;
                foreach(XmlNode step in activity.ChildNodes){
                    string[] splitArgument = step.InnerText.Split(' ');
                    switch(splitArgument[0]){
                        case "to":
                        case "wait": 
                                yield return StartCoroutine(splitArgument[0], splitArgument[1]);
                                break;
                        case "wander": 
                                string joinedString = String.Join(" ", splitArgument[1], splitArgument[2], splitArgument[3]);
                                yield return StartCoroutine(splitArgument[0], joinedString);
                                break;
                        default:
                                Debug.Log("no step named " + step);
                                break;
                    }
                }
                isExecutingActivity = false;
                if(activityEnded != null && !isExecutingHighPriorityActivity){
                    activityEnded();
                }
                popFromActivityStack();
                Debug.Log("Fim da atividade");
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

    public Stack<String> getStack(){
        return interruptedActivities;
    }

    public void popFromActivityStack(){
        if(interruptedActivities.Count == 0){
            Debug.Log("Stack vazia");
            isExecutingHighPriorityActivity = false;//Como não há atividades interrompidas na stack, não há atividade prioritária em execução
            return;
        }
        else{
            this.interruptedActivities.Pop();
        }   
    }
}
