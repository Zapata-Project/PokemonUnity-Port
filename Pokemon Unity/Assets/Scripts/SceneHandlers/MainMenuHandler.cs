//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class MainMenuHandler : MonoBehaviour
{
    public int selectedButton = 0;
    public int selectedFile = 0;
    public bool newGame = false;
    public Texture buttonSelected;
    public Texture buttonDimmed;
    public Sprite playerSprite;
    private GameObject fileDataPanel;
    private GameObject continueButton;

    private GUITexture[] button = new GUITexture[3];
    private GUITexture[] buttonHighlight = new GUITexture[3];
    private GUIText[] buttonText = new GUIText[3];
    private GUIText[] buttonTextShadow = new GUIText[3];

    private GUIText fileNumbersText;
    private GUIText fileNumbersTextShadow;
    private GUIText fileSelected;

    private GUIText mapNameText;
    private GUIText mapNameTextShadow;
    private GUIText dataText;
    private GUIText dataTextShadow;
    private GUITexture[] pokemon = new GUITexture[6];
    private DialogBoxHandler Dialog;
    public AudioClip selectClip; 
    public AudioClip newGameMusic; 
    private string playerName;
	private bool gender;
    public Pokemon.Gender playerGender;
    void Awake()
    {
        SaveLoad.Load();

        fileDataPanel = transform.Find("FileData").gameObject;
        continueButton = transform.Find("Continue").gameObject;

        Transform newGameButton = transform.Find("NewGame");
        Transform settingsButton = transform.Find("Settings");

        Transform[] buttonTransforms = new Transform[]
        {
            continueButton.transform,
            newGameButton,
            settingsButton
        };
        for (int i = 0; i < 3; i++)
        {
            button[i] = buttonTransforms[i].Find("ButtonTexture").GetComponent<GUITexture>();
            buttonHighlight[i] = buttonTransforms[i].Find("ButtonHighlight").GetComponent<GUITexture>();
            buttonText[i] = buttonTransforms[i].Find("Text").GetComponent<GUIText>();
            buttonTextShadow[i] = buttonText[i].transform.Find("TextShadow").GetComponent<GUIText>();
        }

        fileNumbersText = continueButton.transform.Find("FileNumbers").GetComponent<GUIText>();
        fileNumbersTextShadow = fileNumbersText.transform.Find("FileNumbersShadow").GetComponent<GUIText>();
        fileSelected = fileNumbersText.transform.Find("FileSelected").GetComponent<GUIText>();

        mapNameText = fileDataPanel.transform.Find("MapName").GetComponent<GUIText>();
        mapNameTextShadow = mapNameText.transform.Find("MapNameShadow").GetComponent<GUIText>();
        dataText = fileDataPanel.transform.Find("DataText").GetComponent<GUIText>();
        dataTextShadow = dataText.transform.Find("DataTextShadow").GetComponent<GUIText>();

        for (int i = 0; i < 6; i++)
        {
            pokemon[i] = fileDataPanel.transform.Find("Pokemon" + i).GetComponent<GUITexture>();
        }
        Dialog = gameObject.GetComponent<DialogBoxHandler>();
    }

    void Start()
    {
        StartCoroutine(control());
    }

    private void updateButton(int newButtonIndex)
    {
        if (newButtonIndex != selectedButton)
        {
            button[selectedButton].texture = buttonDimmed;
            buttonHighlight[selectedButton].enabled = false;
        }
        selectedButton = newButtonIndex;

        button[selectedButton].texture = buttonSelected;
        buttonHighlight[selectedButton].enabled = true;
    }

    private void updateFile(int newFileIndex)
    {
        selectedFile = newFileIndex;

        Vector2[] highlightPositions = new Vector2[]
        {
            new Vector2(132, 143),
            new Vector2(147, 143),
            new Vector2(162, 143)
        };
        fileSelected.pixelOffset = highlightPositions[selectedFile];
        fileSelected.text = "" + (selectedFile + 1);

        if (SaveLoad.savedGames[selectedFile] != null)
        {
            int badgeTotal = 0;
            for (int i = 0; i < 12; i++)
            {
                if (SaveLoad.savedGames[selectedFile].gymsBeaten[i])
                {
                    badgeTotal += 1;
                }
            }
            string playerTime = "" + SaveLoad.savedGames[selectedFile].playerMinutes;
            if (playerTime.Length == 1)
            {
                playerTime = "0" + playerTime;
            }
            playerTime = SaveLoad.savedGames[selectedFile].playerHours + " : " + playerTime;

            mapNameText.text = SaveLoad.savedGames[selectedFile].mapName;
            mapNameTextShadow.text = mapNameText.text;
            dataText.text = SaveLoad.savedGames[selectedFile].playerName
                            + "\n" + badgeTotal
                            + "\n" + "0" //Pokedex not yet implemented
                            + "\n" + playerTime;
            dataTextShadow.text = dataText.text;

            for (int i = 0; i < 6; i++)
            {
                if (SaveLoad.savedGames[selectedFile].PC.boxes[0][i] != null)
                {
                    pokemon[i].texture = SaveLoad.savedGames[selectedFile].PC.boxes[0][i].GetIcons();
                }
                else
                {
                    pokemon[i].texture = null;
                }
            }
        }
    }

    private IEnumerator animateIcons()
    {
        while (true)
        {
            for (int i = 0; i < 6; i++)
            {
                pokemon[i].border = new RectOffset(32, 0, 0, 0);
            }
            yield return new WaitForSeconds(0.15f);
            for (int i = 0; i < 6; i++)
            {
                pokemon[i].border = new RectOffset(0, 32, 0, 0);
            }
            yield return new WaitForSeconds(0.15f);
        }
    }
    private IEnumerator openAnimNewGame(){
		float scrollSpeed = 0.5f;
		float increment = 0;
		while (increment < 1){
			if(!newGame) {
				increment += (1/scrollSpeed)*Time.deltaTime;
				if (increment > 1){
					increment = 1;}
				transform.Find("FileData").position = new Vector3(0.5f*increment, transform.Find("FileData").position.y, transform.Find("FileData").position.z);
				transform.Find("Continue").position = new Vector3(-0.5f*increment, transform.Find("FileData").position.y, transform.Find("FileData").position.z);
				transform.Find("NewGame").position = new Vector3(-0.5f*increment, transform.Find("FileData").position.y, transform.Find("FileData").position.z);
				transform.Find("Settings").position = new Vector3(-0.5f*increment, transform.Find("FileData").position.y, transform.Find("FileData").position.z);
				 
			}
			if(transform.Find("FileData").position == new Vector3(0.5f, transform.Find("").position.y, transform.Find("FileData").position.z) || newGame) {
				if(!newGame) {
                    BgmHandler.main.PlayMain(newGameMusic, 0, true);
                    yield return StartCoroutine(ScreenFade.main.Fade(false, 0.2f));
                    transform.Find("Background").gameObject.SetActive(false);
                    transform.Find("FileData").gameObject.SetActive(false);
                    transform.Find("Continue").gameObject.SetActive(false);
                    transform.Find("NewGame").gameObject.SetActive(false);
                    transform.Find("Settings").gameObject.SetActive(false);
                    yield return StartCoroutine(ScreenFade.main.Fade(true, 0.2f));
                    newGame = true;
                }
                GlobalVariables.global.SetRPCLargeImageKey("newgame","Main Menu");
                GlobalVariables.global.SetRPCState("Starting a new game...");
				GlobalVariables.global.UpdatePresence();
                yield return new WaitForSeconds(1f);
                Dialog.drawDialogBox();
                yield return Dialog.StartCoroutine("drawText","Are you a boy?\nOr are you a girl?");
                Dialog.drawChoiceBox(new string[]{"Boy","Girl"});
                yield return new WaitForSeconds(0.2f);
                yield return StartCoroutine(Dialog.choiceNavigate());
                int chosenIndexa = Dialog.chosenIndex;
                if(chosenIndexa == 1){ //boy
                    playerName = "Daniel"; //john
                    gender = true;
                    Dialog.undrawChoiceBox();	
                } else if(chosenIndexa == 0) { //girl
                    playerName = "Daria"; //jane
                    gender = false;
                    Dialog.undrawChoiceBox();
                }
                Dialog.drawDialogBox();
                yield return Dialog.StartCoroutine("drawText","What is your name?");
                while(!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back")){yield return null;}
                Dialog.undrawDialogBox();
                yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));
                Scene.main.Typing.gameObject.SetActive(true);
                if(gender){
                    playerGender = Pokemon.Gender.MALE;
                    playerSprite = null;
                }
                else {
                    playerGender = Pokemon.Gender.FEMALE;
                    playerSprite = null;
                }
                StartCoroutine(Scene.main.Typing.control(7,playerName,playerGender,new Sprite[]{playerSprite}));
                while(Scene.main.Typing.gameObject.activeSelf){yield return null;}
                if(Scene.main.Typing.typedString.Length > 0){playerName = Scene.main.Typing.typedString;}
                BgmHandler.main.PlayMain(null, 0, false);
				SaveData.currentSave = new SaveData(SaveLoad.getSavedGamesCount());

				GlobalVariables.global.CreateFileData(playerName, gender); 

				GlobalVariables.global.playerPosition = new Vector3(35,-30,12);
				GlobalVariables.global.playerDirection = 2;
				GlobalVariables.global.fadeIn = true;
                SaveData.currentSave.setCVariable("indoors",1);
				GlobalVariables.global.SetRPCLargeImageKey("player_house","Alferez Village");
                GlobalVariables.global.SetRPCDetails("At home.");
				if(PokemonDatabase.getPokemon(SaveData.currentSave.PC.boxes[0][0].getID()).getName() == SaveData.currentSave.PC.boxes[0][0].getName()){
					GlobalVariables.global.SetRPCState("Follower: " + SaveData.currentSave.PC.boxes[0][0].getName() + " (Level " + SaveData.currentSave.PC.boxes[0][0].getLevel().ToString() + ")");
				}
				else {
					GlobalVariables.global.SetRPCState("Follower: " + SaveData.currentSave.PC.boxes[0][0].getName() + " (" + PokemonDatabase.getPokemon(SaveData.currentSave.PC.boxes[0][0].getID()).getName() + ", Level " + SaveData.currentSave.PC.boxes[0][0].getLevel().ToString() + ")");
				}
				UnityEngine.SceneManagement.SceneManager.LoadScene("overworld");
            }
            yield return null;
        }
    }
    private IEnumerator openAnim(){
		BgmHandler.main.PlayMain(null, 0, false);
		float scrollSpeed = 0.5f;
		float increment = 0;
		while (increment < 1){
			increment += (1/scrollSpeed)*Time.deltaTime;
			if (increment > 1){
				increment = 1;}
			transform.Find("FileData").position = new Vector3(0.5f*increment, transform.Find("FileData").position.y, transform.Find("FileData").position.z);
			transform.Find("Continue").position = new Vector3(-0.5f*increment, transform.Find("FileData").position.y, transform.Find("FileData").position.z);
			transform.Find("NewGame").position = new Vector3(-0.5f*increment, transform.Find("FileData").position.y, transform.Find("FileData").position.z);
			transform.Find("Settings").position = new Vector3(-0.5f*increment, transform.Find("FileData").position.y, transform.Find("FileData").position.z);
			if(transform.Find("FileData").position == new Vector3(0.5f, transform.Find("FileData").position.y, transform.Find("FileData").position.z)) {
                
				yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));
				GlobalVariables.global.debug(SaveLoad.savedGames[0].ToString());
				//GlobalVariables.global.debug(SaveLoad.savedGames[1].ToString());
				//GlobalVariables.global.debug(SaveLoad.savedGames[2].ToString());
				GlobalVariables.global.debug(SaveData.currentSave.playerPosition.v3.ToString());
				GlobalVariables.global.playerPosition = SaveData.currentSave.playerPosition.v3;
				GlobalVariables.global.playerDirection = SaveData.currentSave.playerDirection;
				if(PokemonDatabase.getPokemon(SaveData.currentSave.PC.boxes[0][0].getID()).getName() == SaveData.currentSave.PC.boxes[0][0].getName()){
					GlobalVariables.global.SetRPCState("Follower: " + SaveData.currentSave.PC.boxes[0][0].getName() + " (Level " + SaveData.currentSave.PC.boxes[0][0].getLevel().ToString() + ")");
				}
				else {
					GlobalVariables.global.SetRPCState("Follower: " + SaveData.currentSave.PC.boxes[0][0].getName() + " (" + PokemonDatabase.getPokemon(SaveData.currentSave.PC.boxes[0][0].getID()).getName() + ", Level " + SaveData.currentSave.PC.boxes[0][0].getLevel().ToString() + ")");
				}
				UnityEngine.SceneManagement.SceneManager.LoadScene(SaveData.currentSave.levelName);
				//yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));
			}
			yield return null;
		}
	}
    public IEnumerator control()
    {
        yield return StartCoroutine(ScreenFade.main.Fade(true, 0f));
        if(!newGame) {
            int fileCount = SaveLoad.getSavedGamesCount();
            GlobalVariables.global.SetRPCLargeImageKey("main_menu","Main Menu");
            GlobalVariables.global.SetRPCState("In the Main Menu");
            GlobalVariables.global.UpdatePresence();
            if (fileCount == 0)
            {
                updateButton(1);
                continueButton.SetActive(false);
                fileDataPanel.SetActive(false);
                for (int i = 1; i < 3; i++)
                {
                    button[i].pixelInset = new Rect(button[i].pixelInset.x, button[i].pixelInset.y + 64f,
                        button[i].pixelInset.width, button[i].pixelInset.height);
                    buttonHighlight[i].pixelInset = new Rect(buttonHighlight[i].pixelInset.x,
                        buttonHighlight[i].pixelInset.y + 64f, buttonHighlight[i].pixelInset.width,
                        buttonHighlight[i].pixelInset.height);
                    buttonText[i].pixelOffset = new Vector2(buttonText[i].pixelOffset.x, buttonText[i].pixelOffset.y + 64f);
                    buttonTextShadow[i].pixelOffset = new Vector2(buttonTextShadow[i].pixelOffset.x,
                        buttonTextShadow[i].pixelOffset.y + 64f);
                }
            }
            else
            {
                updateButton(0);
                updateFile(0);

                StartCoroutine(animateIcons());

                if (fileCount == 1)
                {
                    fileNumbersText.text = "File     1";
                }
                else if (fileCount == 2)
                {
                    fileNumbersText.text = "File     1   2";
                }
                else if (fileCount == 3)
                {
                    fileNumbersText.text = "File     1   2   3";
                }
            }

            bool running = true;
            while (running)
            {
                if(newGame)
                    running = false;
                if (Input.GetButtonDown("Select"))
                {
                    if(selectedButton == 0){		//CONTINUE
                        SfxHandler.Play(selectClip);
                        SaveData.currentSave = SaveLoad.savedGames[selectedFile];
                        yield return StartCoroutine("openAnim");
                        
                    }
                    else if(selectedButton == 1){	//NEW GAME
                        SfxHandler.Play(selectClip);
                        yield return StartCoroutine("openAnimNewGame");
                    }
                    else if (selectedButton == 2)
                    {
                        //SETTINGS
                        SfxHandler.Play(selectClip);
                        //yield return new WaitForSeconds(sceneTransition.FadeOut(0.4f));
                        yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));

                        Scene.main.Settings.gameObject.SetActive(true);
                        StartCoroutine(Scene.main.Settings.control());
                        while (Scene.main.Settings.gameObject.activeSelf)
                        {
                            yield return null;
                        }

                        //yield return new WaitForSeconds(sceneTransition.FadeIn(0.4f));
                        yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));
                    }
                }
                else if(Input.GetKeyDown(KeyCode.Delete)){
                    Dialog.drawDialogBox();
                    yield return Dialog.StartCoroutine("drawText","Are you sure you want to delete Save #"+(selectedFile+1)+"?");
                    Dialog.drawChoiceBoxNo();
                    yield return new WaitForSeconds(0.2f);
                    yield return StartCoroutine(Dialog.choiceNavigateNo());
                    int chosenIndex = Dialog.chosenIndex;
                    if(chosenIndex == 1){
                        SaveLoad.resetSaveGame(selectedFile);
                        GlobalVariables.global.debug("Save "+(selectedFile+1)+" was deleted!");
                        Dialog.undrawDialogBox();
                        Dialog.undrawChoiceBox();
                        Dialog.drawDialogBox();
                        yield return Dialog.StartCoroutine("drawText","Save #"+(selectedFile+1)+" was deleted!");
                        yield return new WaitForSeconds(2f);
                        yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));
                        
                        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
                    } else {
                        Dialog.undrawDialogBox();
                        Dialog.undrawChoiceBox();
                    }
                }
                else
                {
                    if (Input.GetAxisRaw("Vertical") > 0)
                    {
                        float minimumButton = (continueButton.activeSelf) ? 0 : 1;
                        if (selectedButton > minimumButton)
                        {
                            updateButton(selectedButton - 1);
                            SfxHandler.Play(selectClip);
                            yield return new WaitForSeconds(0.2f);
                        }
                    }
                    else if (Input.GetAxisRaw("Vertical") < 0)
                    {
                        if (selectedButton < 2)
                        {
                            updateButton(selectedButton + 1);
                            SfxHandler.Play(selectClip);
                            yield return new WaitForSeconds(0.2f);
                        }
                    }
                    if (Input.GetAxisRaw("Horizontal") > 0)
                    {
                        if (selectedButton == 0)
                        {
                            if (selectedFile < fileCount - 1)
                            {
                                updateFile(selectedFile + 1);
                                SfxHandler.Play(selectClip);
                                yield return new WaitForSeconds(0.2f);
                            }
                        }
                    }
                    else if (Input.GetAxisRaw("Horizontal") < 0)
                    {
                        if (selectedButton == 0)
                        {
                            if (selectedFile > 0)
                            {
                                updateFile(selectedFile - 1);
                                SfxHandler.Play(selectClip);
                                yield return new WaitForSeconds(0.2f);
                            }
                        }
                    }
                }


                yield return null;
                }
        }
    }
}