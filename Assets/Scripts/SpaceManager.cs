using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceManager : MonoBehaviour
{
    public SpaceObject[] spaceObjects;
    public BoxCollider2D activeBounds;

    List<SpaceObject> spaceObjectPool;
    private int spaceObjectPoolSize = 20;

    private BoxCollider2D spawnArea;
    private float minPosition, maxPosition, yPosition;
    public float spawnRate = 0.01f;
    private float lastSpawn = 0f;
    private float lastSpeed; 

    // Start is called before the first frame update
    void Start()
    {
        spawnArea = GetComponent<BoxCollider2D>();
        minPosition = spawnArea.bounds.min.x;
        maxPosition = spawnArea.bounds.max.x;
        yPosition = spawnArea.bounds.center.y;

        spaceObjectPool = new List<SpaceObject>(); 
        
        for (int i = 0; i < spaceObjectPoolSize; i++)
        {
            SpaceObject spaceObject = Instantiate(spaceObjects[0], this.transform);
            spaceObject.gameObject.SetActive(false);
            spaceObjectPool.Add(spaceObject);
        }

        lastSpeed = GameManager.Instance.Probe.VisibleSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(lastSpeed != GameManager.Instance.Probe.VisibleSpeed)
        {
            lastSpeed = GameManager.Instance.Probe.VisibleSpeed;

            spaceObjectPool.ForEach(spaceObject =>
            {
                if (spaceObject.gameObject.activeInHierarchy)
                {
                    spaceObject.speed = lastSpeed;
                }
            });
        }

        lastSpawn += Time.deltaTime;
        float spawnTime = 0 + spawnRate; // spawnRate * lastSpeed;

        var spaceObject = spaceObjectPool.Find(spaceObject => !spaceObject.gameObject.activeInHierarchy);

        if(spaceObject != null)
        {
            if(lastSpawn > spawnTime)
            {
                float xPosition = Random.Range(minPosition, maxPosition);
                spaceObject.transform.position = new Vector3(xPosition, yPosition);

                spaceObject.activeBounds = activeBounds.bounds;
                spaceObject.direction = Vector3.down;
                spaceObject.speedMultiplier = Random.Range(0.5f, 1f);
                spaceObject.speed = lastSpeed;

                int brightness = Random.Range(1, 10);
                spaceObject.SetColor(ColorPallette.Grayscale[brightness]);

                int scale = Random.Range(0, 10);
                if (scale >= 9)
                {
                    spaceObject.transform.localScale = new Vector3(2, 2, 1);
                }
                else
                {
                    spaceObject.transform.localScale = new Vector3(1, 1, 1);
                }

                spaceObject.gameObject.SetActive(true);

                lastSpawn = 0f;
            }
        }
    }
}
