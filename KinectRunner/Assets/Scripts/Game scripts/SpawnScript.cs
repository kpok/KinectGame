using UnityEngine;
using System.Collections;

public class SpawnScript : MonoBehaviour {

	public GameObject obstacle;
	public GameObject powerup;
    public GameObject pipe;
	
	float timeElapsed = 0;
	float spawnCycle = 0.5f;
	bool spawnPowerup = true;

	// Update is called once per frame
	void Update () {
        
        int number = Random.Range(1, 4);

		timeElapsed += Time.deltaTime;
		if(timeElapsed > spawnCycle)
		{
			GameObject temp;
			if(number==1)
			{
				temp = (GameObject)Instantiate(powerup);
				Vector3 pos = temp.transform.position;
				temp.transform.position = new Vector3(Random.Range(-3, 4), pos.y, pos.z);
			}
			else if(number==2)
			{
				temp = (GameObject)Instantiate(obstacle);
				Vector3 pos = temp.transform.position;
				temp.transform.position = new Vector3(Random.Range(-3, 4), pos.y, pos.z);
			}
            else if (number == 3)
            {
                temp = (GameObject)Instantiate(pipe);
                Vector3 pos = temp.transform.position;
                temp.transform.position = new Vector3(Random.Range(-3, 4), pos.y, pos.z);
            }
			
			timeElapsed -= spawnCycle;
			spawnPowerup = !spawnPowerup;
		}
	}
}
