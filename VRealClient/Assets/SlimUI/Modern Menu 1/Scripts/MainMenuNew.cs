using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using SlimUI.ModernMenu;

namespace SlimUI.ModernMenu{
	public class MainMenuNew : MonoBehaviour {
		Animator CameraObject;
/*
		[Header("Loaded Scene")]
		[Tooltip("The name of the scene in the build settings that will load")]
		public string sceneName = ""; 
*/
		public enum Theme {custom1, custom2, custom3};
		[Header("Theme Settings")]
		public Theme theme;
		public FlexibleUIData themeController;
		int themeIndex;

		[Header("Panels")]
		[Tooltip("The UI Panel parenting all sub menus")]
		public GameObject mainCanvas;
		[Tooltip("The UI Panel that holds server and google in tab")]
		public GameObject PanelControls;
		[Tooltip("The UI Panel that holds the VIDEO window tab")]
		public GameObject PanelVideo;
		[Tooltip("The UI Panel that holds the GAME window tab")]
		public GameObject PanelGame;
		[Header("SFX")]
		[Tooltip("The GameObject holding the Audio Source component for the HOVER SOUND")]
		public AudioSource hoverSound;
		[Tooltip("The GameObject holding the Audio Source component for the AUDIO SLIDER")]
		public AudioSource sliderSound;
		[Tooltip("The GameObject holding the Audio Source component for the SWOOSH SOUND when switching to the Settings Screen")]
		public AudioSource swooshSound;

		// campaign button sub menu
		[Header("Menus")]
		[Tooltip("The Menu for when the MAIN menu buttons")]
		public GameObject mainMenu;
		[Tooltip("THe first list of buttons")]
		public GameObject firstMenu;
		[Tooltip("The Menu for when the PLAY button is clicked")]
		public GameObject playMenu;
		[Tooltip("The Menu for when the EXIT button is clicked")]
		public GameObject exitMenu;
		
	
	

		// highlights
		[Header("Highlight Effects")]
		[Tooltip("Highlight Image for when GAME Tab is selected in Settings")]
		public GameObject lineGame;
		[Tooltip("Highlight Image for when VIDEO Tab is selected in Settings")]
		public GameObject lineVideo;
		[Tooltip("Highlight Image for when CONTROLS Tab is selected in Settings")]
		public GameObject lineControls;
		[Tooltip("Highlight Image for when KEY BINDINGS Tab is selected in Settings")]
		

		[Header("LOADING SCREEN")]
		public GameObject loadingMenu;
		public Slider loadBar;
		public TMP_Text finishedLoadingText;
		public bool requireInputForNextScene = false;


		void Start(){
			CameraObject = transform.GetComponent<Animator>();
			mainMenu.SetActive(true);
			firstMenu.SetActive(true);
			playMenu.SetActive(false);
			exitMenu.SetActive(false);
			
			SetThemeColors();
		}

		
		void SetThemeColors(){
			if(theme == Theme.custom1){
				themeIndex = 0;
				themeController.currentColor = themeController.custom1.graphic1;
				themeController.textColor = themeController.custom1.text1;
			}else if(theme == Theme.custom2){
				themeIndex = 1;
				themeController.currentColor = themeController.custom2.graphic2;
				themeController.textColor = themeController.custom2.text2;
			}else if(theme == Theme.custom3){
				themeIndex = 2;
				themeController.currentColor = themeController.custom3.graphic3;
				themeController.textColor = themeController.custom3.text3;
			}
		}

		public void PlayCampaign(){
			exitMenu.SetActive(false);
			playMenu.SetActive(true);
		}
		

		public void ReturnMenu(){
			playMenu.SetActive(false);
			exitMenu.SetActive(false);
			mainMenu.SetActive(true);
		}
/*
		public void NewGame(string){
			if(sceneName != ""){
				StartCoroutine(LoadAsynchronously(sceneName));
			}
		}
*/
		//public void LoadScene(string scene){
		//	if(scene != ""){
		//		StartCoroutine(LoadAsynchronously(scene));
		//	}
		//}

		public void  DisablePlayCampaign(){
			playMenu.SetActive(false);
		}

		public void Position2(){
			DisablePlayCampaign();
			CameraObject.SetFloat("Animate",1);
		}

		public void Position1(){
			CameraObject.SetFloat("Animate",0);
		}


		void DisablePanels(){
			PanelControls.SetActive(false);
			PanelVideo.SetActive(false);
			PanelGame.SetActive(false);

			lineGame.SetActive(false);
			lineControls.SetActive(false);
			lineVideo.SetActive(false);

		}

		public void GamePanel(){
			DisablePanels();
			PanelGame.SetActive(true);
			lineGame.SetActive(true);
		}

		public void VideoPanel(){
			DisablePanels();
			PanelVideo.SetActive(true);
			lineVideo.SetActive(true);
		}

		public void ControlsPanel(){
			DisablePanels();
			PanelControls.SetActive(true);
			lineControls.SetActive(true);
		}


		public void PlayHover(){
			hoverSound.Play();
		}

		public void PlaySFXHover(){
			sliderSound.Play();
		}

		public void PlaySwoosh(){
			swooshSound.Play();
		}

		// Are You Sure - Quit Panel Pop Up
		public void AreYouSure(){
			exitMenu.SetActive(true);
			DisablePlayCampaign();
		}
		


		public void QuitGame(){
			#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
			#else
				Application.Quit();
			#endif
		}

		//IEnumerator LoadAsynchronously(string sceneName){ // scene name is just the name of the current scene being loaded
		//	AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
		//	operation.allowSceneActivation = false;
		//	mainCanvas.SetActive(false);
		//	loadingMenu.SetActive(true);

		//	while (!operation.isDone){
		//		float progress = Mathf.Clamp01(operation.progress / .9f);
		//		loadBar.value = progress;

		//		if(operation.progress >= 0.9f){
		//			if(requireInputForNextScene){
		//				finishedLoadingText.gameObject.SetActive(true);

		//				if(Input.anyKeyDown){
		//					operation.allowSceneActivation = true;
		//				}
		//			}else{
		//				operation.allowSceneActivation = true;
		//			}
		//		}
				
		//		yield return null;
		//	}
		//}
	}
}