using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueMove : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
            StartCoroutine(TextDisappear());
    }

    IEnumerator TextDisappear()
    {
        yield return new WaitForSeconds(2f);
        /*if (transform.position.y < 1000)
        {
            transform.position += transform.up * 1500 * Time.deltaTime;
        }
        else
        {
            transform.position = new Vector3(0, 1000, 0);
        }*/
        gameObject.SetActive(false);
    }
}
