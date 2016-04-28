using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class ShowHideLog : MonoBehaviour {
	public GameObject log;
	public Text LogText;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowLog()
	{
		log.SetActive (true);

	}

	public void UpdateLog(string a_text)
	{

		//log.SetActive (true);
	
		LogText.text = LogText.text + "\n" + a_text;
	}

	public void HideLog()
	{
		log.SetActive (false);
	}

	public void ClearLog()
	{
		LogText.text = "";
	}

}
