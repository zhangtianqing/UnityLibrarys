using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;

namespace Assets.Script.Tools
{
    public class FaceVerify : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

    class Baidu {
        // 调用getAccessToken()获取的 access_token建议根据expires_in 时间 设置缓存
        // 返回token示例
        public static string TOKEN = "24.adda70c11b9786206253ddb70affdc46.2592000.1493524354.282335-1234567";

        // 百度云中开通对应服务应用的 API Key 建议开通应用的时候多选服务
        private static string clientId = "l0wrXs9YQescNbs6t0Cym6mG";
        // 百度云中开通对应服务应用的 Secret Key
        private static string clientSecret = "iOnZc7z8VsO7sGHTra4vhdp3WDapEoGp";
        static void Main(string[] args)
        {
            // 获取 AssetToken
            getAccessToken();
        }
        public static string getAccessToken()
        {
            string authHost = "https://aip.baidubce.com/oauth/2.0/token";
            HttpClient client = new HttpClient();
            List<KeyValuePair<string, string>> paraList = new List<KeyValuePair<string, string>>();
            paraList.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            paraList.Add(new KeyValuePair<string, string>("client_id", clientId));
            paraList.Add(new KeyValuePair<string, string>("client_secret", clientSecret));

            HttpResponseMessage response = client.PostAsync(authHost, new FormUrlEncodedContent(paraList)).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            Debug.Log(result);
            return result;
        }

    }
}