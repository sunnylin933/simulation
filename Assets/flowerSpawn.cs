using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flowerSpawn : MonoBehaviour
{
    public static flowerSpawn instance;
    public GameObject spawnObject;
    public float spawnCounter;
    private float randomX;
    private float randomY;

    private void Start()
    {
        instance = this;
    }
    void Update()
    {
        int numberofFlowers = GameObject.FindGameObjectsWithTag("Flower").Length;
        if(numberofFlowers  < 15)
        {
            spawnCounter += Time.deltaTime;
            if(spawnCounter > 1.5f)
            {
                randomX = Random.Range(-12, 12);
                randomY = Random.Range(-6, 6);
                GameObject newFlower = Instantiate(spawnObject, new Vector3(randomX, randomY, 0), Quaternion.identity);
                newFlower.transform.parent = transform;
                spawnCounter = 0;
            }
        }
    }
}
