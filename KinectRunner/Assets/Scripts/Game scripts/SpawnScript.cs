using UnityEngine;
using System.Collections;

public class SpawnScript : MonoBehaviour {

	public GameObject obstacle;
	public GameObject powerup;
    public GameObject pipe;
	
	float timeElapsed = 0;
	float spawnCycle = Utils.SPAWN_CYCLE;

	// Update is called once per frame
	void Update () {
        
        int number = Random.Range(1, 4);

		timeElapsed += Time.deltaTime;
		if(timeElapsed > spawnCycle)
		{

			GameObject temp = null;
			if(number==1)
			{
				temp = (GameObject)Instantiate(powerup);

			}
			else if(number==2)
			{
				temp = (GameObject)Instantiate(obstacle);
			}
            else if (number == 3)
            {
                temp = (GameObject)Instantiate(pipe);
            }
			Vector3 pos = temp.transform.position;
			int posId = Random.Range(0, Utils.FIELDS.Count);
			temp.transform.position = new Vector3(Utils.FIELDS[posId].position, pos.y, pos.z);
			timeElapsed -= spawnCycle;
		}
	}
}
