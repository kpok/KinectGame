using UnityEngine;
using System.Collections;

public class CollisionControler : MonoBehaviour {

    private Controller hController;
    void Start()
    {
        hController = GameObject.Find("Ethan").GetComponent<Controller>();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Red" || col.gameObject.name=="Black")
        {
            hController.BadCollision(col.gameObject);          
        }
        else if (col.gameObject.name == "Yellow" || col.gameObject.name == "Blue")
        {
            hController.BonusCollision(col.gameObject);
        }
    }
}
