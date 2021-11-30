using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    #region variables
    // Title menu
    [SerializeField] private Image title;

    // Object yang menyimpan menu page tempat tombol-tombol menu dari game
    [SerializeField] GameObject menuPage;

    // Button utama yang ada pada main menu
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button exitButton;

    // Menyimpan gameObject untuk menu new game dan setting
    [SerializeField] private GameObject newGameMenu;
    [SerializeField] private GameObject settingMenu;

    // Variable untuk menyimpan status apakah new game atau setting button sedang di select
    [SerializeField] private bool newGameSelected;
    [SerializeField] private bool settingSelected;

    // Color untuk active/inactive button
    private Color32 activeColor = new Color32(255, 255, 225, 225);
    private Color32 inactiveColor = new Color32(255, 255, 225, 100);

    #endregion

    #region singleton
    // Untuk menjadikan object singleton
    private static MainMenu _instance = null;
    public static MainMenu Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MainMenu>();
            }

            return _instance;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // Memastikan keduanya tidak di select sekaligus
        if (newGameSelected)
        {
            settingSelected = false;
        }
        checkMenuDisplay();



        newGameButton.onClick.AddListener(() =>
        {
            //Debug.Log("New game selected");

            newGameSelected = true;
            settingSelected = false;
            checkMenuDisplay();
        });

        settingButton.onClick.AddListener(() =>
        {
            //Debug.Log("Setting selected");

            newGameSelected = false;
            settingSelected = true;
            checkMenuDisplay();
        });
    }

    // Update is called once per frame
    //void Update()
    //{}

    // method untuk membuka menu dari startscreen
    public void OpenMenu()
    {
        //Debug.Log("Open Menu");

        // tampilkan menu utama yang tersembunyi pada start screen
        menuPage.SetActive(true);

        // ubah posisi title menjadi ke pinggir
        title.transform.localPosition = new Vector2(-164, 202);

    }

    private void checkMenuDisplay()
    {
        // set tombol mana yang lebih jelas berdasarkan menu yang aktif
        newGameButton.image.color = (newGameSelected ? activeColor : inactiveColor);
        settingButton.image.color = (settingSelected ? activeColor : inactiveColor);

        // set menu mana yang akan ditampilkan
        newGameMenu.SetActive(newGameSelected);
        settingMenu.SetActive(settingSelected);
    }
}
