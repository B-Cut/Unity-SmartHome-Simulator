using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;

///<summary>Classe responsável por ler e executar os comandos contidos nos arquivos XML.</summary>
public class XmlActivitiesInterpreter : MonoBehaviour
{
    
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

    Vector3[] relaxPoints = new Vector3[5];

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
        pessoa = this.transform.GetComponentInParent<Pessoa>();//Isso funciona pois esse script pertence ao mesmo objeto que pessoa

        
        xmlActivities.Load(Application.dataPath + "/Scripts/Atividades.xml");
        xmlPlaces.Load(Application.dataPath + "/Scripts/Lugares.xml");
        XmlNode activityRoot = xmlActivities.SelectSingleNode("atividades");

        getRelaxPoints();

        Destination.arrived += OnArrived;//Adiciona função ao evento arrived.
    }


    void OnArrived(){
        this.atDestination = true;
    }

    public Hashtable getActivitiesTable(){
        return activitiesTable;
    } 

    IEnumerator to(string place){
        //Leva o usuário para o local indicado
        currentStep = "to";       
        atDestination = false;
        Vector3 dest = getPlaceFromXml(place);
        pessoa.changeDestination(dest);
        while(this.atDestination == false){
            if(this.executeHigherPriority){
                //Interrompe a atividade e salva o destino anterior a interrupção.
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
        //Essa corrotina deixa o usuário no lugar por um certo periodo de tempo
        this.currentStep = "wait";
        int waitTime = getTimeInSeconds(time);
        int endTime = (int) timeManagement.getTime() + waitTime;
        if(endTime > timeManagement.ToSecond(24, 00)){
            //Caso o horario do fim do passo exceda 24 horas, subtrai 24 horas para termos um horario de fim no dia seguinte
            endTime -= (int) timeManagement.ToSecond(24, 00);
        }
        while(timeManagement.getTime() <= endTime){
            if(executeHigherPriority){
                //Caso a atividade seja interrompida, salva a posição atual e o tempo restante de atividade.
                Vector3 previousPosition = pessoa.transform.position;
                Debug.Log("Atividade Interrompida" + this.currentActivity);
                string interruptedActivity = this.currentActivity;
                interruptedActivities.Push(interruptedActivity);

                if(endTime < timeManagement.getTime()){
                    //Se o tempo de final for menor que o tempo atual, somar 24 horas para definir o tempo restante.
                    endTime += (int) timeManagement.ToSecond(24, 00);
                }
                int remainingTime = endTime - (int) timeManagement.getTime();

                yield return StartCoroutine("ExecuteActivity", this.higherPriorityActivity);

                this.currentActivity = interruptedActivity;
                this.currentStep = "wait";
                pessoa.changeDestination(previousPosition);

                endTime = (int) timeManagement.getTime() + remainingTime;
                if(endTime > timeManagement.ToSecond(24, 00)){
                    endTime -= (int) timeManagement.ToSecond(24, 00);
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }


    
    IEnumerator wander(string argumento){
        //Toma 3 argumentos: raio, tempo de execução e centro. Argumentos separados por espaço
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
        //Leva o usuário para um dos pontos marcados como relaxPoint qunado não houver atividades a serem executadas.
        //Atividade não iniciada pelos arquivos XML
        this.currentActivity = "relax";
        this.isExecutingActivity = false;
        int randomRelaxPoint = UnityEngine.Random.Range(0, relaxPoints.Length - 1);
        Vector3 dest = relaxPoints[randomRelaxPoint];
        pessoa.changeDestination(dest);
        while(atDestination == false){
            if(pessoa.getActivityCount() > 0){
                yield break;
            }
        }
        while(pessoa.getActivityCount() == 0){
            yield return new WaitForEndOfFrame();
        }
    }
    ///<summary>O metodo <c>getTimeInSeconds</c> interpreta a string 
    ///<paramref name="time"/> e retorna seu valor em segundos</summary>
    int getTimeInSeconds(string time){
        int totalTime = 0;
        ///Separa a string em três partes e retorna o tempo em segundos da expressão///
        string[] separatedTime = new string[3];
        for(int i = 0; i<time.Length; i++){
            //Separa a string original em três substrings para hora, minuto, segundo
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
            if(value == null) continue;
            //Designa um valor a depender da letra no final da substring
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
        //Essa corrotina procura pela atividade em Atividades.xml e executa cada passo.
        Debug.Log("Executando " + name);
        this.isExecutingActivity = true;
        XmlNode activityRoot = xmlActivities.SelectSingleNode("atividades");
        if(currentActivity == "relax"){
            StopCoroutine("relax");
        }
        if(executeHigherPriority) {
            isExecutingHighPriorityActivity = true;//Essa parte é estranham, rever depois
        }

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
        //Executa a atividade com prioridade.
        //Basicamente a mesma coisa que ExecuteActivity, mas sem tirar uma nova atividade da stack.
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
                break;
            }
        }
    }

    ///<summary>O metodo <c>getPlaceFromXml</c> toma como argumento uma string <paramref name="place"/>
    ///e retorna um <example>Vector3</example> caso ela seja encontrada em <c>Lugares.xml</c></summary>
    
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

    ///<summary>O metodo <c>getPriority</c> toma como argumento uma string <paramref name="activityName"/>
    ///e retorna a prioridade da atividade caso ela esteja em <c>Atividades.xml</c>.</summary>
    public int getPriority(string activityName){
        XmlNode activityRoot = xmlActivities.SelectSingleNode("atividades");
        foreach(XmlNode activity in activityRoot.ChildNodes){
            if(activity.Attributes["name"].Value == activityName){
                return int.Parse(activity.Attributes["prioridade"].Value);
            }
        }
        return -1;
    }

   ///<summary>O metodo <c>getRelaxPoints</c> busca pelo atributo <c>relaxPoint</c> no arquivo 
   ///<c>Lugares.xml</c> e preenche o array <example>relaxPoints</example></summary>
   private void getRelaxPoints(){
        XmlNode placesRoot = xmlPlaces.SelectSingleNode("lugares");

        int counter = 0;
        foreach(XmlNode xmlPlace in placesRoot.ChildNodes){
            Vector3 pos = new Vector3();
            if(xmlPlace.Attributes["relaxPoint"] == null){
                continue;
            }
            else if(xmlPlace.Attributes["relaxPoint"].Value == "true"){
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
                            Debug.Log("Invalid coordinates!");
                            break;
                    }
                }
                relaxPoints[counter] = pos;
                counter++;
            }
        }
        return;
    }
    ///<summary>Retorna a stack <paramref name="interruptedActivities"/></summary>
    public Stack<String> getStack(){
        return interruptedActivities;
    }

    ///<summary>Retira uma atividade da lista de atividades pendentes</summary>
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
