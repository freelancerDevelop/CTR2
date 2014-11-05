﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Menus : MonoBehaviour
{
	public Texture normal;
	public Texture hover;
	public Texture triVolume3;
	public Texture triVolume2;
	public Texture triVolume1;
	public static GameObject cameraMenu;
	private GameObject greyT;
	private float normalTime;
	private int heightLabel=100;
	private int position=0;
	private int positionH=1;
	private bool authorizeNavigate = false;
	private bool waitingForKey = false;
	private bool inPause = false;
	private static GameObject titreAffiche;
	private static GameObject triangleFond;
	private static GameObject triangleVolume;
	private static GameObject textPlayer;
	private static GameObject fleches;
	private static KeyCode keyPressed;
	private static List <KeyCode> listKeys=  new List <KeyCode>();
	private static List <GameObject> flechesD =  new List <GameObject>();
	private static List <GameObject> textureAffichees =  new List <GameObject>();
	private static List <GameObject> textAffiches =  new List <GameObject>();
	private static List <GameObject> controlAffiches =  new List <GameObject>();
	private static Dictionary <int, string> menuCourant = new Dictionary<int, string>();
	public Main main;

	private bool readyToMove = true;

	// menus :
	private static Dictionary <int, string> menuPause =  new Dictionary<int, string>
	{
		{0,"Start"},
		{1,"REPRENDRE"},
		{2,"RECOMMENCER"},
		{3,"CHANGER PERSONNAGE"},
		{4,"CHANGER NIVEAU"},
		{5,"CHANGER CONFIG"},
		{6,"QUITTER"},
		{7,"OPTIONS"}
	};
	private static Dictionary <int, string> menuOptions =  new Dictionary<int, string>
	{
		{0,"Options"},
		{1,"VOLUME :"},
		{2,"REGLAGES CONTROLES"},
		{3,"RETOUR"}
	};
	private static Dictionary <int, string> menuReglages =  new Dictionary<int, string>
	{
		{0,"Reglages Controles"},
		{1,"JOUEUR :"},
		{2,"Avancer"},
		{3,"Reculer"},
		{4,"Tourner Gauche"},
		{5,"Tourner Droite"},
		{6,"Actionner Arme"},
		{7,"Sauter"},
		{8,"Inverser Camera"},
		{9,"Changer Vue"},
		{10,"Mettre en Pause"},
		{11,"RETOUR"}
	};
	// Use this for initialization
	void Start ()
	{
		normalTime = Time.timeScale;
		cameraMenu = (GameObject)GameObject.Instantiate (Resources.Load("cameraMenu"));
		//AudioListener.volume = 0.5f;
	}

	
	// Update is called once per frame
	void Update ()
	{
		navigateMenu ();
		testPause ();

	}

	void testPause()
	{
		bool pressStart = false;
		for (int i = 1; i<5; i++) pressStart |= Input.GetKeyDown (Dictionnaries.playersMapping [i] ["start"]) ;
		if (pressStart)
		{
			Pause();
		}
	}

	void Pause()
	{
		if (!inPause)
		{
			greyT = (GameObject)GameObject.Instantiate(Resources.Load("guiBlack"));
			inPause=true;
			displayMenu(menuPause);
			Time.timeScale=0f;
			AudioListener.pause = true;
		}
		else if (inPause)
		{
			inPause=false;
			viderMenu ();
			Destroy(greyT);
			Time.timeScale=normalTime;
			AudioListener.pause = false;
		}

	}
	void displayMenu(Dictionary <int, string> menu)
	{
		viderMenu ();
		cameraMenu.SetActive (true);
		position = 0;
		GameObject textTitre =(GameObject)Instantiate (Resources.Load ("textTitreMenu"),new Vector3(0.5f,0.5f+(((float)heightLabel/2)/(float)Screen.height)*menu.Count/2f,0),Quaternion.identity);
		textTitre.guiText.text=menu[0];
		titreAffiche = textTitre;
		for (int i = 1; i<menu.Count; i++)
		{
			if(!specialButton(menu,i))
			{
				Vector3 pos =new Vector3(0.5f,0.5f+(((float)heightLabel/2)/(float)Screen.height)*(menu.Count/2-i),-1);
				textureAffichees.Add((GameObject)Instantiate (Resources.Load ("menuButton"),pos,Quaternion.identity));
				GameObject textbutton =(GameObject)Instantiate (Resources.Load ("textButton"),new Vector3(pos.x,pos.y,0),Quaternion.identity);
				textbutton.guiText.text=menu[i];
				textAffiches.Add(textbutton);
			}
		}
		menuCourant = menu;
		authorizeNavigate = true;
	}

	bool specialButton(Dictionary <int, string> menu, int i)
	{
		if(menu[i]=="VOLUME :")
		{
			Vector3 pos =new Vector3(0.5f,0.5f+(((float)heightLabel/2)/(float)Screen.height)*(menu.Count/2-i),-1);
			textureAffichees.Add((GameObject)Instantiate (Resources.Load ("menuButton"),pos,Quaternion.identity));
			triangleFond = (GameObject)Instantiate (Resources.Load ("menuBackgroundTriangle"),new Vector3(pos.x,pos.y,2),Quaternion.identity);
			triangleVolume = (GameObject)Instantiate (Resources.Load ("menuVolumeTriangle"),new Vector3(pos.x,pos.y,3),Quaternion.identity);
			Rect t = new Rect(triangleVolume.guiTexture.pixelInset.x,triangleVolume.guiTexture.pixelInset.y,250*AudioListener.volume*1.42f,25*AudioListener.volume*1.42f);
			triangleVolume.guiTexture.pixelInset=t;
			GameObject textbutton =(GameObject)Instantiate (Resources.Load ("textButton"),new Vector3(pos.x-(float)((float)400/(float)((float)Screen.width*(float)3)),pos.y,0),Quaternion.identity);
			textbutton.guiText.text=menu[i];
			textAffiches.Add(textbutton);
			return true;
		}
		else if(menu[0]=="Reglages Controles" && menu[i]!="RETOUR")
		{
			if(menu[i]=="JOUEUR :")
			{
				Vector3 pos =new Vector3(0.5f,0.5f+(((float)heightLabel/2)/(float)Screen.height)*(menu.Count/2-i),-1);
				textureAffichees.Add((GameObject)Instantiate (Resources.Load ("menuButton"),pos,Quaternion.identity));
				fleches = (GameObject)Instantiate (Resources.Load ("menuFleches"),new Vector3(pos.x+(float)((float)400/(float)((float)Screen.width*(float)5)),pos.y,5),Quaternion.identity);
				GameObject textbutton =(GameObject)Instantiate (Resources.Load ("textButton"),new Vector3(pos.x-(float)((float)400/(float)((float)Screen.width*(float)4)),pos.y,0),Quaternion.identity);
				textbutton.guiText.text=menu[i];
				textAffiches.Add(textbutton);
				textPlayer = (GameObject)Instantiate (Resources.Load ("textButton"),new Vector3(pos.x+(float)((float)400/(float)((float)Screen.width*(float)5)),pos.y,6),Quaternion.identity);
				positionH=main.players[0].numeroJoueur;
				textPlayer.guiText.text="Joueur "+positionH;
			}
			else if(i>1)
			{
				Vector3 pos =new Vector3(0.5f,0.5f+(((float)heightLabel/2)/(float)Screen.height)*(menu.Count/2-i),-1);
				textureAffichees.Add((GameObject)Instantiate (Resources.Load ("menuButton"),pos,Quaternion.identity));
				GameObject textbutton =(GameObject)Instantiate (Resources.Load ("textButton"),new Vector3(pos.x-(float)((float)400/(float)((float)Screen.width*(float)2.2f)),pos.y,0),Quaternion.identity);
				textbutton.guiText.text=menu[i];
				textbutton.guiText.anchor=TextAnchor.MiddleLeft;
				textAffiches.Add(textbutton);
				GameObject textcontrol =(GameObject)Instantiate (Resources.Load ("textControl"),new Vector3(pos.x+(float)((float)400/(float)((float)Screen.width*(float)5)),pos.y,0),Quaternion.identity);
				string action =Dictionnaries.actions[menu[i]];
				KeyCode actual =Dictionnaries.playersMapping[positionH][action];
				textcontrol.guiText.text=actual.ToString();
				controlAffiches.Add(textcontrol);
				GameObject flecheD = (GameObject)Instantiate (Resources.Load ("menuFlecheD"),new Vector3(pos.x+(float)((float)400/(float)((float)Screen.width*(float)2.5f)),pos.y,5),Quaternion.identity);
				flechesD.Add(flecheD);
			}
			return true;
		}
		else return false;
	}

	void navigateMenu()
	{
		if(authorizeNavigate)
		{
			if(textureAffichees.Count>position)
			{
				for(int i=0; i<textAffiches.Count;i++)
				{
					textureAffichees [i].guiTexture.texture = normal;
					textureAffichees [position].guiTexture.texture = hover;
					textAffiches[i].guiText.color=Color.white;
					textAffiches[position].guiText.color=Color.black;
					if(menuCourant[0]=="Reglages Controles")
					{
						textPlayer.guiText.color=Color.white; 
						if(menuCourant[position+1]=="JOUEUR :")
						{
							textPlayer.guiText.color=Color.black;
						}
						if(i<controlAffiches.Count) controlAffiches[i].guiText.color=Color.white;
						if(position>0 && position<menuCourant.Count-2)
						{
							controlAffiches[position-1].guiText.color=Color.black;
						}
					}
				}
			}
			int nControllers = System.Math.Min(Input.GetJoystickNames ().Length + 2, 4);
			bool down = false;
			for (int i = 1; i<nControllers; i++)
			{
				down |= (Dictionnaries.controllersEnabled[i] && Input.GetAxis (Dictionnaries.axisMapping[i]["stop"]) == 1 );
				down |= (!Dictionnaries.controllersEnabled[i] && Input.GetKey (Dictionnaries.playersMapping [i] ["moveBack"]));
			}
			if (!readyToMove)
				down = false;
			else if (down && readyToMove)
				StartCoroutine(RestrictMovement());
			if (down && position<menuCourant.Count-2) position++;
			else if(down && !(position<menuCourant.Count-2)) position = 0;
			bool up = false;
			for (int i = 1; i<nControllers; i++)
			{
				up |= (Dictionnaries.controllersEnabled[i] && Input.GetAxis (Dictionnaries.axisMapping[i]["stop"]) == -1);
				up |= (!Dictionnaries.controllersEnabled[i] && Input.GetKey (Dictionnaries.playersMapping [i] ["moveForward"]));
			}
			if (!readyToMove)
				up = false;
			else if (up && readyToMove)
				StartCoroutine(RestrictMovement());
			if (up && position>0) position--;
			else if(up && !(position>0)) position = menuCourant.Count-2;
			bool ok = false;
			for (int i = 1; i<nControllers; i++)
			{
				ok |= (Dictionnaries.controllersEnabled[i] && Input.GetKeyDown (Dictionnaries.playersMapping [i] ["moveForward"]));
				ok |= (!Dictionnaries.controllersEnabled[i] && Input.GetKeyDown (Dictionnaries.playersMapping [i] ["action"]));
			}
			if(ok)
			{
				action(menuCourant,position);
			}
			
			bool right = false;
			bool left = false;
			for (int i = 1; i<nControllers; i++)
			{
				if (Dictionnaries.controllersEnabled[i]){
					right |= (Input.GetAxis (Dictionnaries.axisMapping[i]["turn"]) == 1 );
					left |= (Input.GetAxis (Dictionnaries.axisMapping[i]["turn"]) == -1 );
				}
				else{
					right |= (Input.GetKeyDown (Dictionnaries.playersMapping [i] ["turnRight"]));
					left |= (Input.GetKeyDown (Dictionnaries.playersMapping [i] ["turnLeft"]));
				}
			}
			if(right && menuCourant[position+1]=="VOLUME :" && AudioListener.volume<=0.692) AudioListener.volume+=0.008f;
			if(left && menuCourant[position+1]=="VOLUME :" && AudioListener.volume>=0.008f) AudioListener.volume-=0.008f;;
			if(menuCourant[position+1]=="VOLUME :")
			{
				Destroy(triangleVolume);
				Vector3 pos =new Vector3(0.5f,0.5f+(((float)heightLabel/2)/(float)Screen.height)*(menuCourant.Count/2-1),-1);
				triangleVolume = (GameObject)Instantiate (Resources.Load ("menuVolumeTriangle"),new Vector3(pos.x,pos.y,3),Quaternion.identity);
				Rect t = new Rect(triangleVolume.guiTexture.pixelInset.x,triangleVolume.guiTexture.pixelInset.y,250*AudioListener.volume*1.42f,25*AudioListener.volume*1.42f);
				triangleVolume.guiTexture.pixelInset=t;
				if(AudioListener.volume<0.4f) triangleVolume.guiTexture.texture=triVolume1;
				else if (AudioListener.volume>=0.4f && AudioListener.volume<0.6f) triangleVolume.guiTexture.texture=triVolume2;
				else triangleVolume.guiTexture.texture=triVolume3;
			}
			else if(menuCourant[position+1]=="JOUEUR :")
			{
				textPlayer.guiText.text="Joueur "+positionH;
				for(int i=0;i<controlAffiches.Count;i++)
				{
					string action =Dictionnaries.actions[menuCourant[i+2]];
					KeyCode actual =Dictionnaries.playersMapping[positionH][action];
					controlAffiches[i].guiText.text=actual.ToString();
				}
				if(right)
				{
					if(positionH<main.players.Count) positionH++;
					else positionH=1;
				}
				else if(left)
				{
					if(positionH>1) positionH--;
					else positionH=main.players.Count;
				}

			}
			else if((menuCourant[0]=="Reglages Controles") && (menuCourant[position+1]!="Reglages Controles") && (menuCourant[position+1]!="JOUEUR :") && (menuCourant[position+1]!="RETOUR"))
			{
				listKeys=  new List <KeyCode>();
				foreach(string a in Dictionnaries.playersMapping[positionH].Keys)
				{
					listKeys.Add(Dictionnaries.playersMapping[positionH][a]);
				}
				if(right)
				{
					authorizeNavigate=false;
					Destroy(flechesD[position-1]);
					controlAffiches[position-1].guiText.text="?";
					controlAffiches[position-1].guiText.color=Color.blue;
					waitingForKey = true;
				}
			}
		}
	}

	void OnGUI()
	{
	
		if(waitingForKey==true && Event.current.isKey)
		{

			StartCoroutine(getKey());
			waitingForKey=false;
		}
		if (Event.current.isKey && Event.current.keyCode.ToString () != "None")
		{
			keyPressed = Event.current.keyCode;
			Debug.Log (keyPressed);
		}
	}

	IEnumerator getKey()
	{
		for(int i=0;i<25;i++)
		{
			yield return new WaitForEndOfFrame ();
		}
		KeyCode temp = keyPressed;
		while(keyPressed==temp || listKeys.IndexOf(keyPressed)>-1)
		{
			yield return new WaitForEndOfFrame ();
		}
		controlAffiches[position-1].guiText.text=keyPressed.ToString();
		Vector3 pos =new Vector3(0.5f,0.5f+(((float)heightLabel/2)/(float)Screen.height)*(menuCourant.Count/2-position-1),-1);
		GameObject flecheD = (GameObject)Instantiate (Resources.Load ("menuFlecheD"),new Vector3(pos.x+(float)((float)400/(float)((float)Screen.width*(float)2.5f)),pos.y,5),Quaternion.identity);
		flechesD[position-1]=flecheD;
		Debug.Log (menuCourant [position +1]);
		string action =Dictionnaries.actions[menuCourant[position+1]];
		Dictionnaries.playersMapping [positionH] [action] = keyPressed;
		for(int i=0;i<25;i++)
		{
			yield return new WaitForEndOfFrame ();
		}
		authorizeNavigate=true;
	}


	IEnumerator RestrictMovement()
	{
		readyToMove = false;
		for(int i=0;i<10;i++)
			yield return new WaitForEndOfFrame ();
		readyToMove = true;
	}

	void action(Dictionary <int, string> menu,int p)
	{
		if(menu[0]=="Start")
		{
			switch (menu[p+1])
			{
			case "REPRENDRE":
				Pause();
				break;
			case "RECOMMENCER":
				Application.LoadLevel (Application.loadedLevel);
				Restart();
				break;
			case "OPTIONS":
				displayMenu(menuOptions);
				break;
			case "CHANGER NIVEAU":
				if (Application.loadedLevelName == "plage")
					Application.LoadLevel("dinoRace");
				else
					Application.LoadLevel("plage");
				Restart();
				break;
			case "QUITTER":
				Application.Quit();
				break;
			default:
				break;
			}
		}
		if(menu[0]=="Options")
		{
			switch (menu[p+1])
			{
			case "REGLAGES CONTROLES":
				displayMenu(menuReglages);
				break;
			case "RETOUR":
				displayMenu(menuPause);
				break;
			default:
				break;
			}
		}
		if(menu[0]=="Reglages Controles")
		{
			switch (menu[p+1])
			{
			case "RETOUR":
				displayMenu(menuOptions);
				break;
			default:
				break;
			}
		}
	}

	void Restart()
	{
		viderMenu ();
		Destroy (greyT);
		Time.timeScale = normalTime;
		Main.Restart();
	}

	void viderMenu()
	{
		authorizeNavigate = false;
		Destroy (titreAffiche);
		Destroy (triangleFond);
		Destroy (triangleVolume);
		Destroy (fleches);
		Destroy (textPlayer);
		foreach (GameObject g in textureAffichees) Destroy (g);
		foreach (GameObject g in textAffiches) Destroy (g);
		foreach (GameObject g in controlAffiches) Destroy (g);
		foreach (GameObject g in flechesD) Destroy (g);
		textureAffichees =  new List <GameObject>();
		textAffiches =  new List <GameObject>();
		controlAffiches =  new List <GameObject>();
		flechesD =  new List <GameObject>();
		cameraMenu.SetActive (false);
	}

}
