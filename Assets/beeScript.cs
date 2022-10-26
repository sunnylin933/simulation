using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class beeScript : MonoBehaviour
{
    public AudioClip[] beeAudio;

    public bool eaten;
    public float mSpeed;
    public float rSpeed;
    private float walkCounter;
    private int randomNum;
    private bool canSpawn;

    private float xScale;
    private float lerpVal;
    public int flowersVisited;
    public float timeAlive;

    [HideInInspector]
    public bool onFlower;
    [HideInInspector]
    public float timeOnFlower;


    // Start is called before the first frame update
    void Start()
    {
        canSpawn = false;
        onFlower = false;
        eaten = false;
        timeOnFlower = 0;
        xScale = 0.25f;
        lerpVal = 0.25f;

        var randomNum = Random.Range(0, beeAudio.Length);
        GetComponent<AudioSource>().clip = beeAudio[randomNum];
        GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -12)
        {
            transform.position = new Vector3(transform.position.x * -1 - 1, transform.position.y, 0);
        }
        else if (transform.position.x > 12)
        {
            transform.position = new Vector3(transform.position.x * -1 + 1, transform.position.y, 0);
        }

        if (transform.position.y < -7)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y * -1 - 1, 0);
        }
        else if (transform.position.y > 7)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y * -1 + 1, 0);
        }

        timeAlive += Time.deltaTime;
        if (!onFlower && !eaten)
        {
            timeOnFlower = 0;
            walkCounter += Time.deltaTime;
            if (walkCounter > 1)
            {
                randomNum = Random.Range(0, 2);
                walkCounter = 0;
            }
            switch (randomNum)
            {
                case 0:
                    transform.eulerAngles += Vector3.forward * rSpeed;
                    break;
                case 1:
                    transform.eulerAngles += Vector3.back * rSpeed;
                    break;
            }
            transform.position += transform.up * mSpeed;
        }
        else if(onFlower)
        {
            timeOnFlower += Time.deltaTime;
            if(timeOnFlower > 2 && canSpawn)
            {
                if(flowersVisited > 2 && GameObject.FindGameObjectsWithTag("Bee").Length < 7)
                {
                    beeSpawner.instance.SpawnBee(transform.position.x, transform.position.y);
                }
                onFlower = false;
            }
        }
        if (timeAlive > 25)
        {
            eaten = true;
            lerpVal = 0;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            if(xScale < 0.25)
            {
                Destroy(gameObject);
            }
        }

        if(lerpVal > 1f)
        {
            lerpVal = 1;
        }
        xScale = Mathf.Lerp(xScale, lerpVal, 0.5f * Time.deltaTime);
        transform.localScale = new Vector3(xScale, xScale, transform.localScale.z);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Flower") && canSpawn)
        {
            lerpVal += 0.25f;
            flowersVisited += 1;
            onFlower = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Flower"))
        {
            timeAlive -= 4;
            collision.GetComponent<flowerScript>().isOccupied = false;
            canSpawn = true;
        }

    }
}
