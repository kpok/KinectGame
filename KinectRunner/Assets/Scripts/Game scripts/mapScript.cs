using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class mapScript : MonoBehaviour
{

    public GameObject map;

    public float startPosX;
    public float startPosY;
    public float startPosZ;

    private List<GameObject> CurrentRoads = new List<GameObject>();
    private int CurrentRoadId = 0;

    private float startPnt;
    private float endPnt;
    private float time = 0;
    private float speed = 0.5f;
    private Transform CenterPoints;
    void Start()
    {
        startPnt = startPosZ;
        endPnt = startPosZ - 1;
        createNewRoad();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 startPoint = new Vector3(startPosX, startPosY, startPnt);
        Vector3 endPoint = new Vector3(startPosX, startPosY, endPnt);
        for (int i = 0; i < CurrentRoads.Count; i++)
        {
            CurrentRoads[i].transform.position = Vector3.Lerp(startPoint, endPoint, time);
        }       
        startPnt -= speed;
        endPnt -= speed;
        time += Time.deltaTime * speed;

        if (time > 0)
        {
            if (CenterPoints.position.z == 0)
            {
                CurrentRoadId++;
                createNewRoad();
            }
        }
    }

    void createNewRoad()
    {
        if (CurrentRoadId > 0)
        {
            if (CurrentRoadId > 1)
            {
                DestroyImmediate(CurrentRoads[CurrentRoadId - 2]);
                CurrentRoads.RemoveAt(0);
                CurrentRoadId--;
            }
            startPnt = startPosZ;
            endPnt = startPosZ - 1;
            CurrentRoads.Add(Instantiate(map, CenterPoints.position, Quaternion.identity) as GameObject);
        }
        else
            CurrentRoads.Add(Instantiate(map, new Vector3(startPosX, startPosY, 1), Quaternion.identity) as GameObject);
        //znalezienie punktu triger
        CenterPoints = CurrentRoads[CurrentRoadId].transform.FindChild("ClonePoint");
    }
}
