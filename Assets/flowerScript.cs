using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class flowerScript : MonoBehaviour
{
    public Sprite[] sprites;
    private float xScale;
    private float lifeTimer;
    private float lifeSpan;
    public bool isOccupied;

    private void Awake()
    {
        isOccupied = false;
        xScale = 0.4f;
        int randomVal = Random.Range(0, sprites.Length);
        gameObject.GetComponent<SpriteRenderer>().sprite = sprites[randomVal];
    }
    void Start()
    {
        lifeSpan = Random.Range(8, 20);
    }

    // Update is called once per frame
    void Update()
    {
        var bees = GameObject.FindGameObjectsWithTag("Bee");
        for(int i = 0; i < bees.Length; i++)
        {
            if (GetComponent<BoxCollider2D>().IsTouching(bees[i].GetComponent<BoxCollider2D>()))
            {
                isOccupied = true;
                print("Occupied");
            }
        }

        if(!isOccupied)
        {
            lifeTimer += Time.deltaTime;
            if (lifeTimer > lifeSpan)
            {
                GetComponent<SpriteRenderer>().color = Color.gray;
                GetComponent<BoxCollider2D>().enabled = false;
                xScale = Mathf.Lerp(xScale, 0, 0.025f);
            }
            else
            {
                xScale = Mathf.Lerp(xScale, 1, 0.05f);
            }

            transform.localScale = new Vector3(xScale, xScale, transform.localScale.z);

            if (transform.localScale.x < 0.025f)
            {
                Destroy(gameObject);
            }
        }
    }
}
