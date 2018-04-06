﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tempete : MonoBehaviour {

	public Transform ventPrefab;
	public Transform eclairPrefab;
	private GameObject player;

	private static Tempete instance;
	private Rigidbody rb;
	private int indiceCercleActuel;

	void Awake(){
		player = GameObject.FindGameObjectWithTag("Player");
		instance = this;
		rb = GetComponent<Rigidbody>();
		indiceCercleActuel = -1;
	}

	// Use this for initialization
	void Start () {
		startNextCercle();
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyUp(KeyCode.V)){
			ajouterVent();
		}

		if(Input.GetKeyUp(KeyCode.B)){
			supprimerVent();
		}

		if(Input.GetKeyUp(KeyCode.R)){
			augmenterVitesseRotation();
		}

		if(Input.GetKeyUp(KeyCode.T)){
			diminuerVitesseRotation();
		}

		if(Input.GetKeyUp(KeyCode.D)){
			resetTempete();
		}

		if(Input.GetKeyUp(KeyCode.S)){
			startNextCercle();
		}
	}

	public static Tempete getInstance(){
		return instance;
	}

	public void ajouterVent(){

		Vector3 toPlayer = (this.transform.position - player.transform.position).normalized;

		Quaternion quat = Quaternion.LookRotation((-toPlayer-2.0f*player.transform.forward).normalized, Vector3.up);

		Instantiate(ventPrefab, player.transform.position, quat, this.transform.GetChild(indiceCercleActuel));

		DigitalRuby.LightningBolt.LightningBoltScript eclair = 
			Instantiate(eclairPrefab, player.transform.position, quat, this.transform)
			.gameObject.GetComponent<DigitalRuby.LightningBolt.LightningBoltScript>();

		eclair.StartPosition = this.transform.position;

		eclair.EndPosition = player.transform.position;
	}

	public void augmenterVitesseRotation(){
		rb.AddTorque(0.0f, -10.0f, 0.0f, ForceMode.Acceleration);
	}

	public void supprimerVent(){
		Transform t = this.transform.GetChild(indiceCercleActuel);
		if(t.childCount > 0){
			Destroy(t.GetChild(t.childCount-1).gameObject);
		}
	}

	public void diminuerVitesseRotation(){
		rb.AddTorque(0.0f, 10.0f, 0.0f, ForceMode.Acceleration);
	}

	public void resetTempete(){
		rb.angularVelocity = new Vector3(0f,0f,0f);
		for(int i=this.transform.childCount-1; i>=0; i--){
			Destroy(this.transform.GetChild(i).gameObject);
		}
		indiceCercleActuel = -1;
		startNextCercle();
	}

	public void startNextCercle(){
		indiceCercleActuel++;
		player.transform.Translate(new Vector3(10.0f, 0.0f, 0.0f));
		GameObject cercle = new GameObject("Cercle_" + indiceCercleActuel);
		cercle.transform.position = this.transform.position;
		cercle.transform.parent = this.transform;
	}
}