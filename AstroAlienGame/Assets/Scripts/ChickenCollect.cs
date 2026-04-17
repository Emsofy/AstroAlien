using UnityEngine;

public class ChickenCollect : MonoBehaviour
{
    //public int chickenCount;
    public GameObject hitPoint;
    public Inventory inventoryScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryScript = GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
       // PickUpchicken();
        //PlaceChicken();
    }
    public int PickUpchicken(int chickenCount)
    {
        if (Input.GetMouseButtonDown(0))
        {
            float radius = 0.5f;
            Vector3 origin = hitPoint.transform.position;
            Vector3 direction = hitPoint.transform.forward;
            RaycastHit hit;
            Debug.Log("running pickup");
            if (Physics.SphereCast(origin, radius, direction, out hit))
            {
                GameObject hitObj = hit.collider.gameObject;
                GameObject root = hitObj.transform.root.gameObject;
                if (root.CompareTag("Chicken"))
                {
                    Destroy(root.gameObject);
                    chickenCount++;
                    inventoryScript.UpdateInventory(7, 1);
                }
            }
        }
        return chickenCount;
    }
    public void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.CompareTag("Pen"))
        {
            PlaceChicken(GameManager.Instance.chickenCount);
            //Debug.Log("In pen");
        }
    }
    public int PlaceChicken(int chickenCount)
    {
        if(Input.GetMouseButtonDown(1) && chickenCount>=1)
        {
            Vector3 forwardPos = transform.position + transform.forward;
            RaycastHit hit;
            if(Physics.Raycast(forwardPos + Vector3.up * 5f, Vector3.down, out hit))
            {
                Vector3 spawnpos = hit.point;
                GameManager.Instance.PlaceChicken(spawnpos);
                chickenCount--;
                inventoryScript.UpdateInventory(7, -1);
                Debug.Log("placing chicken");
            }
            else
            {
                Debug.Log("Couldn't place chicken");
            }
        }
        return chickenCount;
    }
}
