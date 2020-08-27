using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuirkManager : MonoBehaviour
{
    public GameObject[] Quirks;


    void Awake()
    {
        Instantiate(Quirks[0], transform);
    }
}
