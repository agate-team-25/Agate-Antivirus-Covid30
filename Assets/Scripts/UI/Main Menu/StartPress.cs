using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StartPress : MonoBehaviour, IPointerDownHandler
{
    // set blinking time
    [SerializeField] private float timeActive;
    [SerializeField] private float timeInactive;
    [SerializeField] private Image startText;

    private float counter;

    // Start is called before the first frame update
    void Start()
    {
        counter = timeActive;
        startText.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Hanya untuk animasi blinking text
        counter -= Time.deltaTime;
        if (counter <= 0)
        {
            startText.enabled = !startText.enabled;
            
            if (startText.enabled == true)
            {
                counter = timeActive;
            }
            else
            {
                counter = timeInactive;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("Start button pressed");

        // Play button clicked sfx here
        // --BUTTON CLICKES SFX--

        // panggil untuk membuka main menu
        MainMenu.Instance.OpenMenu();

        // hilangkan tombol start
        gameObject.SetActive(false);

    }
}
