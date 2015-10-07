using UnityEngine;
using System.Collections;

public class mapScript : MonoBehaviour {

    public GameObject map;

    private float startPnt = 50f;
    private float endPnt = 1f;
    private float time = 0;
    private float speed = 0.5f;
    private float posY = 0.3f;
    // Update is called once per frame
    void Update()
    {       
            Vector3 startPoint = new Vector3(0f, posY, startPnt);
            Vector3 endPoint = new Vector3(0f,posY,endPnt);
            transform.position = Vector3.Lerp(startPoint, endPoint, time);
            startPnt-=speed;
            endPnt-=speed;
            time += Time.deltaTime * speed;
        
    }
}
