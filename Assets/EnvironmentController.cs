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
    public TextMeshProUGUI generationText;
    public TextMeshProUGUI populationPeakText;
    public int foodAmount;
    public GameObject food;
    public Slider scatter;
    public Slider consistency;
    public Vector2 foodCenter;
    private int generation = 1;
    public List<int> historyOfPopulation;
    void Start()
    {
        foodCenter = new Vector2(0,0);
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
        generationText.text = "Current Generation: " + generation.ToString();

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
            recordPopulation(population);
            int maxValue = Mathf.Max(historyOfPopulation.ToArray());
            populationPeakText.text = "population peak: " + (maxValue).ToString();

            generation++;
            foodCenter = new Vector2(foodCenter.x + Random.Range(-consistency.value, consistency.value), foodCenter.y + Random.Range(-consistency.value/2, consistency.value/2));
            foreach (GameObject foodFromYesterday in GameObject.FindGameObjectsWithTag("food"))
            {
                Destroy(foodFromYesterday);
            }
            foodAmount = (int)slider.value;
          for (int i = 0; i < foodAmount; i++)
           {
                Vector3 foodLocation = new Vector3(foodCenter.x+ Random.Range(-6.0f*scatter.value, 6.0f * scatter.value), foodCenter.y+Random.Range(-3.0f * scatter.value, 3.0f * scatter.value), 0);
                Instantiate(food, foodLocation, Quaternion.identity);
            }
            Daytime = true;
            dayTimeTimer = 10;
           
        }
        void recordPopulation(int a)
        {
            historyOfPopulation.Add(a);
        }
    }
}
