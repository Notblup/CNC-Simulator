using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailRender : MonoBehaviour
{
    // Start is called before the first frame update

    public TrailRenderer trail;
    public float trailWidth;
    void Start()
    {
        trail.widthMultiplier = trailWidth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
