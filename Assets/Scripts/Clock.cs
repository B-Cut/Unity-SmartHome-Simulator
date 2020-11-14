using UnityEngine.UI;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public TimeManagement simulationTime;
    private Text clockText;
    // Start is called before the first frame update
    void Start()
    {
        clockText = GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        // a cada frame o tempo é atualizado

        simulationTime.passSecond();
        clockText.text = $"{simulationTime.day}/{simulationTime.month} | {simulationTime.hour}:{simulationTime.minute}:{Mathf.Round(simulationTime.second)}";
        
    }


    /*public void passSecond(){
        second += 1 * Time.timeScale * Time.deltaTime;//o atributo timeScale controla a velocidade da simulação.
        if(second >= 60){
            second = 0;
            minute++;
        }
        if(minute >= 60){
            minute = 0;
            hour++;
        }
        if(hour >= 24){
            day++;
            hour = 0;
        }
        if(day >= 30){
            day = 1;
            month++;
        }
        if(month >=12){
            month = 1;
            year++;
        }
    }*/
}
