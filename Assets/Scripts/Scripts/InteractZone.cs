using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractZone : MonoBehaviour
{
    public Fungus.Flowchart Interaction;
    public Player.CardinalDirection direction;
	public bool activateOnTouch = false;

    public virtual void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            
            if (Player.instance.PlayerInControl == true && (Player.instance.Direction == direction) && (InputManager.AButton))
            {
                Debug.Log("Invoke attempt");
                Interaction.ExecuteBlock("Start");
                Debug.Log("Call Attempt");
            }
        }
    }

	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && activateOnTouch)
		{
			Interaction.ExecuteBlock("Start");
			if (Player.instance.Direction != direction)
				Player.instance.Direction = direction;
		}
	}

	//*extremely pewka voice - "I don't know where they go but they must just go."
	public void PlaceElswhere(Vector3 pos, Vector3 rot)
	{
		transform.position = pos;
		transform.rotation = Quaternion.Euler (rot);
	}
}
