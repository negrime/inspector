using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
	public Text kek;
	public GameObject attentionPanel;
	[Header("Buttons")] 
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
	}

	private void Update()
	{	
		money.text = "$ " + person.money.ToString();
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
			Attention("Вы пропустили гражданина с поддельными документами! Штраф " + person.penalty.ToString() + "$!");
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
					pass[4].text = "HUY";
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

	private void Attention(string message, string upInfo = "Внимание!")
	{
		attentionPanel.SetActive(true);
		Text[] t = attentionPanel.GetComponentsInChildren<Text>();
		t[0].text = upInfo;
		t[1].text = message;
		StartCoroutine(Pause());
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
		if (person.money >= person.eatCost)
		{
			person.money -= person.eatCost;
			foodSlider.value += person.eat * 2;
		}
	}

	public void WaterButton()
	{
		if (person.money >= person.waterCost)
		{
			person.money -= person.waterCost;
			waterSlider.value += person.water * 2;
		}
	}
	IEnumerator Pause()
	{
		Debug.Log("Pause");
		yield return new WaitForSeconds(5);
		attentionPanel.SetActive(false);
		Debug.Log("UNPause");
	}
	
}
