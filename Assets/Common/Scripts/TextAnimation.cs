using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextAnimation : MonoBehaviour
{
	private TextMeshPro _tmp;
	private Vector3 _localScale;
    void Start()
    {
		_tmp = GetComponent<TextMeshPro>();
    }
    void Update()
    {
	}
}
