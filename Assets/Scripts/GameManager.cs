using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using YG;

public class GameManager : MonoBehaviour
{
    public int[] etalonNumber;
    private int[] itemNumber;

    private int cow, bull;
    private int tryItem;
    private int positionNumber;

    private int hint;

    [SerializeField] Image[] TrueNumberImages;
    [SerializeField] Image[] InputNumberImages;
    [SerializeField] Image[] HintNumberImages;

    [SerializeField] GameObject GeneratedNumber, TrueNumber;

    [SerializeField] GameObject ContentView;

    [SerializeField] GameObject LogItemPrefab;

    [SerializeField] GameObject MenuPanel, WinPanel, GamePanel;

    [SerializeField] ScrollRect scrollRect;

    private Sprite[] NumbersAll;
    private Sprite emptyCell;

    YandexGame YG;

    void Start()
    {
        YG = GameObject.Find("YandexGame").GetComponent<YandexGame>();
        NumbersAll = Resources.LoadAll<Sprite>("Numbers");
        emptyCell = Resources.Load<Sprite>("EmptyCell");
        NewMove();
    }

    public void StartGameButton()
    {

        GenerateNumber();

        if (etalonNumber.Length > 0)
        {
            MenuPanel.SetActive(false);
            TrueNumber.SetActive(false);
            WinPanel.SetActive(false);

            GamePanel.SetActive(true);
        }
        GenerateTrueNumber();

        foreach (var item in ContentView.GetComponentsInChildren<RectTransform>().Where(n => n.name == LogItemPrefab.name))
        { 
            Destroy(item.gameObject);   
        }
        tryItem = 0;
    }

    void GenerateNumber()
    {
        int i = 0;
        etalonNumber = new int[4] { 100, 100, 100, 100 };
        while (i < etalonNumber.Length)
        {
            int random = UnityEngine.Random.Range(0, 10);
            if (!etalonNumber.Contains(random))
            {
                etalonNumber[i] = random;
                i++;
            }
            else
            {
                continue;
            }
        }
    }
    void GenerateTrueNumber()
    {
        for (int i = 0; i < etalonNumber.Length; i++)
        {
            string numberString = "Numbers_" + etalonNumber[i];
            TrueNumberImages[i].sprite = NumbersAll.Where(n => n.name == numberString).First();
        }
    }

    void WinScreen()
    {
        if (bull == 4)
        {  
            WinPanel.SetActive(true);
            TrueNumber.SetActive(true);

            GamePanel.SetActive(false);

            WinPanel.GetComponentsInChildren<TMP_Text>().Where(n => n.name == "Score").First().text = tryItem.ToString();
        }
    }

    public void NumButtonClick(int number)
    {
        if (positionNumber < 4 && !itemNumber.Contains(number))
        {
            string numberString = "Numbers_" + number;
            InputNumberImages[positionNumber].sprite = NumbersAll.Where(n => n.name == numberString).First();
            itemNumber[positionNumber] = number;
            positionNumber++;
        }
    }

    public void Again()
    {
        YG._FullscreenShow();
        StartGameButton();
    }

    public void NewMove()
    {
        cow = 0;
        bull = 0;
        itemNumber = new int[4] { 100, 100, 100, 100 };
        positionNumber = 0;

        for (int i = 0; i < InputNumberImages.Length; i++)
        {
            InputNumberImages[i].sprite = emptyCell;
        }
    }

    public void BackMove()
    {
        if (positionNumber > 0)
        {
            positionNumber--;
            InputNumberImages[positionNumber].sprite = emptyCell;
            itemNumber[positionNumber] = 100;
        }
    }

    public void CheckMove()
    {
        if (positionNumber == 4)
        {
            for (int i = 0; i < itemNumber.Length; i++)
            {
                if (itemNumber[i] == etalonNumber[i])
                {
                    bull++;
                }
                else if (etalonNumber.Contains(itemNumber[i]))
                {
                    cow++;
                }
            }
            CreateLogItem();

            WinScreen();
            NewMove();
        }
    }

    void CreateLogItem()
    {
        tryItem++;
        int finalScore = 0;
        for (int i = 0; i < itemNumber.Length; i++)
        {
            finalScore += itemNumber[i] * Convert.ToInt32(Math.Pow(10, itemNumber.Length - i - 1));
        }

        GameObject newItem = Instantiate(LogItemPrefab);
        newItem.transform.SetParent(ContentView.transform);
        newItem.name = LogItemPrefab.name;
        newItem.transform.localScale = Vector3.one;

        TMP_Text[] texts = newItem.GetComponentsInChildren<TMP_Text>();
        foreach ( TMP_Text text in texts ) 
        {
            switch (text.name)
            {
                case ("ItemNumber"):
                    text.text = tryItem.ToString();
                    break;
                case ("Cows"):
                    text.text = cow.ToString();
                    break;
                case ("Bulls"):
                    text.text = bull.ToString();
                    break;
                case ("Number"):
                    text.text = finalScore.ToString("D4");
                    break;
            }
        }
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0;
        Canvas.ForceUpdateCanvases();
    }

    public void AddHint()
    {
        if (hint < 4)
        {

            string numberString = "Numbers_" + etalonNumber[hint];
            Debug.Log(numberString);
            HintNumberImages[hint].sprite = NumbersAll.Where(n => n.name == numberString).First();
            hint++;
        }
    }
}
