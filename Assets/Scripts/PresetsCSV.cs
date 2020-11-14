using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
//Pega dados de um arquivo CSV e designa eles a respectiva variável

public class PresetsCSV : MonoBehaviour
{
    private Pessoa pessoa;
    public int numeroRotina = 0;
    private Dictionary<string, int> personRoutine = new Dictionary<string, int>();

    public TextAsset csvFile;
    private char lineSeparator = '\n';
    private char fieldSeparator = ',';

    void Start(){
        pessoa = GetComponent<Pessoa>();
        getRoutineData();
        setValueToVariable();
    }

    private void getRoutineData(){
        if(csvFile == null){
            Debug.LogError("O campo CSV é nulo");
            return;
        }
        string[] routines = csvFile.text.Split(lineSeparator);
        if(routines.Length <=1){
            Debug.LogError("O arquivo CSV deve ter ao menos duas colunas");
            return;
        }
        if(numeroRotina == 0){
            numeroRotina = (int) UnityEngine.Random.Range(1, routines.Length -1);
        }
        string[] fields = routines[0].Split(fieldSeparator);
        string[] selectedRoutine = routines[numeroRotina].Split(fieldSeparator);
        for(int i = 0; i < fields.Length; i++){
            personRoutine.Add(fields[i], int.Parse(selectedRoutine[i]));
        }
    }

    private void setValueToVariable(){
        pessoa.sleepTimeHour = personRoutine["sleepTimeHour"];
        pessoa.sleepTimeMinute = personRoutine["sleepTimeMinute"];

        pessoa.wakeUpTimeHour = personRoutine["wakeUpTimeHour"];
        pessoa.wakeUpTimeMinute = personRoutine["wakeUpTimeMinute"];

        pessoa.bathroomTimeHour = personRoutine["bathroomTimeHour"];
        pessoa.bathroomTimeMinute = personRoutine["bathroomTimeMinute"];
        
        pessoa.cookingTimeHour = personRoutine["cookTimeHour"];
        pessoa.cookingTimeMinute = personRoutine["cookTimeMinute"];
        
        pessoa.bathTimeHour = personRoutine["bathTimeHour"];
        pessoa.bathTimeMinute = personRoutine["bathTimeMinute"];
        
        pessoa.mealInterval = personRoutine["mealInterval"];
        pessoa.bathroomInterval = personRoutine["bathroomInterval"];
    }
}
