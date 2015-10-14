using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class PlayerControl : MonoBehaviour
{

	CharacterController controller;

    public bool gameOver;
    
    public int live;
    public int score;

	bool isGrounded= false;
	public float speed = 6.0f;
	public float jumpSpeed = 8.0f;
	public float gravity = 20.0f;
	private Vector3 moveDirection = Vector3.zero;
	KinectManager manager = KinectManager.Instance;

	public Text text;
	public Text infotext;
    //game text zone
    public Text scoreText;
    public Text liveText;
    public bool pause = false;



	// Use this for initialization
	void Start () {
        setText();
        live = 3;
		controller = GetComponent<CharacterController>();
		isGrounded = true;
	}

	private string VectorToString(Vector3 vector)
	{
		return vector.x.ToString("#.#") + " " + vector.y.ToString("#.#") + " " + vector.z.ToString("#.#");
	}

	void UpdateKinectPostion()
	{
		KinectManager manager = KinectManager.Instance;

		if (manager == null) 
		{
			infotext.text = "Failed to find Kinect";
			return;
		}

		uint playerID = manager != null ? manager.GetPlayer1ID () : 0;

		Vector3 head = manager.GetJointPosition(playerID, 
		                                        (int)KinectWrapper.NuiSkeletonPositionIndex.Head);
		Vector3 shoulderCenter = manager.GetJointPosition(playerID, 
		                                                  (int)KinectWrapper.NuiSkeletonPositionIndex.ShoulderCenter);
		Vector3 footRight = manager.GetJointPosition(playerID, 
		                                             (int)KinectWrapper.NuiSkeletonPositionIndex.FootRight);
		Vector3 footLeft = manager.GetJointPosition(playerID, 
		                                       (int)KinectWrapper.NuiSkeletonPositionIndex.FootLeft);

		if (head.z < 0.1f) {
			infotext.text = "Failed to find Player";
			return;
		}

		infotext.text = "Hy: " + head.y +
			" Sx: " + shoulderCenter.x + 
				" RFy: " + footRight.y + 
				" LFy: " + footLeft.y;
		
		double z = shoulderCenter.z;
		double x = shoulderCenter.x;
		double y = Math.Min(footLeft.y, footRight.y);

		if (y > 0.25) 
		{
			Jump ();
		}
		if (x < -0.20)
		{
			GoLeft();
			return;
		}
		if (x > 0.20)
		{
			GoRight();
			return;
		}
		GoCenter ();
		return;

	}

	void UpdateKeyboardPosition()
	{
		if (Input.GetKey(KeyCode.UpArrow))
		{
			Jump();
			return;
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			GoLeft();
			return;
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			GoRight();
			return;
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			GoCenter();
			return;
		}

        //TODO
        if (Input.GetKey(KeyCode.P))
        {
            if (!pause)
            {
                Time.timeScale = 0;
                pause = true;
            }
            else 
            {
                Time.timeScale = 1;
                pause = false;
            }
        }

	}

	// Update is called once per frame
	void Update () {
        setText();
        
        //	if (controller.isGrounded) {
			GetComponent<Animation>().Play("run");            //play "run" animation if spacebar is not pressed
			           //increase the speed of the movement by the factor "speed" 

			//UpdateKinectPostion();
			UpdateKeyboardPosition();
		InvokeChangePosition ();
		//	if(controller.isGrounded)           //set the flag isGrounded to true if character is grounded
		//		isGrounded = true;
		//}
		

	}

	void InvokeChangePosition()
	{
		moveDirection.y -= gravity * Time.deltaTime;       //Apply gravity 
		//Debug.Log (moveDirection.ToString ());
		controller.Move(moveDirection * Time.deltaTime);      //Move the controller
	
	}

	void GoLeft()
	{
		transform.position = new Vector3 (-1.0f, 1.2f, -7.0f);
		//moveDirection = new Vector3(-0.2f, 0.0f, 0.0f);  //get keyboard input to move in the horizontal direction
		//moveDirection = transform.TransformDirection(moveDirection);  //apply this direction to the character
		//moveDirection *= speed; 
		
		text.text = "left";
	}

	void GoCenter()
	{
		transform.position = new Vector3(0.0f, 1.2f, -7.0f);
		//moveDirection = new Vector3(0, 0, 0);  //get keyboard input to move in the horizontal direction
		//moveDirection = transform.TransformDirection(moveDirection);  //apply this direction to the character
		//moveDirection *= speed; 
		
		text.text = "center";

	}

	void GoRight()
	{
		transform.position = new Vector3(1.0f, 1.2f, -7.0f);
		//moveDirection = new Vector3(1, 0, 0);  //get keyboard input to move in the horizontal direction
		//moveDirection = transform.TransformDirection(moveDirection);  //apply this direction to the character
		//moveDirection *= speed; 
		
		text.text = "right";

	}

	public void Jump()
	{
		GetComponent<Animation>().Stop("run");
		GetComponent<Animation>().Play("jump_pose");
		moveDirection.y = jumpSpeed; 

	}

    void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.name == "Powerup(Clone)")
		{
            score += 50;
            Destroy(other.gameObject); 
		}
		else if(other.gameObject.name == "Obstacle(Clone)")
		{
            score += 100;
            Destroy(other.gameObject); 
		}
        else if (other.gameObject.name == "Pipe(Clone)")
        {
            live--;
            Debug.Log("Live: " + live);
            if (live <= 0)
            {
                stopGame();
            }                
            Destroy(other.gameObject);
        }
    }

    //set score i live text
    void setText()
    {
        scoreText.text = "Score: " + score.ToString();
        liveText.text = "Live: " + live.ToString();
    }

    //when stop game 
    void stopGame()
    {
        //TODO
        Destroy(this);
        speed = 0;
        gameOver = true;
    }
}
