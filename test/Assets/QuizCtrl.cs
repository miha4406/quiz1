using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
//using UniRx.Async;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

public class QuizCtrl : MonoBehaviour
{
    
    Label questLabel;
    Label answerLabel;
    Label resultLabel;
    VisualElement answerPanel;
    VisualElement resultPanel;
    [SerializeField] Button btn1;    
    [SerializeField] Button btn2;    
    [SerializeField] Button btn3;    
    [SerializeField] UIDocument ui;

    float answerDur;

    List<Questions> questList = new List<Questions>();
    
    int currStage;
    int currQuestNo, corrAnswNo;
    CancellationTokenSource cts_;


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
               
        questLabel = ui.rootVisualElement.Q<Label>("questLabel");
        answerLabel = ui.rootVisualElement.Q<Label>("answerLabel");
        resultLabel = ui.rootVisualElement.Q<Label>("resultLabel");
        resultPanel = ui.rootVisualElement.Q<VisualElement>("resultPanel");
        answerPanel = ui.rootVisualElement.Q<VisualElement>("answerPanel");

        btn1 = ui.rootVisualElement.Q<Button>("btn1");        
        btn2 = ui.rootVisualElement.Q<Button>("btn2");      
        btn3 = ui.rootVisualElement.Q<Button>("btn3");       
        btn1.clicked += () => {
            cts_?.Cancel();
            CheckQuestion(btn1);
        } ;
        btn2.clicked += () => {
            cts_?.Cancel();
            CheckQuestion(btn2);
        };
        btn3.clicked += () => {
            cts_?.Cancel();
            CheckQuestion(btn3);
        };

        resultPanel.RegisterCallback<ClickEvent>(LoadNextStage);

    }


    void Start()
    {
        answerDur = 100f;  //answer duration
        answerPanel.visible = false;
        resultPanel.visible = false;       


        AddQuestions();
                
        ReadQuestions();

        cts_ = new CancellationTokenSource();       

    }


    void Update()
    {
        if (answerPanel.visible)
        {
            answerPanel.style.opacity = answerDur;
            answerDur -= 0.5f;

            if (answerPanel.style.opacity.value < 0f)
            {
                answerPanel.visible = false;
                answerDur = 100f;
            }
        }
        
    }



    void AddQuestions()  //10
    {
        if (currStage == 1)
        {
            foreach(string s in MotoData.Json1)
            {                
                questList.Add(JsonConvert.DeserializeObject<Questions>(s));
            }
        }
        else if (currStage == 2)
        {
            foreach (string s in MotoData.Json2)
            {
                questList.Add(JsonConvert.DeserializeObject<Questions>(s));
            }
        }
        else if (currStage == 3)
        {
            foreach (string s in MotoData.Json3)
            {
                questList.Add(JsonConvert.DeserializeObject<Questions>(s));
            }
        }
        else { print(currStage); }
    }


    void ReadQuestions()
    {
        questLabel.text = questList[currQuestNo].Question;
        btn1.text = questList[currQuestNo].Var1;
        btn2.text = questList[currQuestNo].Var2;
        btn3.text = questList[currQuestNo].Var3;

        cts_?.Cancel();
        Delay10sec();
    }


    void CheckQuestion(Button clBtn)  //on click
    {
        //cts_.Cancel();

        answerPanel.visible = true;
        if (clBtn.name == questList[currQuestNo].Cor)        
        {
            print("Correct");
            answerLabel.text = "正解";
            answerLabel.style.color = Color.green;

            corrAnswNo++;
        }
        else {
            print("Wrong");
            answerLabel.text = "不正解";
            answerLabel.style.color = Color.red;
        }

        currQuestNo++;

        if (currQuestNo < 10)  //quest
        {
            ReadQuestions();
        }
        else {
            ShowResult();
        }

        //Delay10sec();
    }



    async UniTask Delay10sec()
    {
        cts_ = new CancellationTokenSource();

        await UniTask.Delay(10000, cancellationToken: cts_.Token);   //10sec
        //await UniTask.Run(() => { }, cancellationToken); ??

        print("TIME OUT");
        answerPanel.visible = true;
        answerLabel.text = "時間切れ";
        answerLabel.style.color = Color.red;

        currQuestNo++;
        if (currQuestNo < 10)  //quest
        {
            ReadQuestions();            
        }
        else
        {
            ShowResult();
        }

    }



    void ShowResult()
    {
        float rate = (float)corrAnswNo / 10;  //quest

        print( String.Format("RATE = {0:P0}", rate) ); 
        resultLabel.text = "クリア率は" + String.Format("{0:P0}", rate);
        resultPanel.visible = true;

        if ( rate >= 0.5f)  //test 50%
        {            
            resultLabel.text += " (WIN)";   
            resultLabel.style.color = Color.green;   
        }
        else
        {            
            resultLabel.text += " (LOSE)";   
            resultLabel.style.color = Color.red;            
        }

    }



    void LoadNextStage(ClickEvent evt)
    {
        //cts_?.Cancel();  //test
        
        if ( resultLabel.text.Contains("WIN") )
        {
            if (currStage < 3)
            {
                SceneManager.LoadScene(currStage+1);
            }
            else { SceneManager.LoadScene(0); }
        }
        else
        {
            SceneManager.LoadScene(currStage);
        }
    }

}
