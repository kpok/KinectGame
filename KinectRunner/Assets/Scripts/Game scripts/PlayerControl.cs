using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Threading;
public class PlayerControl : MonoBehaviour
{
	public bool gameOver;
	public int live;
	public int score;
	//game text zone
	public Text scoreText;
	public Text liveText;
	public bool pause = false;
	public bool isSquat = false;
	private bool isDynamicMode = true;
	
	
	public int frameCounter = 0;
	CharacterController controller;
	public float speed = 6.0f;
	public float jumpSpeed = 8.0f;
	public float gravity = 20.0f;
	private Vector3 moveDirection = Vector3.zero;
	KinectManager manager = KinectManager.Instance;
	public bool playerFound = false;
	public bool calibration = false;
	private float calibrationSumHeadY = 0.0f;
	private float calibrationSumSpineY = 0.0f;
	private int calibrationCountSpineY = 0;
	private int calibrationCountHeadY = 0;
	
	private float spineStablePositionY = 1.2f;
	private float headStablePositionY = 1.74f;
	
	public Text text;
	public Text infotextHead;
	public Text infotextSpine;
	public Text infotextFootL;
	public Text infotextFootR;
	
	private float minDifference = 0.0001f;
	private int outOfFrameCount = 0;
	private int maxOutOfFrameCount = 200;
	private Vector3 lastHead= new Vector3(-1000.0f, -1000.0f, -1000.0f);
	private Vector3 lastSpine = new Vector3(-1000.0f, -1000.0f, -1000.0f);
	private Vector3 lastFootR = new Vector3(-1000.0f, -1000.0f, -1000.0f);
	private Vector3 lastFootL = new Vector3(-1000.0f, -1000.0f, -1000.0f);
	
	private object _lock = new object();
	
	
	
	// Use this for initialization
	void Start () 
	{
		setText();
		live = 3;
		controller = GetComponent<CharacterController> ();
	}
	
	
	
	
	
	private string VectorToString(Vector3 vector)
	{
		return vector.x.ToString("#.#") + " " + vector.y.ToString("#.#") + " " + vector.z.ToString("#.#");
	}
	
	private Field GetPositionXFromKinect(Vector3 position)
	{
		//similarity of triangles
		float baseX = Utils.baseZ * position.x / position.z;
		
		foreach (Field field in Utils.FIELDS) 
		{
			if (field.start <= baseX && field.end >= baseX)
				return field;
		}
		return null;
	}
	
	private float GetPositionYFromKinect(Vector3 position)
	{
		//similarity of triangles
		float baseY = Utils.baseZ * position.y / position.z;
		
		
		return baseY;
	}
	
	void StartCalibration()
	{
		calibration = true;
		
		new Thread (() =>
		            {
			for (int i = 0; i < 50; i++)
			{
				if (!calibration)
					return;
				Thread.Sleep(100);
			}
			lock(_lock)
			{
				if (calibration)
				{
					spineStablePositionY = calibrationSumSpineY / calibrationCountSpineY;
					headStablePositionY = calibrationSumHeadY / calibrationCountHeadY;
				}
				
			}
			
			
		}).Start();
	}
	
	void UpdateKinectPostion()
	{
		infotextHead.text = "";
		infotextFootL.text = "";
		infotextFootR.text = "";
		infotextSpine.text = "";
		KinectManager manager = KinectManager.Instance;
		if (manager == null) 
		{
			infotextHead.text = "Failed to find Kinect";
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
		
		Vector3 spine = manager.GetJointPosition(playerID, 
		                                         (int)KinectWrapper.NuiSkeletonPositionIndex.Spine);
		
		lastHead = head;
		
		
		lock (_lock) 
		{
			if (head.x - lastHead.x < minDifference ||
			    head.y - lastHead.y < minDifference ||
			    head.z - lastHead.z < minDifference ||
			    spine.x - lastSpine.x < minDifference ||
			    spine.y - lastSpine.y < minDifference ||
			    spine.z - lastSpine.z < minDifference ||
			    footLeft.x - lastFootL.x < minDifference ||
			    footLeft.y - lastFootL.y < minDifference ||
			    footLeft.z - lastFootL.z < minDifference ||
			    footRight.x - lastFootR.x < minDifference ||
			    footRight.y - lastFootR.y < minDifference ||
			    footRight.z - lastFootR.z < minDifference)
			{
				outOfFrameCount++;
			}
			
			if (outOfFrameCount > maxOutOfFrameCount || 
			    spine.z < 0.1f || head.z < 0.1f || footLeft.z < 0.1f || footRight.z < 0.1f) 
			{
				infotextHead.text = "Failed to find Player";
				playerFound = false;
				calibration = false;
				calibrationCountSpineY = 0;
				calibrationCountHeadY = 0;
				calibrationSumSpineY = 0.0f;
				calibrationSumHeadY = 0.0f;
				outOfFrameCount = 0;
				
				lastHead= new Vector3(-1000.0f, -1000.0f, -1000.0f);
				lastSpine = new Vector3(-1000.0f, -1000.0f, -1000.0f);
				lastFootR = new Vector3(-1000.0f, -1000.0f, -1000.0f);
				lastFootL = new Vector3(-1000.0f, -1000.0f, -1000.0f);
				return;
			} 
			outOfFrameCount = 0;
			
			if (playerFound == false)
			{
				playerFound = true;
				//StartCalibration();
			}
			else if (calibration)
			{
				calibrationCountSpineY++;
				calibrationCountHeadY++;
				calibrationSumSpineY += spine.y;
				calibrationSumHeadY += head.y;
				
				infotextHead.text = "Calibration";
			}
			
			
			
		}
		
		if (playerFound == false || calibration)
			return;
		
		infotextHead.text = "H: " + head.x.ToString ("#.##") + " " +
			head.y.ToString ("#.##") + " " +
				head.z.ToString ("#.##") + " " +
				headStablePositionY.ToString ("#.##");
		
		infotextSpine.text = "S: " + spine.x.ToString ("#.##") + " " +
			spine.y.ToString ("#.##") + " " +
				spine.z.ToString ("#.##") + " " +
				spineStablePositionY.ToString ("#.##");
		
		infotextFootR.text = "FR: " + footRight.x.ToString ("#.##") + " " +
			footRight.y.ToString ("#.##") + " " +
				footRight.z.ToString ("#.##");
		
		infotextFootL.text = "FL: " + footLeft.x.ToString ("#.##") + " " +
			footLeft.y.ToString ("#.##") + " " +
				footLeft.z.ToString ("#.##");
		
		
		if (spine.y > 
		    spineStablePositionY + Utils.jumpDistance)
		{
			Jump ();
		}
		if (head.y < 
		    headStablePositionY - Utils.SQUAT_DISTANCE) {
			Squat (true);
		} else 
		{
			Squat(false);
		}
		
		
		
		var field = GetPositionXFromKinect (spine);
		//ChangePositionX (field);
		ChangePositionXDynamic ((spine.x + head.x) / 2.0f);;
		
	}
	
	
	void UpdateKeyboardPosition()
	{
		if (Input.GetKey(KeyCode.UpArrow))
		{
			Jump();
			return;
		}
		if (Input.GetKey(KeyCode.Q))
		{
			ChangePositionX(Utils.FIELDS[0]);
			return;
		}
		if (Input.GetKey(KeyCode.W))
		{
			ChangePositionX(Utils.FIELDS[1]);
			return;
		}
		if (Input.GetKey(KeyCode.E))
		{
			ChangePositionX(Utils.FIELDS[2]);
			return;
		}
		if (Input.GetKey(KeyCode.R))
		{
			ChangePositionX(Utils.FIELDS[3]);
			return;
		}
		if (Input.GetKey(KeyCode.T))
		{
			ChangePositionX(Utils.FIELDS[4]);
			return;
		}
		if (Input.GetKey(KeyCode.Space))
		{
			Jump ();
			return;
		}
		if (Input.GetKey(KeyCode.X))
		{
			Squat (true);
			return;
		}
		if (Input.GetKey(KeyCode.Z))
		{
			Squat (false);
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
	
	bool IsGrounded()
	{
		//infotextSpine.text = transform.position.y.ToString();
		return transform.position.y  < Utils.GroundPosition;
	}
	
	// Update is called once per frame
	void Update () 
	{
		setText();
		Move ();
		if (IsGrounded ()) 
		{
			if (isSquat)
			{
				transform.position = new Vector3(transform.position.x, transform.position.y - Utils.PlayerYPostionSquatDiff, transform.position.z);
				GetComponent<Animation> ().Play ("RollAnimation");
			}
			
			else
			{
				transform.rotation = Quaternion.identity;
				GetComponent<Animation> ().Play ("run"); 
			}//play "run" animation if spacebar is not pressed
			//increase the speed of the movement by the factor "speed" 
		}
		try
		{
			UpdateKinectPostion();
		}
		catch
		{
			infotextHead.text = "Failed to find kinect";
		}
		UpdateKeyboardPosition();
		//infotextHead.text = transform.position.y.ToString();
		
		InvokeChangePosition ();
		
	}
	
	
	void InvokeChangePosition()
	{
		moveDirection.y -= gravity * Time.deltaTime;       //Apply gravity 
		//Debug.Log (moveDirection.ToString ());
		controller.Move(moveDirection * Time.deltaTime);      //Move the controller
		
	}
	
	void Move()
	{
		//if (transform.position.x - currentField.position > 0.5)
		//	moveDirection = new Vector3(-0.1f, moveDirection.y, 0.0f);  //get keyboard input to move in the horizontal direction
		//else if (transform.position.x - currentField.position < -0.1)
		//	moveDirection = new Vector3(0.1f, moveDirection.y, 0.0f);
		//moveDirection = transform.TransformDirection(moveDirection);  //apply this direction to the character
		//moveDirection.x *= speed; 
	}
	
	void ChangePositionX(Field field)
	{
		//oldField = currentField;
		//currentField = field;
		//Move ();
		
		transform.position = new Vector3 (field.position, transform.position.y, transform.position.z);
		text.text = field.name;
	}
	void ChangePositionXDynamic (float real)
	{
		real += 0.6f;
		real = real / 1.2f;
		real = real * 6.0f;
		real -= 3.0f;
		float gameX = real;
		if (gameX < -3.0f)
			gameX = -3.0f;
		if (gameX > 3.0f)
			gameX = 3.0f;
		transform.position = new Vector3 (gameX, transform.position.y, transform.position.z);
	}
	
	
	
	public void Jump()
	{
		if (IsGrounded ()) 
		{
			GetComponent<Animation>().Stop("run");
			GetComponent<Animation>().Play("jump_pose");
			moveDirection.y = jumpSpeed;
			infotextFootR.text += "Jump";
		}
		
		
	}
	
	public void Squat(bool value)
	{
		if (IsGrounded ()) 
		{
			infotextFootR.text += "Squat";
			isSquat = value;
		}
	}
	
	
	
	//check if the character collects the powerups or the snags
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.name == "Powerup(Clone)")
		{
			score += 50;
		}
		else if(other.gameObject.name == "Obstacle(Clone)")
		{
			score += 100; 
		}
		else if (other.gameObject.name == "Pipe(Clone)")
		{
			live--;
			Debug.Log("Live: " + live);
			if (live <= 0)
			{
				checkTopScores();
				Application.LoadLevel("scoreMenu");
				
			}                
		}
		
		Destroy(other.gameObject); 
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
	void checkTopScores()
	{
		foreach(playerScore ps in Globals.score.player)
		{
			if(this.score>=ps.score)
			{
				playerScore player = new playerScore();
				player.score = this.score;
				player.nick = this.score.ToString();
				Globals.score.player.Add(player);
				Globals.SaveScore();
				return;
			};
		}
	}
}
