using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beeSpawner : MonoBehaviour
{
    public static beeSpawner instance;
    public GameObject spawnObject;
    public float spawnCounter;
    private float randomX;
    private float randomY;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        int numberofBees = GameObject.FindGameObjectsWithTag("Bee").Length;

        if(numberofBees < 3)
        {
            spawnCounter += Time.deltaTime;
            if (spawnCounter > 5f)
            {
                randomX = Random.Range(-12, 12);
                randomY = Random.Range(-6, 6);
                GameObject newBee = Instantiate(spawnObject, new Vector3(randomX, randomY, 0), Quaternion.identity);
                newBee.transform.parent = transform;
                spawnCounter = 0;
            }
        }
    }

    public void SpawnBee(float xSpawn, float ySpawn)
    {
        GameObject newBee = Instantiate(spawnObject, new Vector3(xSpawn, ySpawn, 0), Quaternion.identity);
        newBee.transform.parent = transform;
    }
}
