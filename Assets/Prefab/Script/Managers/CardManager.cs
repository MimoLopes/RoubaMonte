using UnityEngine;

using Photon.Pun;


public class CardManager : MonoBehaviourPun
{
    [SerializeField]
    private GameObject cardPrefab;
    [SerializeField]
    private MaterialList materialList;
    [SerializeField]
    private float time;

    Vector3 velocity = Vector3.zero;
    public  string Name { get; private set; }
    public Vector3 Center { get; set; }
    public Transform stealler { get; set; }

    private void Start()
    {
        name = Name;
    }

    private void Update()
    {
        if(stealler == null)
        {
            MoveToCenter();
        }
        else
        {
            MoveToStealler(stealler);
        }
    }
    public void SetMaterial(string materialName)
    {
        Name = materialName;
        cardPrefab.GetComponent<SkinnedMeshRenderer>().material = materialList.Find(materialName);
    }

    private void MoveToCenter()
    {

        transform.position = Vector3.SmoothDamp(transform.position, Vector3.zero + Center, ref velocity, time * Time.deltaTime);
    }

    private void MoveToStealler(Transform stealler)
    {
        transform.position = Vector3.SmoothDamp(transform.position, stealler.position, ref velocity, time * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, stealler.rotation, time * Time.deltaTime);

        if(transform.position == stealler.position)
        {
            Destroy(gameObject);
        }
    }
}
