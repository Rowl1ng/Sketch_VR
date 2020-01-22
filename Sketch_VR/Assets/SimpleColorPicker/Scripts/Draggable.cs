using UnityEngine;

public class Draggable : MonoBehaviour
{
    public GameObject rightController;

    public Transform minBound;

    public bool fixX;
	public bool fixY;
	public Transform thumb;	
	public bool dragging;
    private GameObject space;

    private void Start()
    {
        thumb.localPosition = thumb.localPosition = new Vector3(0.0f, 0.0f, -0.001f);
        space = GameObject.Find("space");
    }
    void FixedUpdate()
	{
        
        if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger, OVRInput.Controller.Touch))
        {
            
            dragging = false;
            space.GetComponent<BoxCollider>().enabled = true;
            Ray ray = new Ray(rightController.transform.position, rightController.transform.forward);
            // var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (GetComponent<Collider>().Raycast(ray, out hit, 100))
            {
                dragging = true;
                space.GetComponent<BoxCollider>().enabled = false;
            }
        }
  
        if (OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger, OVRInput.Controller.Touch)) dragging = false;
        // if (Input.GetMouseButtonUp(0)) dragging = false;
        if (dragging && OVRInput.Get(OVRInput.Button.SecondaryHandTrigger, OVRInput.Controller.Touch))
        {
            Ray ray = new Ray(rightController.transform.position, rightController.transform.forward);
            // var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (GetComponent<Collider>().Raycast(ray, out hit, 100))
            {
                var point = hit.point;
                point = GetComponent<Collider>().ClosestPointOnBounds(point);
                SetThumbPosition(point);
                //SetDragPoint(point);
                SendMessage("OnDrag", Vector3.one - (thumb.localPosition - minBound.localPosition) / GetComponent<BoxCollider>().size.x);

            }
        }
	}

	void SetDragPoint(Vector3 point)
	{
		point = (Vector3.one - point) * GetComponent<Collider>().bounds.size.x + GetComponent<Collider>().bounds.min;
		SetThumbPosition(point);
	}

	void SetThumbPosition(Vector3 point)
	{
        //thumb.position = new Vector3(fixX ? thumb.position.x : point.x, fixY ? thumb.position.y : point.y, thumb.position.z);
        Vector3 temp = thumb.localPosition;
        thumb.position = point;
        thumb.localPosition = new Vector3(fixX ? temp.x : thumb.localPosition.x, fixY ? temp.y : thumb.localPosition.y, thumb.localPosition.z - 1);
    }
}
