using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class mantisScript : MonoBehaviour
{
    
    public float mSpeed;
    private Rigidbody2D rb;

    public bool isWalking;
    private bool isEating;
    public float walkTime;
    public float walkCounter;
    public float waitTime;
    private float waitCounter;
    private float xScale;
    private float lerpVal;

    private int walkDirection;
    private float timeAlive;
    public int beesEaten;
    private float mateCooldown;
    public bool canMate;
    void Start()
    {
        mateCooldown = 0;
        canMate = true;
        beesEaten = 0;
        isEating = false;
        rb = GetComponent<Rigidbody2D>();

        var randomWait = Random.Range(0.5f, 2f);
        waitTime = randomWait;
        var randomWalk = Random.Range(1, 4);
        walkTime = randomWalk;

        waitCounter = waitTime;
        walkCounter = walkTime;
        xScale = 0.5f;
        lerpVal = 0.5f;


        ChooseDirection();
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

        if(!canMate)
        {
            mateCooldown += Time.deltaTime;
        }
        if (mateCooldown > 5)
        {
            canMate = true;
            mateCooldown = 0;
        }


        if (!isEating)
        {
            timeAlive += Time.deltaTime;
            if(timeAlive > 25)
            {
                GetComponent<BoxCollider2D>().enabled = false;
                isWalking = false;
                lerpVal = 0;
                if(xScale < 0.2)
                {
                    Destroy(gameObject);
                }
            }
            
            if (isWalking)
            {
                if (!GetComponent<AudioSource>().isPlaying)
                {
                    GetComponent<AudioSource>().Play();
                }

                walkCounter -= Time.deltaTime;
                switch (walkDirection)
                {
                    case 0:
                        rb.velocity = new Vector2(0, mSpeed);
                        break;
                    case 1:
                        rb.velocity = new Vector2(0, -mSpeed);
                        break;
                    case 2:
                        rb.velocity = new Vector2(mSpeed, 0);
                        gameObject.GetComponent<SpriteRenderer>().flipX = true;
                        break;
                    case 3:
                        rb.velocity = new Vector2(-mSpeed, 0);
                        gameObject.GetComponent<SpriteRenderer>().flipX = false;
                        break;
                }
                if (walkCounter < 0)
                {
                    isWalking = false;
                    waitCounter = waitTime;
                }
            }
            else
            {
                waitCounter -= Time.deltaTime;
                rb.velocity = Vector2.zero;
                GetComponent<AudioSource>().Stop();
                if (waitCounter < 0)
                {
                    ChooseDirection();
                }
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        if (lerpVal > 1f)
        {
            lerpVal = 1;
        }
        xScale = Mathf.Lerp(xScale, lerpVal, 0.5f * Time.deltaTime);
        transform.localScale = new Vector3(xScale, xScale, transform.localScale.z);
    }

    public void ChooseDirection()
    {
        walkDirection = Random.Range(0, 4);
        isWalking = true;
        walkCounter = walkTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.CompareTag("Bee") && beesEaten<5)
        {
            isEating = true;
            timeAlive -= 5;
            beesEaten += 1;
            StartCoroutine(FinishEating());
            collision.GetComponent<beeScript>().timeAlive = 25;
            collision.GetComponent<beeScript>().eaten = true;
            lerpVal += 0.1f;
        }

       if(collision.CompareTag("Mantis"))
        {
            if(collision.GetComponent<mantisScript>().beesEaten + beesEaten > 0 && collision.GetComponent<mantisScript>().canMate && canMate)
            {
                mantisSpawner.instance.SpawnMantis(transform.position.x, transform.position.y);
                collision.GetComponent<mantisScript>().canMate = false;
                canMate = false;
            }
        }
    }

    IEnumerator FinishEating()
    {
        yield return new WaitForSeconds(2);
        isEating = false;
    }
}
