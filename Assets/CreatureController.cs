using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour
{
    // Start is called before the first frame update

    public static float speed = 2;
    public float energy = 1;
    public float size;
    public Collider2D senseCircle;
    private int foodFound = 0;
    private bool insideSafeZone;
    public Rigidbody2D rigidbodyOfCreature;
    public Collider2D creatureBody;
    //private bool targetLocked = false;
    private float dayTimeLeft = 20;
    public GameObject creature;
    private int dayExisted = 0;
    private int NightCycleTime = 0;
    private Vector3 home;
    private GameObject targetFood;
    private Vector3 upFromLastFrame;
    private float timerForDirectionChange = 3f;
    private bool BackHome = false;
    private bool NightRoutineRun;
    private bool DayRoutineRun;
    void Start()
    {
        rigidbodyOfCreature = gameObject.GetComponent<Rigidbody2D>();
        home = transform.position;
        DayTimeStart();
    }

    // Update is called once per frame
    void Update()
    {

        if (EnvironmentController.Daytime == true)
        {
        rigidbodyOfCreature.velocity = transform.up * speed;
        if (targetFood == null)
        {
            transform.up = upFromLastFrame;
            if (timerForDirectionChange <= 0)
            {
                timerForDirectionChange = 1.5f;
                transform.Rotate(0,0,Random.Range(-50f,50f));
                upFromLastFrame = transform.up;
            }

            timerForDirectionChange -= Time.deltaTime;
        }

          if (foodFound == 2 || ((foodFound == 1) && EnvironmentController.dayTimeTimer < 3))
        {
            //go back to home
            BackHome = true;
                transform.up = home - transform.position;
            }
        }
       

        if (EnvironmentController.Daytime == false)
        {
            NightTime();
        }
        
    }
    
    void DayTimeStart()
    {
        dayExisted++;
        transform.up = new Vector3(Random.Range(-3f,3f), Random.Range(-2f, 2f), 0) - transform.position;
        upFromLastFrame = transform.up;
    }
    void NightTime()
    {

        if (NightCycleTime < dayExisted)
        {
        if (foodFound == 0 || transform.position != home)
        {
            // die
            Destroy(gameObject);
        }
        if (foodFound >= 2)
        {
            //reproduce
            GameObject newCreature = Instantiate(creature, transform.position, Quaternion.identity);
            //newCreature.GetComponents<CreatureController>(). = ;
        }
            NightCycleTime++;
            foodFound = 0;
        }
       
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("food") && targetFood == null && foodFound < 2)
        {
            transform.up = col.gameObject.transform.position - transform.position;
            targetFood = col.gameObject;
            Debug.Log("lock");
        }
       

    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("food"))
        {
            foodFound++;
            Destroy(collision.gameObject) ;
            Debug.Log(foodFound);
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
