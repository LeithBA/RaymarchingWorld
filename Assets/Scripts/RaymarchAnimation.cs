using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaymarchAnimation : MonoBehaviour
{
    private Raymarch R;

    private void Start()
    {
        R = this.GetComponent<Raymarch>();
    }

    private void Update()
    {
        AnimateSphere1();
        AnimateBox1();
		AnimateCamera();
    }

    private void AnimateSphere1()
    {
        R._sphere1 = new Vector4(0,
                                 0,
                                 0,
                                 Mathf.PingPong(Time.time, 2)+10);
    }

    private void AnimateBox1()
    {
        R._box1 = new Vector4(0,
                              0,
                              0,
                              Mathf.PingPong(Time.time, 1)+9);
    }

	private void AnimateCamera()
	{
		this.transform.Translate(0,0,0.2f);
	}
}

