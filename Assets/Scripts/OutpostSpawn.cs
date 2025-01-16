using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutpostSpawn : MonoBehaviour
{
    public GameObject outpost;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 6; i++)
        {
            Instantiate(outpost, new Vector3(Random.Range(1000,3000),150,Random.Range(1000,3000)), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
