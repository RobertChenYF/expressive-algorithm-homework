using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnvironmentController : MonoBehaviour
{
    // Start is called before the first frame update

    public static float landSize;
    
    public static bool Daytime = true;
    public static float dayTimeTimer = 10;
    public static int state;
    public Slider slider;
    public TextMeshProUGUI textMeshUI;
    public TextMeshProUGUI foodAmountText;
    public TextMeshProUGUI PressSpace;
    public TextMeshProUGUI PopulationText;
    public int foodAmount;
    public GameObject food;

    void Start()
    {
        for (int i = 0; i < foodAmount; i++)
        {
            Vector3 foodLocation = new Vector3(Random.Range(-6.0f, 6.0f), Random.Range(-3.0f, 3.0f), 0);
            Instantiate(food, foodLocation, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        textMeshUI.text = "Time left: " + ((int)dayTimeTimer).ToString();
        foodAmountText.text = "Food Amount: " + ((int)(slider.value)).ToString();


        int population = 0;
        foreach (GameObject creature in GameObject.FindGameObjectsWithTag("Player"))
        {
            population++;
        }
        PopulationText.text = "Poluation: " + population.ToString();
        if (Daytime)
        {
            dayTimeTimer -= Time.deltaTime;
            PressSpace.enabled = false;
        }
        if (dayTimeTimer<= 0)
        {
            
            Daytime = false;
            PressSpace.enabled = true;
           
        }
        if (Input.GetKeyDown(KeyCode.Space) && dayTimeTimer <= 0)
        {

            foreach (GameObject foodFromYesterday in GameObject.FindGameObjectsWithTag("food"))
            {
                Destroy(foodFromYesterday);
            }
            foodAmount = (int)slider.value;
          for (int i = 0; i < foodAmount; i++)
           {
                Vector3 foodLocation = new Vector3(Random.Range(-6.0f, 6.0f), Random.Range(-3.0f, 3.0f), 0);
                Instantiate(food, foodLocation, Quaternion.identity);
            }
            Daytime = true;
            dayTimeTimer = 10;
            
        }

    }
}
