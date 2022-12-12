using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathParticleScript : MonoBehaviour
{
    void Start()
    {
        //this.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        Destroy(this.gameObject, 3f);
    }

}
