using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceObject : MonoBehaviour
{
    public Bounds activeBounds;
    public Vector3 direction;
    public float speedMultiplier;
    public float speed;

    void Start()
    {
        
    }

    public void SetColor(Color color)
    {
        var renderer = GetComponent<SpriteRenderer>();
        if(renderer != null)
        {
            renderer.color = color;
        }
    }
    
    void Update()
    {
        transform.Translate(direction * Time.deltaTime * speedMultiplier * speed);

        if(!activeBounds.Contains(transform.position))
        {
            gameObject.SetActive(false);
        }
    }
}
