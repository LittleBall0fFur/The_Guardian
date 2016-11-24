using UnityEngine;
using System.Collections;
using System.Timers;

public class followMouse : MonoBehaviour
{
    private bool teleport;

    private float negRadius; //south and east border of Radar
    private float posRadius; //north and west border of Radar
    private readonly float sensitivity = 20; //mouse event sensitivity

    public GameObject objectToMove; //use inspector

    private int pointer;

    private Timer t = new Timer();

    private Vector3[] mousePositions;



    // Use this for initialization
    void Start()
    {
        Cursor.visible = false;
        mousePositions = new Vector3[10];
        t.Elapsed += new ElapsedEventHandler(onTimer);
        t.Interval = 1000;

        float offset = objectToMove.transform.localPosition.x; //get offset from start position

        posRadius = GetComponentInParent<DrawRadar>().getRadius() + offset; //get radius add offset(start position)
        negRadius = (posRadius * -1); //convert radius to positive numeral

    }

    // Update is called once per frame
    void Update()
    {
        continuesMouseIntegration();
        if (pointer == 10)
        {
            pointer = 0;
            moveObjectPosition();
        }

        mousePositions[pointer] = Input.mousePosition;
        pointer++;
    }

    //move gameObject
    private void moveObjectPosition()
    {
        Vector3 previousPosition = mousePositions[0];
        Vector3 currentPosition = mousePositions[9];

        float vectorH = currentPosition.x - previousPosition.x;
        float vectorV = currentPosition.y - previousPosition.y;

        bool negativeH = vectorH < 0; //check if horizontal value is negative
        bool negativeV = vectorV < 0; //check if vertical value is negative

        if (teleport) //if recent continuesMouseIntegration event, do nothing
        {
            return;
        }
        if (negativeH) //if negative horizontal value
        {
            float convertedH = Mathf.Abs(vectorH); //convert negative horizontal value to positive for comparison with sensitivity
            if (convertedH > (sensitivity * 2.5)) //if converted horizontal value is higher then the sensitivity multiplied (prioritize vertical over horizontal)
            {
                horizontal(vectorH); //run horizontal modifyer
            }
        }
        if (negativeV) // if negative vertical value
        {
            float convertedV = Mathf.Abs(vectorV); //convert negative vertical value to positive for comparison with sensitivity
            if (convertedV > sensitivity) vertical(vectorV); //if converted vertical value is higher then the sensitivity run vertical modifyer
        }
        if (!negativeH) // if positive horizontal value
        {
            if (vectorH > (sensitivity * 2.5)) horizontal(vectorH);
        }
        if (!negativeV) //if positive vertical value
        {
            if (vectorV > sensitivity) vertical(vectorV);
        }
    }

    /**
     * Method to adjust the horizontal position of the gameobject we want to move
     * @param direction: east(positive) or west(negative)
     */
    private void horizontal(float direction)
    {
        if (direction > sensitivity) //change must be higher then desired sensitivity
        {
            if (objectToMove.transform.position.x <= negRadius) return; //if already at desired max distance do nothing and return;
            else
            {
                Vector3 newPosition = new Vector3(objectToMove.transform.position.x - 1, objectToMove.transform.position.y, objectToMove.transform.position.z);
                objectToMove.transform.position = newPosition;
            }
        }
        else if (direction < (sensitivity * -1)) //change must be higher then desired sensitivity. uses negative comparison for negative float
        {

            if (objectToMove.transform.position.x >= posRadius) return; //if already at desired max distance do nothing and return;
            else
            {
                Vector3 newPosition = new Vector3(objectToMove.transform.position.x + 1, objectToMove.transform.position.y, objectToMove.transform.position.z);
                objectToMove.transform.position = newPosition;
            }
        }
    }

    /**
     * Method to adjust the vertical position of the gameobject we want to move
     * @param direction: north(positive) or south(negative)
     */
    private void vertical(float direction)
    {
        if (direction > sensitivity)
        {
            if (objectToMove.transform.position.y >= posRadius) return; //if already at desired max distance do nothing and return;
            Vector3 newPosition = new Vector3(objectToMove.transform.position.x, objectToMove.transform.position.y + 1, objectToMove.transform.position.z);
            objectToMove.transform.position = newPosition;
        }
        else if (direction < (sensitivity * -1))
        {
            if (objectToMove.transform.position.y <= negRadius) return; //if already at desired max distance do nothing and return;
            Vector3 newPosition = new Vector3(objectToMove.transform.position.x, objectToMove.transform.position.y - 1, objectToMove.transform.position.z);
            objectToMove.transform.position = newPosition;
        }
    }

    /**
     *  Method to 'teleport' the mouse cursor if it gets stuck on the borders (north, east, south, west)
     *  !not native to Unity! will only work in fullscreen on actuall 1920 / 1080 screen
     *  Dll dependencies (windows only)
     *  left:   0, -
     *  top:    -, 0
     *  right:  1919, -
     *  bottum: -, 1079
     */
    private void continuesMouseIntegration()
    {
        Vector2 currentPosition = CursorControl.GetGlobalCursorPos();
        Vector2 newPosition;
        /** from south to north **/
        if (currentPosition.y <= 5)
        {
            newPosition = new Vector2(currentPosition.x,1040);
            CursorControl.SetGlobalCursorPos(newPosition);
            resetMousePositions();
        }

        /** from north to south **/
        else if (currentPosition.y >= 1050)
        {
            newPosition = new Vector2(currentPosition.x, 16);
            CursorControl.SetGlobalCursorPos(newPosition);
            resetMousePositions();
        }
        /** from east to west **/
        if (currentPosition.x <= 5)
        {
            newPosition = new Vector2(1790, currentPosition.y);
            CursorControl.SetGlobalCursorPos(newPosition);
            resetMousePositions();
        }

        /** from west to east **/
        else if (currentPosition.x >= 1800)
        {
            newPosition = new Vector2(6, currentPosition.y);
            CursorControl.SetGlobalCursorPos(newPosition);
            resetMousePositions();
        }
    }

    /**
     * Method to empty tracked mouse positions
     * Called after continuesMouseIntegration event
     **/
    private void resetMousePositions()
    {
        teleport = true;
        pointer = 0;

        for (int i = 0; i < mousePositions.Length; i++)
        {
            mousePositions[i] = Input.mousePosition;
        }

        t.Start();
    }

    /**
     * Method to disable GameObject positioning
     * this needs to be done after the mouse has 'teleported' to give this thread the time to empty previous recorded position
     * while update thread keeps running. This method simulates Semaphores
     */
    void onTimer(object source, ElapsedEventArgs e)
    {
        //do things 
        teleport = false;
    }
}