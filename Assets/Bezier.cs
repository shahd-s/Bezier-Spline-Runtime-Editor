using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier : MonoBehaviour
{   
    const int maxpoints = 1000;
    const int numpos = 50;
    public LineRenderer lr; 
    int meow = 0;
    bool cubic = true;
    public GameObject Linepoint;
    public GameObject ControlPoint;
    [SerializeField]
    List<GameObject> PointList = new List<GameObject>();

    [SerializeField]
    List<GameObject> ControlPointList = new List<GameObject>();

    public Vector3 sizee;
   
    public Vector3 newPos, oldPos;

    
    private Vector3[] pos = new Vector3[maxpoints];
    // Start is called before the first frame update
    void Start()
    {


       lr.positionCount = 0;
        lr.SetWidth((float)0.1, (float)0.1);


       // Invoke("MyFunction", 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) {
            newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPos.z = 0;


            if (CreatePointMarker(newPos) > 1)
            {
                Debug.Log("T");
               
                if (cubic)
                    drawCubicCurve(oldPos, newPos);

                else
                    drawQuadCurve(oldPos, newPos);
                meow++;

            }

            oldPos = newPos;

        }
    }

    private int CreatePointMarker(Vector3 pointPosition)
    {
        GameObject pm = (GameObject)Instantiate(Linepoint, pointPosition, Quaternion.identity, transform);
        PointList.Add(pm);

        MovePointMarker mctrlpt = pm.GetComponent<MovePointMarker>();
        mctrlpt.pointID = PointList.Count - 1;

        sizee =PointList[PointList.Count-1].transform.position;
        return PointList.Count;

    }

    private int AddControlPoint(Vector3 pointPosition)
    {
        GameObject cp = (GameObject)Instantiate(ControlPoint, pointPosition, Quaternion.identity, transform);
        ControlPointList.Add(cp);

        MoveCtrlPt mctrlpt = cp.GetComponent<MoveCtrlPt>();
        mctrlpt.pointID = ControlPointList.Count - 1;

        return ControlPointList.Count;
    }

    private int InsertControlPoint(Vector3 pointPosition, int id)
    {
        GameObject cp = (GameObject)Instantiate(ControlPoint, pointPosition, Quaternion.identity, transform);
        ControlPointList.Insert(id, cp);

        MoveCtrlPt mctrlpt = cp.GetComponent<MoveCtrlPt>();
        mctrlpt.pointID = id;

        return ControlPointList.Count;
    }

    public void MovePointMarker(int id, Vector3 pos)
    {
        //don't move the first point....
        oldPos = PointList[PointList.Count - 1].transform.position;

        if (cubic == false)
        {


            if( id == 0) {
                editQuadCurve(pos, PointList[id + 1].transform.position, id, ControlPointList[id].transform.position);
            }
            else {
                editQuadCurve(PointList[id - 1].transform.position, pos, id-1, ControlPointList[id-1].transform.position);
                //print("you are moving normal point number: ", id);
                editQuadCurve(pos, PointList[id+1].transform.position, id, ControlPointList[id].transform.position);

            }

        }
        else
        {
            if (id == 0)
            {
                editCubicCurve(pos, PointList[id + 1].transform.position, id, ControlPointList[0].transform.position, ControlPointList[1].transform.position);

            }
            else
            {
                editCubicCurve(PointList[id - 1].transform.position, pos, id - 1, ControlPointList[(id - 1) * 2].transform.position, ControlPointList[(id - 1) * 2 + 1].transform.position);
                //print("you are moving normal point number: "+id.ToString());
                editCubicCurve(pos, PointList[id + 1].transform.position, id, ControlPointList[id * 2].transform.position, ControlPointList[id * 2 + 1].transform.position);

            }
        }
        print(id); 
        print(pos);
    }

    public void MoveControlPoint(int id, Vector3 pos)
    {
        // print(id);

        if (cubic == false)
        {
              
            editQuadCurve(PointList[id].transform.position, PointList[id+1].transform.position, id, pos);

             
        }
        else
        {

            //CUBIC
            editCubicCurve(PointList[id / 2].transform.position, PointList[id / 2 + 1].transform.position, id / 2, ControlPointList[(id % 2 == 0) ? id : (id - 1)].transform.position, ControlPointList[(id % 2 == 0) ? (id + 1) : id].transform.position);

        }
    }


    private void drawCubicCurve(Vector3 p1, Vector3 p2)
    {
        Vector3 CtrlPt1, CtrlPt2;
        CtrlPt1.z = 0;
        CtrlPt1.x = ((float)((p1.x + p2.x + 1.7320508076 * (p1.y - p2.y)) / 2));
        CtrlPt1.y = ((float)((p1.y + p2.y + 1.7320508076 * (p2.x - p1.x)) / 2));
        CtrlPt2 = CtrlPt1;
        CtrlPt2.x += 2;
        AddControlPoint(CtrlPt1);
        AddControlPoint(CtrlPt2);
        for (int i = 1; i <= numpos; i++)
        {
            lr.positionCount ++;
            pos[(i - 1) + (numpos * (PointList.Count - 2))] = getcubemapoint((float)((float)i / (float)numpos), p1, CtrlPt1, CtrlPt2, p2);
            pos[(i - 1) + (numpos * (PointList.Count - 2))].z = 0;
        }

        lr.SetPositions(pos);
    }

    private void drawQuadCurve(Vector3 p1, Vector3 p2)
    {
        Vector3 CtrlPt;
        CtrlPt.z = 0;
        CtrlPt.x = ((float)((p1.x + p2.x + 1.7320508076 * (p1.y - p2.y)) / 2));
        CtrlPt.y = ((float)((p1.y + p2.y + 1.7320508076 * (p2.x - p1.x)) / 2));
        AddControlPoint(CtrlPt);
        for (int i = 1; i <=numpos; i++)
        {
            lr.positionCount++;
            pos[(i -1) + (numpos * (PointList.Count-2))] = getquadmapoint((float)((float)i / (float)numpos), p1, p2, CtrlPt);
            pos[(i-1 ) + (numpos * (PointList.Count-2))].z = 0;
        }
        lr.SetPositions(pos);
    }

    private void editQuadCurve(Vector3 p1, Vector3 p2, int whichseg, Vector3 CtrlPt)
    {
        //Delete the corresponding control point
       
        /* ControlPointList.Remove(ControlPointList[whichseg]);
        Destroy(ControlPointList[whichseg]);

        InsertControlPoint(CtrlPt, whichseg);
        */      
       // AddControlPoint(CtrlPt);
        for (int i = 1; i <= numpos; i++)
        {
            pos[(i - 1) + (numpos*whichseg)] = getquadmapoint((float)((float)i / (float)numpos), p1, p2, CtrlPt);
            pos[(i - 1) + (numpos * whichseg)].z = 0;
        }
        lr.SetPositions(pos);
    }

    private void editCubicCurve(Vector3 p1, Vector3 p2, int whichseg, Vector3 CtrlPt1, Vector3 CtrlPt2)
    {
        //Delete the corresponding control point

        /* ControlPointList.Remove(ControlPointList[whichseg]);
        Destroy(ControlPointList[whichseg]);

        InsertControlPoint(CtrlPt, whichseg);
        */
        // AddControlPoint(CtrlPt);
        for (int i = 1; i <= numpos; i++)
        {
            pos[(i - 1) + (numpos * whichseg)] = getcubemapoint((float)((float)i / (float)numpos), p1, CtrlPt1, CtrlPt2, p2);
            pos[(i - 1) + (numpos * whichseg)].z = 0;
        }
        lr.SetPositions(pos);
    }

    private void drawLinearCurve(Vector3 p1, Vector3 p2)
    {

        for (int i = 1; i <= numpos; i++)
        {
            pos[(i - 1)+numpos*(PointList.Count-1)] = getmapoint((float)((float)i / (float)numpos), p1, p2);
            pos[(i - 1)+numpos* (PointList.Count-1)].z = 0;
        }
        lr.SetPositions(pos);
    }

    private Vector3 getmapoint(float t, Vector3 p1, Vector3 p2)
    {
        Vector3 returnpoint;
        returnpoint.x = p1.x + t * (p2.x - p1.x);
        returnpoint.y = p1.y + t * (p2.y - p1.y);
        returnpoint.z = 0;
        return returnpoint;
    }

    private Vector3 getquadmapoint(float t, Vector3 p1, Vector3 p2, Vector3 controlPoint)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * p1;
        p += u * 2 * t * controlPoint;
        p += tt * p2;
        return p;

    }

    private Vector3 getcubemapoint(float t, Vector3 start, Vector3 ctrl1, Vector3 ctrl2, Vector3 end)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;
        Vector3 p = uuu * start;
        p += 3  *uu * t * ctrl1;
        p += 3  *u * tt * ctrl2;
        p += ttt * end;
        return p;


    }
}
