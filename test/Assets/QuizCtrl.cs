using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
//using UniRx.Async;
using Cysharp.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

public class QuizCtrl : MonoBehaviour
{
    
    Label questLabel;
    [SerializeField] Button btn1;    
    [SerializeField] Button btn2;    
    [SerializeField] Button btn3;    
    [SerializeField] UIDocument ui;

    List<Questions> questList = new List<Questions>();
    
    int currStage;
    int currQuestNo, corrAnswNo;
    CancellationTokenSource cts;

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
        btn1 = ui.rootVisualElement.Q<Button>("btn1");        
        btn2 = ui.rootVisualElement.Q<Button>("btn2");      
        btn3 = ui.rootVisualElement.Q<Button>("btn3");
       
        btn1.clicked += () => CheckQuestion(btn1);
        btn2.clicked += () => CheckQuestion(btn2);
        btn3.clicked += () => CheckQuestion(btn3);
        
    }


    void Start()
    {    
        AddQuestions();
                
        ReadQuestions();

        cts = new CancellationTokenSource();
        Delay10sec();

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
    }

    void CheckQuestion(Button clBtn)  //on click
    {
        cts.Cancel();            

        if (clBtn.name == questList[currQuestNo].Cor)        
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

        Delay10sec();
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


    async UniTask Delay10sec ()
    {
        cts = new CancellationTokenSource();        

        await UniTask.Delay(10000, cancellationToken: cts.Token);   //10sec

        //await Task.Run(() => { }, cancellationToken);
        print("TIME OUT");
        currQuestNo++;
        if (currQuestNo < 10)  //quest
        {
            ReadQuestions();
            Delay10sec();
        }
        else
        {
            ShowResult();
        }

    }

}
