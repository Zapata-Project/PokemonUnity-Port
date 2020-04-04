using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseHandlerNew : MonoBehaviour
{
    public Image pauseBottom;
    public Image pda;
    public Image generic;
    public Image iconPokedexPDA;
    public Image iconPartyPDA;
    public Image iconBagPDA;
    public Image iconTrainerPDA;
    public Image iconSavePDA;
    public Image iconSettingsPDA;
    public Image iconSaveGeneric;
    public Image iconSettingsGeneric;
    public Image saveDataDisplay;

    public Text mapName;
    public Text mapNameShadow;
    public Text dataText;
    public Text dataTextShadow;

    public DialogBoxHandlerNew Dialog;

    public AudioSource PauseAudio;
    public AudioClip selectClip;
    public AudioClip openClip;
    private int selectedIcon;
    private Image targetIcon;
    public Image selectArrow;
    public Text selectedTextPDA;
    public Text selectedTextPDAShadow;
    public Text selectedText;
    public Text selectedTextShadow;
    public Vector3[] selectPositions;
    public Text[] timePDA;
    public bool running;
    private Vector3 sliderPosition = new Vector3(0,-48,0);
    private Vector3 sliderPosition2 = new Vector3(0,-144,0);
    public Vector3[] sliderPositionsPDA;
    public bool openRunning = false;
    public bool closeRunning = false;
    public GameObject directions;
    void FixedUpdate()
    {
        foreach(Text timeText in timePDA) {
            timeText.text = System.DateTime.Now.Hour+":"+(System.DateTime.Now.Minute >= 10 ? System.DateTime.Now.Minute.ToString() : "0"+System.DateTime.Now.Minute.ToString());
        }
    }
    void Start()
    {
        pauseBottom.rectTransform.anchoredPosition = new Vector3(0,-144f,0);
        pauseBottom.gameObject.SetActive(false);
        selectArrow.gameObject.SetActive(false);
        generic.gameObject.SetActive(false);
        setSelectedText("");

        selectedIcon = 0;

        saveDataDisplay.gameObject.SetActive(false);

        pda.rectTransform.anchoredPosition = new Vector3(132.03f,-139.84f,0);
        SaveData.currentSave.setCVariable("NewPause",1);
        if(SaveData.currentSave.getCVariable("NewPause") == 1) {
            pda.gameObject.SetActive(true);
        } else {
            pda.gameObject.SetActive(false);
        }
    }
    private void setSelectedText(string text)
    {
        selectedText.text = text;
        selectedTextShadow.text = text;
        if(text == "Pokémon Party") {
            selectedTextPDA.fontSize = 8;
            selectedTextPDAShadow.fontSize = 8;
        } else {
            selectedTextPDA.fontSize = 10;
            selectedTextPDAShadow.fontSize = 10;
        }
        selectedTextPDA.text = text;
        selectedTextPDAShadow.text = text;
    }
    private IEnumerator openAnim()
    {
        pauseBottom.gameObject.SetActive(true);
        float speed = 250f;
        openRunning = true;
        closeRunning = false;
        if(SaveData.currentSave.getCVariable("NewPause") == 1) {
            pda.gameObject.SetActive(true);
            directions.SetActive(true);
            while(pda.rectTransform.anchoredPosition.y != -30f && openRunning && !closeRunning) {
                pda.rectTransform.anchoredPosition = Vector3.MoveTowards(pda.rectTransform.anchoredPosition, sliderPositionsPDA[1], Time.deltaTime * speed);
                yield return null;
            }
        } else {
            generic.gameObject.SetActive(true);
            while(pauseBottom.rectTransform.anchoredPosition.y < -48f && openRunning && !closeRunning) {
                pauseBottom.rectTransform.anchoredPosition = Vector3.MoveTowards(pauseBottom.rectTransform.anchoredPosition, sliderPosition, Time.deltaTime * speed);
                generic.rectTransform.anchoredPosition = Vector3.MoveTowards(generic.rectTransform.anchoredPosition, sliderPosition, Time.deltaTime * speed);
                yield return null;
            }
        }
        openRunning = false;
    }
    private IEnumerator closeAnim()
    {
        openRunning = false;
        closeRunning = true;
        float speed = 200f;
        selectArrow.gameObject.SetActive(false);
        if(SaveData.currentSave.getCVariable("NewPause") == 1) {
            while(pda.rectTransform.anchoredPosition.y != -140f && !openRunning && closeRunning) {
                pda.rectTransform.anchoredPosition = Vector3.MoveTowards(pda.rectTransform.anchoredPosition, sliderPositionsPDA[0], Time.deltaTime * speed);
                yield return null;
            }
        } else {
            while(pauseBottom.rectTransform.anchoredPosition.y > -144f && closeRunning && !openRunning) {
                pauseBottom.rectTransform.anchoredPosition = Vector3.MoveTowards(pauseBottom.rectTransform.anchoredPosition, sliderPosition2, Time.deltaTime * speed);
                generic.rectTransform.anchoredPosition = Vector3.MoveTowards(generic.rectTransform.anchoredPosition, sliderPosition2, Time.deltaTime * speed);
                yield return null;
            }
        }
        generic.gameObject.SetActive(false);
        closeRunning = false;
    }
    public IEnumerator updateIcon(int index)
    {
        float speed = 1f;
        float step = speed * Time.deltaTime;
        if (selectedIcon == 1)
        {
            selectArrow.gameObject.SetActive(true);
            targetIcon = iconPokedexPDA;
            setSelectedText("Pokédex");
            selectArrow.rectTransform.anchoredPosition = selectPositions[0];
        }
        else if (selectedIcon == 2)
        {
            selectArrow.gameObject.SetActive(true);
            targetIcon = iconPartyPDA;
            setSelectedText("Pokémon Party");
            selectArrow.rectTransform.anchoredPosition = selectPositions[1];
        }
        else if (selectedIcon == 3)
        {
            selectArrow.gameObject.SetActive(true);
            targetIcon = iconBagPDA;
            setSelectedText("Bag");
            selectArrow.rectTransform.anchoredPosition = selectPositions[2];
        }
        else if (selectedIcon == 4)
        {
            selectArrow.gameObject.SetActive(true);
            targetIcon = iconTrainerPDA;
            setSelectedText(SaveData.currentSave.playerName);
            selectArrow.rectTransform.anchoredPosition = selectPositions[3];
        }
        else if (selectedIcon == 5)
        {
            selectArrow.gameObject.SetActive(true);
            setSelectedText("Save Game");
            if(SaveData.currentSave.getCVariable("NewPause") == 1) {
                targetIcon = iconSavePDA;
                selectArrow.rectTransform.anchoredPosition = selectPositions[4];
            } else {
                targetIcon = iconSaveGeneric;
                selectArrow.rectTransform.anchoredPosition = new Vector3(-15.8305f,-45.501f,0);
            }
        }
        else if (selectedIcon == 6)
        {
            selectArrow.gameObject.SetActive(true);
            setSelectedText("Settings");
            if(SaveData.currentSave.getCVariable("NewPause") == 1) {
                targetIcon = iconSettingsPDA;
                selectArrow.rectTransform.anchoredPosition = selectPositions[5];
            } else {
                targetIcon = iconSettingsGeneric;
                selectArrow.rectTransform.anchoredPosition = new Vector3(16.5f,-45.501f,0);
            }
        }
        else
        {
            targetIcon = null;
            setSelectedText("");
        }
        yield return null;
    }
    public IEnumerator control()
    {
        selectedIcon = 0;
        setSelectedText("");
        SfxHandler.Play(openClip);
        yield return StartCoroutine(openAnim());
        running = true;
        while (running)
        {
            if(SaveData.currentSave.getCVariable("NewPause") == 1) {
                if (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)
                {
                    directions.SetActive(false);
                    if (Input.GetAxisRaw("Vertical") > 0)
                    {
                        if (selectedIcon == 0)
                            selectedIcon = 2;
                        else if (selectedIcon > 3)
                            selectedIcon -= 3;
                    }
                    else if (Input.GetAxisRaw("Horizontal") < 0)
                    {
                        if (selectedIcon == 0)
                            selectedIcon = 1;
                        if (selectedIcon != 1 && selectedIcon != 4)
                            selectedIcon -= 1;
                    }
                    else if (Input.GetAxisRaw("Vertical") < 0)
                    {
                        if (selectedIcon == 0)
                            selectedIcon = 5;
                        if (selectedIcon < 4)
                            selectedIcon += 3;
                    }
                    else if (Input.GetAxisRaw("Horizontal") > 0)
                    {
                        if (selectedIcon == 0)
                            selectedIcon = 3;
                        else if (selectedIcon != 3 && selectedIcon != 6)
                            selectedIcon += 1;
                    }
                    StartCoroutine(updateIcon(selectedIcon));
                    SfxHandler.Play(selectClip);
                    yield return new WaitForSeconds(0.2f);
                }
                else if (Input.GetButton("Select"))
                {
                    if (selectedIcon == 1)
                    {
                        //Pokedex
                        GlobalVariables.global.debug("Pokédex not yet implemented");
                        yield return new WaitForSeconds(0.2f);
                    }
                    else if (selectedIcon == 2)
                    {
                        //Party
                        SfxHandler.Play(selectClip);
                        //StartCoroutine(fadeIcons(0.4f));
                        //yield return new WaitForSeconds(sceneTransition.FadeOut(0.4f));
                        yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));
                        

                        yield return StartCoroutine(runSceneUntilDeactivated(Scene.main.Party.gameObject));

                        
                        //StartCoroutine(unfadeIcons(0.4f));
                        //yield return new WaitForSeconds(sceneTransition.FadeIn(0.4f));
                        yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));
                    }
                    else if (selectedIcon == 3)
                    {
                        //Bag
                        SfxHandler.Play(selectClip);
                        //StartCoroutine(fadeIcons(0.4f));
                        //yield return new WaitForSeconds(sceneTransition.FadeOut(0.4f));
                        yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));
                        

                        yield return StartCoroutine(runSceneUntilDeactivated(Scene.main.Bag.gameObject));

                        
                        //StartCoroutine(unfadeIcons(0.4f));
                        //yield return new WaitForSeconds(sceneTransition.FadeIn(0.4f));
                        yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));
                    }
                    else if (selectedIcon == 4)
                    {
                        //TrainerCard
                        SfxHandler.Play(selectClip);
                        //StartCoroutine(fadeIcons(0.4f));
                        //yield return new WaitForSeconds(sceneTransition.FadeOut(0.4f));
                        yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));
                        

                        yield return StartCoroutine(runSceneUntilDeactivated(Scene.main.Trainer.gameObject));

                        
                        //StartCoroutine(unfadeIcons(0.4f));
                        //yield return new WaitForSeconds(sceneTransition.FadeIn(0.4f));
                        yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));
                    }
                    else if (selectedIcon == 5)
                    {
                        //Save
                        saveDataDisplay.gameObject.SetActive(true);
                        saveDataDisplay.sprite =
                            Resources.Load<Sprite>("Frame/choice" + PlayerPrefs.GetInt("frameStyle"));

                        int badgeTotal = 0;
                        for (int i = 0; i < 12; i++)
                        {
                            if (SaveData.currentSave.gymsBeaten[i])
                            {
                                badgeTotal += 1;
                            }
                        }
                        string playerTime = "" + SaveData.currentSave.playerMinutes;
                        if (playerTime.Length == 1)
                        {
                            playerTime = "0" + playerTime;
                        }
                        playerTime = SaveData.currentSave.playerHours + " : " + playerTime;

                        mapName.text = PlayerMovement.player.accessedMapSettings.mapName;
                        dataText.text = SaveData.currentSave.playerName + "\n" +
                                        badgeTotal + "\n" +
                                        "0" + "\n" + //pokedex not yet implemented
                                        playerTime;
                        mapNameShadow.text = mapName.text;
                        dataTextShadow.text = dataText.text;

                        Dialog.DrawDialogBox();
                        yield return StartCoroutine(Dialog.DrawText("Would you like to save the game?"));
                        yield return StartCoroutine(Dialog.DrawChoiceBox(0));
                        int chosenIndex = Dialog.chosenIndex;
                        if (chosenIndex == 1)
                        {
                            //update save file
                            //Dialog.UndrawChoiceBox();
                            SaveData.currentSave.levelName = Application.loadedLevelName;
                            SaveData.currentSave.playerPosition = new SeriV3(PlayerMovement.player.transform.position);
                            SaveData.currentSave.playerDirection = PlayerMovement.player.direction;
                            SaveData.currentSave.mapName = PlayerMovement.player.accessedMapSettings.mapName;

                            NonResettingHandler.saveDataToGlobal();

                            SaveLoad.Save();
                            Dialog.DrawDialogBox();
                            yield return
                                StartCoroutine(Dialog.DrawText(SaveData.currentSave.playerName + " saved the game!"));
                            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                            {
                                yield return null;
                            }
                        }
                        Dialog.UndrawDialogBox();
                        Dialog.UndrawChoiceBox();
                        saveDataDisplay.gameObject.SetActive(false);
                        yield return new WaitForSeconds(0.2f);
                    }
                    else if (selectedIcon == 6)
                    {
                        //Settings
                        SfxHandler.Play(selectClip);
                        //StartCoroutine(fadeIcons(0.4f));
                        //yield return new WaitForSeconds(sceneTransition.FadeOut(0.4f));
                        yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));
                        
                        
                        yield return StartCoroutine(runSceneUntilDeactivated(Scene.main.Settings.gameObject));
                        
                        
                        //StartCoroutine(unfadeIcons(0.4f));
                        //yield return new WaitForSeconds(sceneTransition.FadeIn(0.4f));
                        yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));
                    }
                }

                if (Input.GetButton("Start") || Input.GetButton("Back"))
                {
                    running = false;
                }
                yield return null;
            } else {
                if (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)
                {
                    if (Input.GetAxisRaw("Horizontal") < 0)
                    {
                        if(selectedIcon == 6 || selectedIcon == 0) {
                            SfxHandler.Play(selectClip);
                            selectedIcon = 5;
                        }
                    }
                    else if (Input.GetAxisRaw("Horizontal") > 0)
                    {
                        if(selectedIcon == 5 || selectedIcon == 0) {
                            SfxHandler.Play(selectClip);
                            selectedIcon = 6;
                        }
                    }
                    StartCoroutine(updateIcon(selectedIcon));
                    yield return new WaitForSeconds(0.2f);
                }
                else if (Input.GetButton("Select"))
                {
                    if (selectedIcon == 5)
                    {
                        //Save
                        saveDataDisplay.gameObject.SetActive(true);
                        saveDataDisplay.sprite =
                            Resources.Load<Sprite>("Frame/choice" + PlayerPrefs.GetInt("frameStyle"));

                        int badgeTotal = 0;
                        for (int i = 0; i < 12; i++)
                        {
                            if (SaveData.currentSave.gymsBeaten[i])
                            {
                                badgeTotal += 1;
                            }
                        }
                        string playerTime = "" + SaveData.currentSave.playerMinutes;
                        if (playerTime.Length == 1)
                        {
                            playerTime = "0" + playerTime;
                        }
                        playerTime = SaveData.currentSave.playerHours + " : " + playerTime;

                        mapName.text = PlayerMovement.player.accessedMapSettings.mapName;
                        dataText.text = SaveData.currentSave.playerName + "\n" +
                                        badgeTotal + "\n" +
                                        "0" + "\n" + //pokedex not yet implemented
                                        playerTime;
                        mapNameShadow.text = mapName.text;
                        dataTextShadow.text = dataText.text;

                        Dialog.DrawDialogBox();
                        yield return StartCoroutine(Dialog.DrawText("Would you like to save the game?"));
                        yield return StartCoroutine(Dialog.DrawChoiceBox(0));
                        int chosenIndex = Dialog.chosenIndex;
                        if (chosenIndex == 1)
                        {
                            //update save file
                            Dialog.UndrawChoiceBox();
                            Dialog.UndrawDialogBox();

                            SaveData.currentSave.levelName = Application.loadedLevelName;
                            SaveData.currentSave.playerPosition = new SeriV3(PlayerMovement.player.transform.position);
                            SaveData.currentSave.playerDirection = PlayerMovement.player.direction;
                            SaveData.currentSave.mapName = PlayerMovement.player.accessedMapSettings.mapName;

                            NonResettingHandler.saveDataToGlobal();

                            SaveLoad.Save();

                            yield return
                                StartCoroutine(Dialog.DrawText(SaveData.currentSave.playerName + " saved the game!"));
                            while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                            {
                                yield return null;
                            }
                        }
                        Dialog.UndrawDialogBox();
                        Dialog.UndrawChoiceBox();
                        saveDataDisplay.gameObject.SetActive(false);
                        yield return new WaitForSeconds(0.2f);
                    }
                    else if (selectedIcon == 6)
                    {
                        //Settings
                        SfxHandler.Play(selectClip);
                        //StartCoroutine(fadeIcons(0.4f));
                        //yield return new WaitForSeconds(sceneTransition.FadeOut(0.4f));
                        yield return StartCoroutine(ScreenFade.main.Fade(false, 0.4f));
                        
                        pauseBottom.gameObject.SetActive(false);
                        generic.gameObject.SetActive(false);
                        yield return StartCoroutine(runSceneUntilDeactivated(Scene.main.Settings.gameObject));

                        pauseBottom.gameObject.SetActive(true);
                        generic.gameObject.SetActive(true);
                        //StartCoroutine(unfadeIcons(0.4f));
                        //yield return new WaitForSeconds(sceneTransition.FadeIn(0.4f));
                        yield return StartCoroutine(ScreenFade.main.Fade(true, 0.4f));
                    }
                }

                if (Input.GetButton("Start") || Input.GetButton("Back"))
                {
                    running = false;
                }
                yield return null;
            }
        }


        yield return StartCoroutine(closeAnim());
        pauseBottom.gameObject.SetActive(false);
        generic.gameObject.SetActive(false);
    }
    
    /// Only runs the default scene (no parameters)
    private IEnumerator runSceneUntilDeactivated(GameObject sceneInterface)
    {
        disableAll();
        sceneInterface.SetActive(true);
        sceneInterface.SendMessage("control");
        yield return new WaitForSeconds(0.05f);
        while (sceneInterface.activeSelf)
        {
            yield return null;
        }
        enableAll();
    }
    private void disableAll()
    {
       //setSelectedText("");
       generic.gameObject.SetActive(false);
       pauseBottom.gameObject.SetActive(false);
       selectArrow.gameObject.SetActive(false);
       hidePDA();
    }
    private void enableAll()
    {
       //setSelectedText("");
       //selectedIcon = 0;
       generic.gameObject.SetActive(true);
       pauseBottom.gameObject.SetActive(true);
       selectArrow.gameObject.SetActive(true);
       showPDA();
    }
    public void hidePDA()
    {
        pda.gameObject.SetActive(false);
    }
    public void showPDA()
    {
        if(SaveData.currentSave.getCVariable("NewPause") == 1) {
            pda.gameObject.SetActive(true);
        }
    }
}
