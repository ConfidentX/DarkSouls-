using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyIUserInput : UserInput{

	// Use this for initialization
	void Start () {
        //Dup = 1.0f;
        //Dright = 0;
       // rb = true;
    }
	
	// Update is called once per frame
	void Update () {
        UpdateDmagDvec(Dup, Dright);
	}
}
