using UnityEngine;
using UnityEngine.AI;

public class ChickenCollect : MonoBehaviour
{
    //public int chickenCount;
    //public GameObject hitPoint;
    public float rayDistance = 1f;    // Distance in front
    public float rayHeight = 2f;      // Height above ground to start
    public float radius = 1.0f;
    public float maxDistance = 2.0f;
    //public float spawnRange = 10f;    // Editable range for Gizmos
    private Vector3 hitPoint;
    //private bool hasHit = false;
    public Inventory inventoryScript;
    public GameObject player;
   public int chickenNavMeshAreaMask;
// Start is called once before the first execution of Update after the MonoBehaviour is created
void Start()
    {
        inventoryScript = GetComponent<Inventory>();
        chickenNavMeshAreaMask = NavMesh.GetAreaFromName("ChickenPen");
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
            Ray ray = new Ray(transform.position,transform.forward* maxDistance);
            RaycastHit hit;
            //float radius = 0.5f;
            //Vector3 origin = hitPoint.transform.position;
            //Vector3 direction = hitPoint.transform.forward;
            //RaycastHit hit;
            // Calculate ray origin (slightly in front and above)
            //Vector3 rayOrigin = transform.position + (transform.forward * rayDistance) + (Vector3.up * rayHeight);

            //RaycastHit hit;
            // Raycast downwards
            Debug.Log("running pickup");
            if (Physics.SphereCast(ray,radius, out hit, maxDistance))
            {
                Debug.Log("Hit: " + hit.collider.name);
                Debug.DrawLine(transform.position, hit.point, Color.purple);

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
            else
            {
                Debug.DrawRay(transform.position, transform.forward* maxDistance, Color.hotPink);
            }
        }
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellowNice;
        Gizmos.DrawWireSphere(transform.position + transform.forward * maxDistance, radius);
    }
    //public void OnTriggerStay(Collider other)
    //{
    //    if(other.gameObject.CompareTag("Pen"))
    //    {
    //        PlaceChicken();
    //        Debug.Log("In pen");
    //    }
    //}
    public void PlaceChicken()
    {
        //Debug.Log("able to place");
        if(Input.GetMouseButtonDown(1) && GameManager.Instance.chickenCount>=1)
        {
            Debug.Log("running chicken place");
            // Calculate ray origin (slightly in front and above)
            Vector3 rayOrigin = transform.position + (transform.forward * rayDistance) + (Vector3.up * rayHeight);

            RaycastHit hit;
            // Raycast downwards
            if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 100f))
            {
                NavMeshHit navHit;
                //int chickenArea = NavMesh.GetAreaFromName("ChickenPen");
                if (NavMesh.SamplePosition(hit.point, out navHit, 2f, NavMesh.AllAreas))
                {
                    int chickenArea = NavMesh.GetAreaFromName("ChickenPen");
                    if (chickenArea == -1)
                    {
                        Debug.LogError("ChickenPen area NOT FOUND. Check spelling + NavMesh bake.");
                        return;
                    }

                    //NavMeshPath path = new NavMeshPath();
                    //NavMeshAgent tempAgent = GetComponent<NavMeshAgent>();
                    int hitAreaMask = navHit.mask;
                    if ((hitAreaMask & (1 << chickenArea)) != 0)
                    {
                        GameManager.Instance.PlaceChicken(navHit.position);
                        GameManager.Instance.RemoveChicken(1);
                        Debug.Log("placing chicken");
                        return;
                    }
                }
                Debug.Log("Couldn't place chicken (invalid NavMesh for chicken)");
                //     //hitPoint = hit.point;
                //     //hasHit = true;
                //     //Vector3 spawnpos = hit.point;
                //     GameManager.Instance.PlaceChicken(hit.point);
                // GameManager.Instance.RemoveChicken(1);
                //// inventoryScript.UpdateInventory(7, -1);
                // Debug.Log("placing chicken");
            }
            else
            {
                Debug.Log("Couldn't place chicken (no ground hit)");
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
