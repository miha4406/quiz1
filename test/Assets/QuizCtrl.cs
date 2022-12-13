using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class QuizCtrl : MonoBehaviour
{
    [SerializeField] Text questText;
    [SerializeField] Button btn1;
    [SerializeField] Text varText1;
    [SerializeField] Button btn2;
    [SerializeField] Text varText2;
    [SerializeField] Button btn3;
    [SerializeField] Text varText3;

    List<Questions> questList = new List<Questions>();

    int currStage;
    int currQuestNo, corrAnswNo;


    public struct Questions   
    {
        string question;
        string var1, var2, var3;
        string cor;

        public Questions(string question, string var1, string var2, string var3, string cor) : this()
        {
            this.Question = question;
            this.Var1 = var1;
            this.Var2 = var2;
            this.Var3 = var3;
            this.Cor = cor;
        }

        public string Question { get; private set; }
        public string Var1 {get; private set;}
        public string Var2 { get; private set; }
        public string Var3 { get; private set; }
        public string Cor { get; private set; }

    }

    void Awake()
    {
        currStage = SceneManager.GetActiveScene().buildIndex;
        currQuestNo = 0;
        corrAnswNo = 0;

        btn1.onClick.AddListener(() => CheckQuestion());
        btn2.onClick.AddListener(() => CheckQuestion());
        btn3.onClick.AddListener(() => CheckQuestion());
    }


    void Start()
    {
        AddQuestions();
                
        ReadQuestions();       

    }


    void AddQuestions()  //10
    {
        if (currStage == 1)
        {
            questList.Add(new Questions("quest1-1", "answ1", "answ2", "answ3", "btn1"));
            questList.Add(new Questions("quest1-2", "answ1", "answ2", "answ3", "btn2"));
            questList.Add(new Questions("quest1-3", "answ1", "answ2", "answ3", "btn3"));
            questList.Add(new Questions("quest1-4", "answ1", "answ2", "answ3", "btn1"));
            questList.Add(new Questions("quest1-5", "answ1", "answ2", "answ3", "btn2"));
            questList.Add(new Questions("quest1-6", "answ1", "answ2", "answ3", "btn3"));
            questList.Add(new Questions("quest1-7", "answ1", "answ2", "answ3", "btn1"));
            questList.Add(new Questions("quest1-8", "answ1", "answ2", "answ3", "btn2"));
            questList.Add(new Questions("quest1-9", "answ1", "answ2", "answ3", "btn3"));
            questList.Add(new Questions("quest1-10", "answ1", "answ2", "answ3", "btn1"));
        }
        else if (currStage == 2)
        {
            questList.Add(new Questions("quest2-1", "answ1", "answ2", "answ3", "btn1"));
            questList.Add(new Questions("quest2-2", "answ1", "answ2", "answ3", "btn2"));
            questList.Add(new Questions("quest2-3", "answ1", "answ2", "answ3", "btn3"));
            questList.Add(new Questions("quest2-4", "answ1", "answ2", "answ3", "btn1"));
            questList.Add(new Questions("quest2-5", "answ1", "answ2", "answ3", "btn2"));
            questList.Add(new Questions("quest2-6", "answ1", "answ2", "answ3", "btn3"));
            questList.Add(new Questions("quest2-7", "answ1", "answ2", "answ3", "btn1"));
            questList.Add(new Questions("quest2-8", "answ1", "answ2", "answ3", "btn2"));
            questList.Add(new Questions("quest2-9", "answ1", "answ2", "answ3", "btn3"));
            questList.Add(new Questions("quest2-10", "answ1", "answ2", "answ3", "btn1"));
        }
        else if (currStage == 3)
        {
            questList.Add(new Questions("quest3-1", "answ1", "answ2", "answ3", "btn1"));
            questList.Add(new Questions("quest3-2", "answ1", "answ2", "answ3", "btn2"));
            questList.Add(new Questions("quest3-3", "answ1", "answ2", "answ3", "btn3"));
            questList.Add(new Questions("quest3-4", "answ1", "answ2", "answ3", "btn1"));
            questList.Add(new Questions("quest3-5", "answ1", "answ2", "answ3", "btn2"));
            questList.Add(new Questions("quest3-6", "answ1", "answ2", "answ3", "btn3"));
            questList.Add(new Questions("quest3-7", "answ1", "answ2", "answ3", "btn1"));
            questList.Add(new Questions("quest3-8", "answ1", "answ2", "answ3", "btn2"));
            questList.Add(new Questions("quest3-9", "answ1", "answ2", "answ3", "btn3"));
            questList.Add(new Questions("quest3-10", "answ1", "answ2", "answ3", "btn1"));
        }
        else { print(currStage); }
    }


    void ReadQuestions()
    {        
        questText.text = questList[currQuestNo].Question;
        varText1.text = questList[currQuestNo].Var1;
        varText2.text = questList[currQuestNo].Var2;
        varText3.text = questList[currQuestNo].Var3;
    }

    void CheckQuestion()
    {
        GameObject btn = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        
        if(btn.name == questList[currQuestNo].Cor)
        {
            print("Correct");
            corrAnswNo++;
        }
        else { print("Wrong"); }

        currQuestNo++;

        if (currQuestNo < 10)  //quest
        {
            ReadQuestions();
        }
        else {
            ShowResult();
        }
    }

    void ShowResult()
    {
        float rate = (float)corrAnswNo / 10;  //quest

        print("RATE=" + rate);

        if ( rate >= 0.5f)  //test 50%
        {
            print("PASS");
            if (currStage<3)
            {
                SceneManager.LoadScene(currStage+1);
            }
            else { SceneManager.LoadScene(0); }
            
        }
        else
        {
            print("LOSE");
            SceneManager.LoadScene(currStage);
        }
    }

}
