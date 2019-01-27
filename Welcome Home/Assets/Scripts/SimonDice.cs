using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimonDice : MonoBehaviour
{
    System.Random rand;
    List<SimonButton> buttons;
    List<SimonButton> buttonsToPress;
    WaitForSeconds wait;
    public List<int> activeButtons;
    public float timeBeteweenButtons;
    Door door;
    StartButton startButton;
    public bool firstTime = true;
    int level = 0;
    int Level { get { return level; }
        set { level = value;
            if (level == maxLevel)
            {
                for (int i = 0; i < buttons.Count; i++)
                {
                    buttons[i].Activate(false);
                    buttons[i].anim.SetTrigger(Constantes.SIMON_BUTTON_DEACTIVE);
                }
                door.Open();
            }
            else
            {
                StartCoroutine(GenerateCode());
            }
        }
    }
    int maxLevel;

    public void Start()
    {
        buttons = GetComponentsInChildren<SimonButton>().ToList();
        buttonsToPress = new List<SimonButton>();
        rand = new System.Random();
        maxLevel = activeButtons.Count;
        startButton = GetComponentInChildren<StartButton>();
        startButton.Init(this);
        door = GetComponentInChildren<Door>();
        wait = new WaitForSeconds(timeBeteweenButtons);
    }

    public IEnumerator GenerateCode()
    {
        
            for (int i = 0; i < buttons.Count && !firstTime; i++)
            {
                buttons[i].Activate(false);
            }
            firstTime = false;
            yield return wait;
            for (int i = 0; i < activeButtons[level]; i++)
            {
                //Activar x botones aleatorios
                int x = rand.Next(0,buttons.Count);
                SimonButton button = buttons[x].UseOnce();
                //Guardarlos en activos
                buttonsToPress.Add(button);
                yield return wait;
            }
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Activate(true);
            }

    }

    public void CompareButton(SimonButton button)
    {
        if(buttonsToPress[0] == button)
        {
            buttonsToPress.RemoveAt(0);
            if (buttonsToPress.Count == 0)
                Level++;
        }
        else
        {
            Restart();
        }
    }

    public void Restart()
    {
        for (int i = 0; i < buttons.Count; i++)
        {  
            buttons[i].Init(this);  
        }
        buttonsToPress.Clear();
        Level = 0;
    }

}
