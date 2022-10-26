using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mantisSpawner : MonoBehaviour
{
    public static mantisSpawner instance;
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
        int numberofMantis = GameObject.FindGameObjectsWithTag("Mantis").Length;

        if (numberofMantis < 3)
        {
            spawnCounter += Time.deltaTime;
            if (spawnCounter > 7f)
            {
                randomX = Random.Range(-11, 11);
                randomY = Random.Range(-6, 6);
                GameObject newMantis = Instantiate(spawnObject, new Vector3(randomX, randomY, 0), Quaternion.identity);
                newMantis.transform.parent = transform;
                spawnCounter = 0;
            }
        }
    }

    public void SpawnMantis(float xSpawn, float ySpawn)
    {
        GameObject newMantis = Instantiate(spawnObject, new Vector3(xSpawn, ySpawn, 0), Quaternion.identity);
        newMantis.transform.parent = transform;
    }
}
