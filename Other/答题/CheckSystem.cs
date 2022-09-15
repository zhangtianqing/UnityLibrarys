using Excel;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CheckSystem : BaseUI
{


    [SerializeField]
    internal Text text;
    [SerializeField]
    internal GameObject scoreViewContainer;
    int[] real_index;
   
    string value;
    string all;
    /// <summary>
    /// 题干列表
    /// </summary>
    internal List<string> contentList  = new List<string>();
    /// <summary>
    /// 题目类型列表
    /// </summary>
    internal List<string> testType = new List<string>();
    /// <summary>
    /// 选项列表
    /// </summary>
    internal List<string> OptionsList = new List<string>();
    /// <summary>
    /// 答案列表
    /// </summary>
    internal List<string> answerList = new List<string>();

    /// <summary>
    /// 四个选项
    /// </summary>
    public List<Toggle> toggleGroup = new List<Toggle>();
    /// <summary>
    /// 得分
    /// </summary>
    public int score = 0;

    private void Start()
    {
        LoadData();

        real_index = GetRandomSequence(contentList.Count, 10);
        Init();


    }

    private void Init()
    {
        score = 0;
        toggleGroup[0].isOn = true;
        toggleGroup[1].isOn = true;
        toggleGroup[2].isOn = true;
        toggleGroup[3].isOn = true;
        text.text = contentList[real_index[rank]];
        toggleGroup[0].GetComponentInChildren<Text>().text = OptionsList[real_index[rank]].Split('；')[0];
        toggleGroup[1].GetComponentInChildren<Text>().text = OptionsList[real_index[rank]].Split('；')[1];
        toggleGroup[2].GetComponentInChildren<Text>().text = OptionsList[real_index[rank]].Split('；')[2];
        toggleGroup[3].GetComponentInChildren<Text>().text = OptionsList[real_index[rank]].Split('；')[3];
    }

    private void Update()
    {
    }
    public void LoadData()
    {
        contentList.Clear();
        testType.Clear();
        OptionsList.Clear();
        answerList.Clear();
        FileStream fileStream = File.Open(Application.streamingAssetsPath + "/paper.xlsx", FileMode.Open, FileAccess.Read);
        IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
        // 表格数据全部读取到result里(引入：DataSet（
        DataSet result = excelDataReader.AsDataSet();
        // 获取表格有多少列 
        int columns = result.Tables[0].Columns.Count;
        // 获取表格有多少行 
        int rows = result.Tables[0].Rows.Count;
        // 根据行列依次打印表格中的每个数据 

        List<string> excelDta = new List<string>();
        //第一行为表头，不读取
        for (int i = 1; i < rows; i++)
        {
            value = null;
            all = null;
            for (int j = 0; j < columns; j++)
            {
                
                // 获取表格中指定行指定列的数据 
                value = result.Tables[0].Rows[i][j].ToString();
                if (value == "")
                {
                    continue;
                }
                switch (j)
                {
                    case 0: contentList.Add(value); break;
                    case 1: testType.Add(value); break;
                    case 2: OptionsList.Add(value); break;
                    case 3: answerList.Add(value); break;
                }
                //Debug.Log(value);
                all = all + value + "|";
            }
            if (all != null)
            {
                excelDta.Add(all);
            }
        }

    }
    public int rank = 0;

    /// <summary>
    /// 显示得分界面
    /// </summary>
    public void ShowScoreView()
    {
        //UIManager.Instance.SwitchDialogUI<ScoreViewDialog>(UIDefine.UIID.ScoreViewDialog);
        scoreViewContainer.GetComponent<ScoreViewItem>().ShowScore(score);
        scoreViewContainer.SetActive(true);
        
    }
    //切换下一题，完成后显示分数
    public void SwitchQuestion()
    {
        IsRight();

        toggleGroup[0].isOn = true;
        toggleGroup[1].isOn = true;
        toggleGroup[2].isOn = true;
        toggleGroup[3].isOn = true;
        if(rank > 9)
        {

            ShowScoreView();
            return;
        }
        text.text = contentList[ real_index[rank]];
        toggleGroup[0].GetComponentInChildren<Text>().text = OptionsList[real_index[rank]].Split('；')[0];
        toggleGroup[1].GetComponentInChildren<Text>().text = OptionsList[real_index[rank]].Split('；')[1];
        toggleGroup[2].GetComponentInChildren<Text>().text = OptionsList[real_index[rank]].Split('；')[2];
        toggleGroup[3].GetComponentInChildren<Text>().text = OptionsList[real_index[rank]].Split('；')[3];
    }
    public void IsRight()
    {
        int isRight = 0;

        string currentAns = answerList[real_index[rank]];
        Debug.Log(currentAns);
        //char[] strCharArr = str.ToCharArray();


        foreach (var item in toggleGroup)
        {


            if (item.isOn == false)//此项被用户选中，再判断答案中是否有此项
            {
                //判定答案中是否有这个选项，没有就直接return score，有就正确答案数量+1
                if (!currentAns.Contains(item.name))
                {
                    return;
                }
                else
                {
                    isRight++;
                }
            }
        }
        if (isRight > 0 && isRight < currentAns.Length)
        {
            score += 3;//假设答对但不全得三分
        }
        else if (isRight == currentAns.Length)
        {
            score += 5;//全对得5分
        }
        Debug.LogError(" -------" + score);
        rank += 1;
    }


    protected override void OnEnterAnimationFinish()
    {
        int i = 0;
    }

    protected override void OnQuiteAnimationFinish()
    {
        int i = 0;
    }

    /// <summary>
    /// 随机抽取x个
    /// </summary>
    /// <param name="total">总数</param>
    /// <param name="count">抽取的个数</param>
    /// <returns></returns>
    int[] GetRandomSequence(int total, int count)
    {
        int[] sequence = new int[total];
        int[] output = new int[count];

        for (int i = 0; i < total; i++)
        {
            sequence[i] = i;
        }
        int end = total - 1;
        for (int i = 0; i < count; i++)
        {
            //随机一个数，每随机一次，随机区间-1
            int num = Random.Range(0, end + 1);
            output[i] = sequence[num];
            //将区间最后一个数赋值到取到的数上
            sequence[num] = sequence[end];
            end--;
        }
        return output;
    }

}
