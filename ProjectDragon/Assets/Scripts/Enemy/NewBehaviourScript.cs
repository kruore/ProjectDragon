using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(st());
        Time.timeScale = 0.1f;
    }

    IEnumerator st()
    {
        yield return new WaitForSeconds(3.0f);
        GetComponent<Animator>().SetBool("New Bool", true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void temp()
    {
        Debug.Log("Temp");
    }
}
