using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
	[Header("Person")]
	public Person person;
	public Text money;

	[Header("Gameplay UI")]
	public Slider waterSlider;
	public Slider foodSlider;
	public Image gamePanel;
	public GameObject lifePanel;
	public bool ok;
	public Text[] passport;
	public Text[] pass;
	public Text timeTxt;
    public Text dayTxt;
	public Text kek;
	public GameObject attentionPanel;
    [Header("Buttons")]
    public GameObject restartBtn;
	public Button cancelBtn;
	public Button enterBtn;
	public Button nextBtn;
	Data data = new Data();
	
	public int time = 8;
	private float tempTime = 0;
	private bool isDay = true;
	
	void Start () 
	{
		Clear();
        dayTxt.text = "День: " + person.day.ToString();
        lifePanel.SetActive(false);
    }

    private void Update()
	{	
        if (person.money < 0)
        {
            money.text = "$БАНКРОТ";
            GameOver();
        }
        else
        {
            money.text = "$ " + person.money.ToString();
        }

		if (isDay)
		{
			tempTime += Time.deltaTime;
			if ((int)tempTime == 6)
			{
				foodSlider.value -= person.eat;
				waterSlider.value -= person.water;
				time++;
				tempTime = 0;
			}
			timeTxt.text = time.ToString() + ":00";
			if (time == 18)
			{
				lifePanel.SetActive(true);
				time = 0;
				isDay = false;
				Attention("Рабочий день закончен!");
				Clear();
				enterBtn.interactable = false;
				cancelBtn.interactable = false;
				nextBtn.interactable = false;
			}
		}
	}

	public void NextClick()
	{
		enterBtn.interactable = true;
		cancelBtn.interactable = true;
		nextBtn.interactable = false;
		ok = Random.Range(0, 3) != 2;
		DoPassport();	
		DoPass();
	}

	public void EnterClick()
	{
		Clear();
		enterBtn.interactable = false;
		cancelBtn.interactable = false;
		nextBtn.interactable = true;
		if (!ok)
		{
			Attention("Вы пропустили гражданина с поддельными документами!\nШтраф " + person.penalty.ToString() + "$!");
			person.money -= person.penalty;
		}
		else
		{
			person.money += person.payment;
		}
	}

	public void CancelClick()
	{
		Clear();
		enterBtn.interactable = false;
		cancelBtn.interactable = false;
		nextBtn.interactable = true;
		if (ok)
		{
			person.money -= person.penalty;
			Attention("Вы не пропустили гражданина с корректными документами! Штраф " + person.penalty.ToString() + "$!");
		}
		else
		{
			person.money += person.payment;
		}
	}

	public void NewDayClick()
	{
		if (!Die())
		{
			isDay = true;
			enterBtn.interactable = false;
			cancelBtn.interactable = false;
			nextBtn.interactable = true;
			time = 8;
			tempTime = 0;
			lifePanel.SetActive(false);
            person.day++;
            dayTxt.text = "День: " + person.day.ToString();
		}
	}

	private void DoPassport()
	{
		passport[0].text = data.manNames[Random.Range(0, data.manNames.Length)];
		passport[1].text = data.manSurnames[Random.Range(0, data.manSurnames.Length)];
		passport[1].fontSize = passport[1].text.Length > 8 ? 17 : 33;
		passport[2].text = data.cities[Random.Range(0, data.cities.Length)];
		passport[3].text = data.series[Random.Range(0, data.series.Length)];
		passport[4].text = data.storages[Random.Range(0, data.storages.Length)];
	}

	private void DoPass()
	{
		pass[0].text = passport[0].text;
		pass[1].text = passport[1].text;
		pass[1].fontSize = passport[1].text.Length > 8 ? 17 : 33;
		pass[2].text = passport[2].text;
		pass[3].text = passport[3].text;
		pass[4].text = passport[4].text;
		
		if (!ok)
		{
			int rnd = Random.Range(0, 5);
			string temp;
			switch (rnd)
			{
				case 0:
					temp = passport[0].text;
					pass[0].text = temp.Insert(Random.Range(1, temp.Length), "и");
					break;
				case 1:
					temp = passport[1].text;
					pass[1].text = temp.Remove(Random.Range(1, temp.Length), 1);
					break;
				case 2:
					do
					{
						pass[2].text = data.cities[Random.Range(0, data.cities.Length)];
					} while (pass[2].text == passport[2].text);
					break;
				case 3:
					temp = passport[3].text;
					pass[3].text = temp.Insert(Random.Range(0, temp.Length), Random.Range(0, 9).ToString());
					break;
				case 4:
                    do
                    {
                        temp = passport[4].text;
                        temp = temp.Insert(2, Random.Range(1, 10).ToString());
                        temp = temp.Insert(3, Random.Range(1, 10).ToString());
                        pass[4].text = temp.Remove(4, 2);

                    } while (pass[4].text == passport[4].text);
                    
					break;
					
			}
		}

		kek.text = ok.ToString();
	}

	private void Clear()
	{
		for (int i = 0; i < passport.Length; i++)
		{
			passport[i].text = "";
		}
		
		for (int i = 0; i < pass.Length; i++)
		{
			pass[i].text = "";
		}
	}

	private void Attention(string message, string upInfo = "Внимание!", int time = 5)
	{
		attentionPanel.SetActive(true);
		Text[] t = attentionPanel.GetComponentsInChildren<Text>();
		t[0].text = upInfo;
		t[1].text = message;
		StartCoroutine(Pause(time));
	}

	private bool Die()
	{
		bool result = false;

		if (foodSlider.value > 0)
		{
			result = false;
		}
		else
		{
			result = true;
		}
		return result;
	}

	public void EatButton()
	{
		if (person.money >= person.eatCost && foodSlider.value < 100)
		{
			person.money -= person.eatCost;
			foodSlider.value += person.eat * 2;
		}
	}

	public void WaterButton()
	{
		if (person.money >= person.waterCost && waterSlider.value < 100)
		{
			person.money -= person.waterCost;
			waterSlider.value += person.water * 2;
		}
	}

    public void RestartClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GameOver()
    {
        lifePanel.SetActive(false);
        time = 0;
        isDay = false;
        Attention("Вы проиграли!", time : 20);
        Clear();
        enterBtn.interactable = false;
        cancelBtn.interactable = false;
        nextBtn.interactable = false;
        restartBtn.SetActive(true);

    }
    IEnumerator Pause(int time = 5)
	{
		yield return new WaitForSeconds(time);
		attentionPanel.SetActive(false);
	}

    
	
}
