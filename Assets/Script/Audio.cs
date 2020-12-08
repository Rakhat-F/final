
using UnityEngine;

public class Audio : MonoBehaviour
{
    PlayerController cc;
    void Start()
    {
        cc = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            GetComponent<AudioSource>().Play();
        }
    }

}