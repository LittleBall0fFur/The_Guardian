using UnityEngine;
using System.Collections;

public class MouseCollision : MonoBehaviour {
    private char oldDir;

    public Color normalColor;
    public Color triggerColor;

    private float speed = 0.12f;
    private float borderValue;
    private float offsetx;
    private float offsety;

    public GameObject objectToDetect; //use inspector
    public GameObject player;

    public SpriteRenderer renderer;

    // Use this for initialization
    void Start () {
        oldDir = 'n';
        offsetx = objectToDetect.transform.localPosition.x;
        offsety = objectToDetect.transform.localPosition.y;

        borderValue = GetComponentInParent<DrawRadar>().getRadius();
	}
	
	// Update is called once per frame
	void Update () {
        if (objectToDetect.transform.localPosition.y >= borderValue + offsety) moveCharacterNorth();
        else if (objectToDetect.transform.position.y <= ((borderValue - offsety) * -1)) moveCharacterSouth();
        else if (objectToDetect.transform.position.x >= borderValue + offsetx) moveCharacterEast();
        else if (objectToDetect.transform.localPosition.x <= ((borderValue - offsetx) * -1)) moveCharacterWest();
        else if (renderer.color != normalColor) renderer.color = normalColor; 
    }

    private void moveCharacterNorth()
    {
        rotate('n');
        if (!player.GetComponent<Animation>().IsPlaying("Attack")) player.GetComponent<Animation>().CrossFade("Walk");
        player.transform.Translate(Vector3.forward * speed);
        visualizeFeedback();
    }

    private void moveCharacterSouth()
    {
        rotate('s');
        if (!player.GetComponent<Animation>().IsPlaying("Attack")) player.GetComponent<Animation>().CrossFade("Walk");
        player.transform.Translate(Vector3.forward * speed);
        visualizeFeedback();
    }

    private void moveCharacterEast()
    {
        rotate('e');
        if (!player.GetComponent<Animation>().IsPlaying("Attack")) player.GetComponent<Animation>().CrossFade("Walk");
        player.transform.Translate(Vector3.forward * speed);
        visualizeFeedback();
    }

    private void moveCharacterWest()
    {
        rotate('w');
        if(!player.GetComponent<Animation>().IsPlaying("Attack"))player.GetComponent<Animation>().CrossFade("Walk");
        player.transform.Translate(Vector3.forward * speed);
        visualizeFeedback();
    }

    private void visualizeFeedback()
    {
        renderer.color = triggerColor;
    }

    private void rotate(char direction)
    {
        switch (direction)
        {
            case 'n':
                if(oldDir == 'e') player.transform.Rotate(0, 270, 0);
                if (oldDir == 's') player.transform.Rotate(0,180, 0);
                if (oldDir == 'w') player.transform.Rotate(0, 90, 0);
                break;

            case 'e':
                if (oldDir == 'n') player.transform.Rotate(0, 90, 0);
                if (oldDir == 's') player.transform.Rotate(0, 270, 0);
                if (oldDir == 'w') player.transform.Rotate(0, 180, 0);
                break;

            case 's':
                if (oldDir == 'n') player.transform.Rotate(0, 180, 0);
                if (oldDir == 'e') player.transform.Rotate(0, 90, 0);
                if (oldDir == 'w') player.transform.Rotate(0, 270, 0);
                break;

            case 'w':
                if (oldDir == 'n') player.transform.Rotate(0, 270, 0);
                if (oldDir == 'e') player.transform.Rotate(0, 180, 0);
                if (oldDir == 's') player.transform.Rotate(0, 90, 0);
                break;
        }
        oldDir = direction;
    }
}
