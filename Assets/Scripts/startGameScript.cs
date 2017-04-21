using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class startGameScript : MonoBehaviour {

    RaycastHit hit;
    private float gazeTime;
    public GameObject startQuad;
    public GameObject quitQuad;
    private Transform startQuad_T;
    private Transform quitQuad_T;
    AudioSource audio;

    // Use this for initialization
    void Start () {
        gazeTime = 0.0f;
        audio = gameObject.GetComponent<AudioSource>();
        audio.Play();

        startQuad_T = (Transform)GetComponent<Transform>();
        quitQuad_T = (Transform)GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {

        if (Physics.Raycast(transform.position, transform.forward, out hit, 60))
        {
            if (hit.collider.tag.Contains("startQuad"))
            {
                startQuad.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
                gazeTime += Time.deltaTime;
                if (gazeTime > 4.0f)
                {
                    SceneManager.LoadScene(1);
                }
            }
            else if (hit.collider.tag.Contains("quitQuad"))
            {
                quitQuad.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
                gazeTime += Time.deltaTime;
                if (gazeTime > 4.0f)
                {
                    Application.Quit();
                }
            }
        }
        else
        {
            startQuad.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
            quitQuad.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
            gazeTime = 0.0f;
        }
    }
}
