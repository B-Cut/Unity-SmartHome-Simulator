using UnityEngine.UI;
using UnityEngine;

///<summary>Atualiza o relogio da UI</summary>
public class Clock : MonoBehaviour
{
    public TimeManagement simulationTime;
    private Text clockText;
    void Start()
    {
        clockText = GetComponent<Text>();

    }

    void Update()
    {
        // a cada frame o tempo é atualizado
        clockText.text = $"{simulationTime.day}/{simulationTime.month} | {simulationTime.hour}:{simulationTime.minute}:{Mathf.Round(simulationTime.second)}";
        
    }
}
