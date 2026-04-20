using UnityEngine;

public class TreeCollect : MonoBehaviour
{
    //public int woodCount = 0;
    //public int seedCount = 0;
    //public int appleCount = 0;
    public bool hasGoldApple = false;
    //public GameObject hitPoint;
    public float rayDistance = 1f;    // Distance in front
    public float rayLong = 4f;
    public float rayHeight = 2f;      // Height above ground to start
    public float spawnRange = 10f;    // Editable range for Gizmos
    private Vector3 hitPoint;
    private bool hasHit = false;
    //public float treeDistance = 2f; //distance player is away from tree for raycast to work

    public GameObject stumpPrefab;
    public float offest = 1.0f; //offset for raycast 
    public Inventory inventoryScript;

    //blah blah test test please be fixed 
    //inventory components
    //public Inventory inventoryScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       // moveScript = GetComponent<Movement>();
       inventoryScript = GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        //ChopTree();
        //PlaceTree();
    }

    public void PlaceTree()
    {
        if (Input.GetMouseButtonDown(1) && GameManager.Instance.seedCount >= 1 && GameManager.Instance.selectedItem == 2) //add && to check if placeable tile (collision compare tag planter plot)
        {
            //Vector3 origin = hitPoint.transform.position;
            //Vector3 direction = hitPoint.transform.forward;
            //Vector3 forwardPos = transform.position + transform.forward * treeDistance;
            //RaycastHit hit;
            // Calculate ray origin(slightly in front and above)
            Vector3 rayOrigin = transform.position + (transform.forward * rayDistance) + (Vector3.up * rayHeight);

            RaycastHit hit;
            // Raycast downwards
            //if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 100f
            if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 100f))
            {
                hitPoint= hit.point;
                hasHit = true;
                //Vector3 spawnpos = hit.point;
                GameManager.Instance.PlantTree(hitPoint);
                //GameManager.Instance.seedCount--;
                Debug.Log("planting seed");
                //inventoryScript.UpdateInventory(2, -1);
                GameManager.Instance.RemoveSeed(1);
            }
        }
        //else if 
        //{
        //    Debug.Log("couldn't plant seed");
        //}
        //return seedCount;
    }

    public void ChopTree()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Vector3 rayOrigin = transform.position + (transform.forward * rayLong);

            //RaycastHit hit;

            //float radius = 0.5f;
            //Vector3 origin = hitPoint.transform.position;
            //Vector3 direction = hitPoint.transform.forward;
            //RaycastHit hit;
            Vector3 forward = transform.TransformDirection(Vector3.forward) * 2.5f;
            Debug.DrawRay(transform.position + transform.up * 1.5f, forward, Color.green); // Visualize in editor

            //{
            //    Debug.Log("Hit: " + hit.transform.name);
            //}
            //Debug.Log("running tree chop");
            //if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 100f))
            if (Physics.Raycast(transform.position + transform.up * 1.5f, forward, out RaycastHit hit, 2.5f))
            {
                GameObject hitObj = hit.collider.gameObject;
                GameObject root = hitObj.transform.root.gameObject;

                if (root.CompareTag("Tree") || root.CompareTag("AppleTree") || root.CompareTag("GoldenTree"))
                {
                    Debug.Log("Chopping" + root.tag);
                    TreeGrow tree = root.GetComponent<TreeGrow>();
                    if (tree != null)
                    {
                        GameManager.Instance.activeTrees.Remove(tree);
                    }
                    SpawnStump(root);
                    //Destroy(hit.collider.gameObject);
                    //SpawnStump(hit.collider.gameObject);
                    //TreeGrow mapTree = hit.collider.GetComponent<TreeGrow>();

                    //if (mapTree != null)
                    //{
                    //    GameManager.Instance.activeTrees.Remove(mapTree);
                    //    Destroy(mapTree.gameObject);
                    //    //SaveSystem.SaveGame();
                    //}

                    int woodRand = Random.Range(5, 11);
                    Debug.Log("wood given:" + woodRand);
                    //woodCount += woodRand;
                    // inventoryScript.UpdateInventory(1, woodRand);
                    GameManager.Instance.AddWood(woodRand);


                    int seedRand = Random.Range(1, 4);
                    Debug.Log("seed given:" + seedRand);
                    //seedCount += seedRand;
                    //inventoryScript.UpdateInventory(2, seedRand);
                    GameManager.Instance.AddSeed(seedRand);

                    if (root.CompareTag("AppleTree"))
                    {
                        //appleCount += 3;
                        Debug.Log("apple given: 3");
                        GameManager.Instance.AddApple(3);
                        //inventoryScript.UpdateInventory(5, appleCount);
                    }
                    else if (root.CompareTag("GoldenTree"))
                    {
                        //appleCount += 1;
                        GameManager.Instance.AddApple(1);
                        GameManager.Instance.AddGoldenApple(1);
                        hasGoldApple = true;
                        Debug.Log("gold apple got got");
                    }
                    
                    SaveSystem.SaveGame();

                }
                else
                {
                    Debug.Log("hit: " + hit.collider);
                    Debug.Log("Hit object: " + hit.collider.name + " | Tag: " + hit.collider.tag);
                }

            }
            //else
            //{
            //    Debug.Log("didn't hit a tree");
            //}
               //Debug.DrawRay(transform.position, rayOrigin * rayDistance, Color.yellow, 0.5f);
        }
       // return seedCount;
    }
    // Show Gizmo in Scene View
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 rayOrigin = transform.position + (transform.forward * rayDistance) + (Vector3.up * rayHeight);

        // Draw the line being cast
        Gizmos.DrawLine(rayOrigin, rayOrigin + Vector3.down * 100f);

        // Draw gizmo at the hit point
        if (hasHit)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(hitPoint, 0.5f);
        }
    }
    void SpawnStump(GameObject tree)
    {
        // Capture location/rotation before destroying
        Vector3 pos = tree.transform.position;
        //Quaternion rot = tree.transform.rotation;

        Destroy(tree); // Remove the tree
        Instantiate(stumpPrefab, pos, Quaternion.Euler(-90,0,0)); // Spawn the stump
    }

    //function for tree growth 
    //4 hours for tree to grow
    //progress bar onto of tree to show how long left
    //make debug to complete timer
    //when tree is done give 2 apples 
    // 1/10 chance to get golden apple 
    //if golden apple is on a tree dont spawn any more


}
