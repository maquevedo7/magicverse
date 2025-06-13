using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnmerNotas : MonoBehaviour
{

    public GameObject[] notesPrefabs; // array de prefabs, do, re, mi, fa, sol
    public int amountNotes = 10;
    public Vector3 areaCenter = Vector3.zero;
    public Vector3 areasize = new Vector3(20, 3, 20);



    void Start()
    {
        GenerarNotas();
    }

    // Update is called once per frame


    void GenerarNotas()

    {
        for (int i = 0; i < amountNotes; i++)
        {

            Vector3 RandomPosition = areaCenter + new Vector3(
            Random.Range(-areasize.x / 2, areasize.x / 2),
            Random.Range(-areasize.y / 2, areasize.y / 2),
            Random.Range(-areasize.z / 2, areasize.z / 2)



        );
        }





    }





    private void OnDrawGizmosSelected()
    {
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube( areaCenter, areasize );


    }



}
