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
    /// ����б�
    /// </summary>
    internal List<string> contentList  = new List<string>();
    /// <summary>
    /// ��Ŀ�����б�
    /// </summary>
    internal List<string> testType = new List<string>();
    /// <summary>
    /// ѡ���б�
    /// </summary>
    internal List<string> OptionsList = new List<string>();
    /// <summary>
    /// ���б�
    /// </summary>
    internal List<string> answerList = new List<string>();

    /// <summary>
    /// �ĸ�ѡ��
    /// </summary>
    public List<Toggle> toggleGroup = new List<Toggle>();
    /// <summary>
    /// �÷�
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
        toggleGroup[0].GetComponentInChildren<Text>().text = OptionsList[real_index[rank]].Split('��')[0];
        toggleGroup[1].GetComponentInChildren<Text>().text = OptionsList[real_index[rank]].Split('��')[1];
        toggleGroup[2].GetComponentInChildren<Text>().text = OptionsList[real_index[rank]].Split('��')[2];
        toggleGroup[3].GetComponentInChildren<Text>().text = OptionsList[real_index[rank]].Split('��')[3];
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
        // �������ȫ����ȡ��result��(���룺DataSet��
        DataSet result = excelDataReader.AsDataSet();
        // ��ȡ����ж����� 
        int columns = result.Tables[0].Columns.Count;
        // ��ȡ����ж����� 
        int rows = result.Tables[0].Rows.Count;
        // �����������δ�ӡ����е�ÿ������ 

        List<string> excelDta = new List<string>();
        //��һ��Ϊ��ͷ������ȡ
        for (int i = 1; i < rows; i++)
        {
            value = null;
            all = null;
            for (int j = 0; j < columns; j++)
            {
                
                // ��ȡ�����ָ����ָ���е����� 
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
    /// ��ʾ�÷ֽ���
    /// </summary>
    public void ShowScoreView()
    {
        //UIManager.Instance.SwitchDialogUI<ScoreViewDialog>(UIDefine.UIID.ScoreViewDialog);
        scoreViewContainer.GetComponent<ScoreViewItem>().ShowScore(score);
        scoreViewContainer.SetActive(true);
        
    }
    //�л���һ�⣬��ɺ���ʾ����
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
        toggleGroup[0].GetComponentInChildren<Text>().text = OptionsList[real_index[rank]].Split('��')[0];
        toggleGroup[1].GetComponentInChildren<Text>().text = OptionsList[real_index[rank]].Split('��')[1];
        toggleGroup[2].GetComponentInChildren<Text>().text = OptionsList[real_index[rank]].Split('��')[2];
        toggleGroup[3].GetComponentInChildren<Text>().text = OptionsList[real_index[rank]].Split('��')[3];
    }
    public void IsRight()
    {
        int isRight = 0;

        string currentAns = answerList[real_index[rank]];
        Debug.Log(currentAns);
        //char[] strCharArr = str.ToCharArray();


        foreach (var item in toggleGroup)
        {


            if (item.isOn == false)//����û�ѡ�У����жϴ����Ƿ��д���
            {
                //�ж������Ƿ������ѡ�û�о�ֱ��return score���о���ȷ������+1
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
            score += 3;//�����Ե���ȫ������
        }
        else if (isRight == currentAns.Length)
        {
            score += 5;//ȫ�Ե�5��
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
    /// �����ȡx��
    /// </summary>
    /// <param name="total">����</param>
    /// <param name="count">��ȡ�ĸ���</param>
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
            //���һ������ÿ���һ�Σ��������-1
            int num = Random.Range(0, end + 1);
            output[i] = sequence[num];
            //���������һ������ֵ��ȡ��������
            sequence[num] = sequence[end];
            end--;
        }
        return output;
    }

}
