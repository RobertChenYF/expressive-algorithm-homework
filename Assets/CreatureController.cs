using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed = 3;
    private float currentSpeed;
    public float energy = 1;
    public float SenseCircleSize = 1;
    public Collider2D senseCircle;
    private int foodFound = 0;
    private bool insideSafeZone;
    public Rigidbody2D rigidbodyOfCreature;
    public Collider2D creatureBody;
    //private bool targetLocked = false;
    
    public GameObject creature;
    private int dayExisted = 0;
    private int NightCycleTime = 0;
    
    private Vector3 home;
    private GameObject targetFood;
    private Vector3 upFromLastFrame;
    private float timerForDirectionChange = 3f;
    private bool BackHome = false;
    private bool NightRoutineRun = false;
    private bool DayRoutineRun = true;
    public bool justSpawned = true;
    public GameObject detectionCycle;
    private BoxCollider2D foodSpawnBox;
    private SpriteRenderer spriteRenderer;
    public int currentGeneration = 1;
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        
        currentSpeed = speed;
        rigidbodyOfCreature = gameObject.GetComponent<Rigidbody2D>();
        home = transform.position;
        DayRoutineRun = false;
        DayTimeStart(DayRoutineRun);
        foodSpawnBox = GameObject.Find("FoodSpawnBox").GetComponent<BoxCollider2D>();
        spriteRenderer.color = new Color(0.8f- (speed-3)*0.7f,1,0.8f-(SenseCircleSize-1));
        detectionCycle.transform.localScale = new Vector3(SenseCircleSize , SenseCircleSize, 1);
    }

    // Update is called once per frame
    void Update()
    {

        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (foodFound >= 2)
            {
                //reproduce

                Vector3 newHomeLocation = transform.position + new Vector3(Random.Range(-0.6f, 0.6f), Random.Range(-0.6f, 0.6f), 0);
              //  while (foodSpawnBox.bounds.Contains(newHomeLocation))
              //  {
              //      newHomeLocation = transform.position + new Vector3(Random.Range(-0.6f, 0.6f), Random.Range(-0.6f, 0.6f), 0);
               // }
                GameObject NewBornCreature = Instantiate(creature, transform.position + new Vector3(Random.Range(0.5f, 0.6f), Random.Range(0.5f, 0.6f), 0), Quaternion.identity);
                NewBornCreature.GetComponent<CreatureController>().speed = speed + Random.Range(-0.5f,0.5f);
                NewBornCreature.GetComponent<CreatureController>().currentGeneration = currentGeneration + 1;
                NewBornCreature.GetComponent<CreatureController>().SenseCircleSize = SenseCircleSize + Random.Range(-0.1f,0.1f);
                NewBornCreature.name = "creature generation" + (currentGeneration+1).ToString();
                print("reproduce");
                //newCreature.GetComponents<CreatureController>(). = ;
            }

            GameObject newCreature = Instantiate(creature, transform.position, Quaternion.identity);
            newCreature.GetComponent<CreatureController>().speed = speed;
            newCreature.GetComponent<CreatureController>().currentGeneration = currentGeneration;
            newCreature.GetComponent<CreatureController>().SenseCircleSize = SenseCircleSize;
            newCreature.name = "creature generation" + (currentGeneration).ToString();
            Destroy(gameObject);
        }
        
        if (EnvironmentController.Daytime == true)
        {
            justSpawned = false;

            if (targetFood == null && BackHome == false)
            {//wandering
                transform.up = upFromLastFrame;
            if (timerForDirectionChange <= 0)
            {
                timerForDirectionChange = 1.5f;
                transform.Rotate(0,0,Random.Range(-50f,50f));
                upFromLastFrame = transform.up;
            }

            timerForDirectionChange -= Time.deltaTime;
                rigidbodyOfCreature.velocity = transform.up * currentSpeed;
            }
        else if (targetFood != null && BackHome == false)
            {
                //sense food
                transform.up = targetFood.transform.position - transform.position; 
                rigidbodyOfCreature.velocity = transform.up * currentSpeed;
            }
         if ((((foodFound >= 2) || ((foodFound == 1) && EnvironmentController.dayTimeTimer < 4))) && (Vector2.Distance(home, transform.position) > 0.1f))
        {
            //go back to home
            BackHome = true;
                print("going home");
                transform.up = home - transform.position;
                rigidbodyOfCreature.velocity = transform.up * currentSpeed;
            }
           if(BackHome && Vector2.Distance(home,transform.position) < 0.1f)
            {
                insideSafeZone = true;
                print("home success");
                currentSpeed = 0;
            }  

          
        }
       

        if (EnvironmentController.Daytime == false && EnvironmentController.dayTimeTimer <=0)
        {
            NightTime(NightRoutineRun);
        }
        
    }
    
    void DayTimeStart(bool DayTimeStartRuned)
    {
        if (DayTimeStartRuned == false)
        {
        dayExisted++;
        transform.up = new Vector3(Random.Range(-3f,3f), Random.Range(-2f, 2f), 0) - transform.position;
        upFromLastFrame = transform.up;
        DayTimeStartRuned = true;

        }
        
    }
    void NightTime(bool NightTimeRuned)
    {

        if (NightCycleTime < dayExisted)
        {
        if (foodFound == 0 || insideSafeZone == false)
        {
                // die
                if (justSpawned != true)
                {
                Destroy(gameObject);
                Debug.Log("die");
                }
            
        }
        
            NightTimeRuned = true;
            
            NightCycleTime++;
        }
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("food") && targetFood == null && foodFound < 2)
        {
            //transform.up = col.gameObject.transform.position - transform.position;
            targetFood = col.gameObject;
            Debug.Log("lock");
        }
       

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("food"))
        {
            foodFound++;
            Destroy(collision.gameObject) ;
            Debug.Log("eat");
            //targetLocked = false;
           
        }

        if (collision.gameObject.CompareTag("wall") && BackHome == false)
        {
            transform.Rotate(0, 0, Random.Range(160f, 200f));
            upFromLastFrame = transform.up;
            Debug.Log("collide Wall");
        }
    }
}
