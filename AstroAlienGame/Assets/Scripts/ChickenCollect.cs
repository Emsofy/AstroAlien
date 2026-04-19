using UnityEngine;

public class ChickenCollect : MonoBehaviour
{
    //public int chickenCount;
    //public GameObject hitPoint;
    public float rayDistance = 5f;    // Distance in front
    public float rayHeight = 2f;      // Height above ground to start
    //public float spawnRange = 10f;    // Editable range for Gizmos
    private Vector3 hitPoint;
    //private bool hasHit = false;
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
    public void PickUpchicken()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //float radius = 0.5f;
            //Vector3 origin = hitPoint.transform.position;
            //Vector3 direction = hitPoint.transform.forward;
            //RaycastHit hit;
            // Calculate ray origin (slightly in front and above)
            Vector3 rayOrigin = transform.position + (transform.forward * rayDistance) + (Vector3.up * rayHeight);

            RaycastHit hit;
            // Raycast downwards
            Debug.Log("running pickup");
            if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 100f))
            {
                GameObject hitObj = hit.collider.gameObject;
                GameObject root = hitObj.transform.root.gameObject;
                if (root.CompareTag("Chicken"))
                {
                    Destroy(root.gameObject);
                    //chickenCount++;
                    //inventoryScript.UpdateInventory(7, 1);
                    GameManager.Instance.AddChicken(1);
                }
            }
        }
        //return chickenCount;
    }
    public void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.CompareTag("Pen"))
        {
            PlaceChicken();
            //Debug.Log("In pen");
        }
    }
    public void PlaceChicken()
    {
        if(Input.GetMouseButtonDown(1) && GameManager.Instance.chickenCount>=1)
        {
            // Calculate ray origin (slightly in front and above)
            Vector3 rayOrigin = transform.position + (transform.forward * rayDistance) + (Vector3.up * rayHeight);

            RaycastHit hit;
            // Raycast downwards
            if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 100f))
            {
                hitPoint = hit.point;
                //hasHit = true;
                //Vector3 spawnpos = hit.point;
                GameManager.Instance.PlaceChicken(hit.point);
                GameManager.Instance.RemoveChicken(1);
               // inventoryScript.UpdateInventory(7, -1);
                Debug.Log("placing chicken");
            }
            else
            {
                Debug.Log("Couldn't place chicken");
            }
        }
        //return chickenCount;
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Egg"))
        {
            GameManager.Instance.AddEgg(1);
            Destroy(collision.gameObject);
            // GameManager.Instance.activeEggs.Remove(collision.gameObject);
            //SaveSystem.SaveGame();

        }
        if (collision.gameObject.CompareTag("GoldenEgg"))
        {
            GameManager.Instance.AddGoldenEgg(1);
            Destroy(collision.gameObject);
            //GameManager.Instance.activeEggs.Remove(collision.gameObject);
            // SaveSystem.SaveGame();
        }
    }
}
