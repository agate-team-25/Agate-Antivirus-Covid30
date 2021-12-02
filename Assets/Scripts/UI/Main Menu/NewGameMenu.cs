using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewGameMenu : MonoBehaviour
{

    #region variables
    // Button level yang ada
    [SerializeField] private Button level1Button;
    [SerializeField] private Button level2Button;
    [SerializeField] private Button level3Button;

    // Menyimpan image text dari nama setiap level
    [SerializeField] private GameObject level1Text;
    [SerializeField] private GameObject level2Text;
    [SerializeField] private GameObject level3Text;

    // Menyimpan image text status completion dari setiap level
    [SerializeField] private GameObject level1Complete;
    [SerializeField] private GameObject level2Complete;
    [SerializeField] private GameObject level3Complete;

    // Menyimpan image untuk button level 2 dst jika level ke unlock
    [SerializeField] private Sprite level2Unlocked;
    [SerializeField] private Sprite level3Unlocked;

    // Color untuk active/inactive button
    private Color32 activeColor = new Color32(255, 255, 225, 225);
    private Color32 inactiveColor = new Color32(255, 255, 225, 100);
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // Asumsi level 1 ada pada scene ke-1, level 2 pada scene ke-2, dst.

        // ambil level progress dari saved data
        float levels = UserDataManager.Progress.levelProgress;

        level1Text.SetActive(true);

        // listener untuk button ke level 1
        level1Button.onClick.AddListener(() =>
        {
            //Debug.Log("Level 1 clicked");

            SceneManager.LoadScene(1);
        });

        // jika level 1 sudah dimenangkan
        if (levels >= 1)
        {
            level1Complete.SetActive(true);
            level2Text.SetActive(true);

            // listener untuk button ke level 2
            level2Button.onClick.AddListener(() =>
            {
                //Debug.Log("Level 2 clicked");

                //SceneManager.LoadScene(2);
            });

            // dan ubah image tombol level 2
            level2Button.image.sprite = level2Unlocked;
        }

        // jika level 2 sudah dimenangkan
        if (levels >= 2)
        {
            level2Complete.SetActive(true);
            level3Text.SetActive(true);

            // listener untuk button ke level 3
            level3Button.onClick.AddListener(() =>
            {
                //Debug.Log("Level 3 clicked");

                //SceneManager.LoadScene(3);
            });

            // dan ubah image tombol level 3
            level3Button.image.sprite = level3Unlocked;
        }

        // jika level 3 sudah dimenangkan
        if (levels >= 3)
        {
            level3Complete.SetActive(true);
        }

    }

    // Update is called once per frame
    //void Update()
    //{}
}
