using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class Controller : MonoBehaviour
{
    private int playerScore;
    public Text scoreText;
    public Text gameOverText;

    private bool dead;

    public GameObject Road;
    public GameObject Coin;
    public List<Material> materials = new List<Material>();

    Animator anim;

    private List<Transform> Cpoints = new List<Transform>();
    private List<GameObject> CurrentRoads = new List<GameObject>();
    private List<GameObject> CurrentCoins = new List<GameObject>();
    private int CurrentPoint = 1;
    private int positionNum = 0;
    private int positionYNum = 0;
    private int CurrentRoadId = 0;
    private int LevelCount = 0;
    private float time = 0;
    private float distance = 0, nextDistance = 0;
    private float distanceY = -0.5f, nextYDistance =1.8f;
    private float cubeY = 0.8f;
    private float speed;
    private Transform CenterPoints;
    private bool jump = false;
    private bool crouch = false;

    void Start()
    {
        //edit by browser
        anim = GetComponent<Animator>();
        speed = 1.5f;
        playerScore = 0;
        dead = false;
        gameOverText.text = "";
        setScoreText();
        CreateNewRoad();
    }
    void CreateNewRoad()
    {
        for (int i = CurrentCoins.Count - 1; i >= 0; i--)
        {
            if (CurrentCoins[i].transform.position.z < this.transform.position.z)
            {                
                    DestroyImmediate(CurrentCoins[i]);
                    CurrentCoins.RemoveAt(i);                
            }
        }
        if (CurrentRoadId > 0)
        {
            if (CurrentRoadId > 1)
            {
                DestroyImmediate(CurrentRoads[CurrentRoadId - 2]);
                CurrentRoads.RemoveAt(0);
                CurrentRoadId--;
            }

            CurrentRoads.Add(Instantiate(Road, Cpoints[LevelCount * 19 + LevelCount - 1].position,
                                            Quaternion.identity) as GameObject);
        }
        else
            CurrentRoads.Add(Instantiate(Road, Vector3.zero, Quaternion.identity) as GameObject);

        CenterPoints = CurrentRoads[CurrentRoadId].transform.FindChild("Plane").FindChild("Points");
        for (int i = 1; i < 21; i++)
        {
            Transform childPoint = CenterPoints.transform.FindChild("P" + i);
            Cpoints.Add(childPoint);

            int typeNum = Random.Range(0, materials.Count);
            Vector3 cPOS = Vector3.zero;

            if (LevelCount > 0)
                cPOS = new Vector3(childPoint.transform.position.x + Random.Range(-1, 2) * 1.5f,
                                   childPoint.transform.position.y + cubeY,
                                   childPoint.transform.position.z);
            else
                cPOS = new Vector3(Cpoints[i - 1].position.x + Random.Range(-1, 2) * 1.5f, Cpoints[i - 1].position.y + cubeY, Cpoints[i - 1].position.z);

            GameObject newCoin = (Instantiate(Coin, cPOS, Quaternion.identity) as GameObject);
            CurrentCoins.Add(newCoin);
            newCoin.transform.Rotate(90, 0, 90);
            newCoin.transform.GetComponent<Renderer>().material = materials[typeNum];

            if (typeNum == 0)
                newCoin.name = "Yellow";

            else if (typeNum == 1)
                newCoin.name = "Red";

            else if (typeNum == 2)
                newCoin.name = "Blue";

            else if (typeNum == 3)
                newCoin.name = "Black";
        }
    }

    void Update()
    {
        Vector3 startPoint = new Vector3(Cpoints[CurrentPoint].position.x + distance,
                                          Cpoints[CurrentPoint].position.y  + distanceY,
                                          Cpoints[CurrentPoint].position.z);
        Vector3 endPoint = new Vector3(Cpoints[CurrentPoint + 1].position.x + distance,
                                        Cpoints[CurrentPoint + 1].position.y + distanceY,
                                        Cpoints[CurrentPoint + 1].position.z);
        transform.position = Vector3.Lerp(startPoint, endPoint, time);
        transform.LookAt(Vector3.Lerp(Cpoints[CurrentPoint + 1].transform.position,
                                             Cpoints[CurrentPoint+2].transform.position, time));
        time += Time.deltaTime * speed;

        anim.SetFloat("Speed", 1.0f);
        anim.SetBool("Jump", jump);
        anim.SetBool("Crouch", crouch);

        if (time > 1)
        {
            time = 0;
            CurrentPoint++;

            if (Cpoints[CurrentPoint].name == "P15")
            {
                CurrentRoadId++;
                LevelCount++;
                CreateNewRoad();
            }

        }

        this.keyControl();
    }

    public void keyControl()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (positionNum != -1)
            {
                positionNum--;
                nextDistance = 1.0f * positionNum;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (positionNum != 1)
            {
                positionNum++;
                nextDistance = 1.0f * positionNum;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (positionYNum == 0)
            {
                jump = true;                
                print("gora");
                positionYNum++;
                distanceY += nextYDistance;
                EasyTimer.SetTimeout(() =>
                {
                    positionYNum--;
                    distanceY -= nextYDistance;
                    jump = false;
                    
                }, 700);
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (positionYNum == 0)
            {
                print("dol");
                crouch = true;
                positionYNum--;
                //distanceY -= nextYDistance;
                EasyTimer.SetTimeout(() =>
                {
                    positionYNum++;
                    //distanceY += nextYDistance;
                    crouch = false;
                }, 700);
            }
        }
        if (dead)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                this.restartAction();
                this.Start();
            }
        }
        distance = Mathf.Lerp(distance, nextDistance, Time.deltaTime * 3 * speed)+nextDistance*0.03f;
        //distance = nextDistance;
    }

    public void BadCollision(GameObject hGameObject)
    {
        print("bad object");

        if (!dead)
        {
            if(hGameObject.name.Equals("Black"))
            {
                playerScore -= 150;
                setScoreText();
            }
            else if (hGameObject.name.Equals("Red"))
            {
                dead = true;
                this.speed = 0;
                gameOverText.text = "Game Over \nTap space to restart";
            }

        }

        if (this.CurrentCoins.Contains(hGameObject))
        {
            CurrentCoins.Remove(hGameObject);
            Destroy(hGameObject);
        }
    }

    public void BonusCollision(GameObject hGameObject)
    {
        print("good object");

        if (hGameObject.name.Equals("Yellow")) playerScore += 50;
        else playerScore += 100;
        setScoreText();

        if (this.CurrentCoins.Contains(hGameObject))
        {
            CurrentCoins.Remove(hGameObject);
            Destroy(hGameObject);
        }
    }

    void setScoreText()
    {
        this.scoreText.text = "Score: " + playerScore.ToString();
    }

    void restartAction()
    {
        Cpoints.Clear();
        CurrentPoint = 1;
        positionNum = 0;
        positionYNum = 0;
        CurrentRoadId = 0;
        LevelCount = 0;
        time = 0;
        distance = 0;
        nextDistance = 0;
        distanceY = -0.5f;
        nextYDistance = 0.5f;
        for (int i = CurrentCoins.Count - 1; i >= 0; i--)
        {
           DestroyImmediate(CurrentCoins[i]);
           CurrentCoins.RemoveAt(i);
        }
        for (int i = CurrentRoads.Count - 1; i >= 0; i--)
        {
            DestroyImmediate(CurrentRoads[i]);
            CurrentRoads.RemoveAt(i);
        }
    }

}
