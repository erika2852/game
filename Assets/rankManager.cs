using UnityEngine;
using UnityEngine.UI;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using System.Collections.Generic;

public class rankManager : MonoBehaviour
{
    DynamoDBContext context;
    AmazonDynamoDBClient DBclient;
    CognitoAWSCredentials credentials;

    public Text rank1Text;
    public Text rank2Text;
    public Text rank3Text;
    public Text rank4Text;
    public Text rank5Text;

    private List<Character> characterList = new List<Character>();

    private void Awake()
    {
        UnityInitializer.AttachToGameObject(this.gameObject);
        credentials = new CognitoAWSCredentials("ap-northeast-2:b3f1d2ce-0fd8-421e-b730-4d5eaa16ea05", RegionEndpoint.APNortheast2);
        DBclient = new AmazonDynamoDBClient(credentials, RegionEndpoint.APNortheast2);
        context = new DynamoDBContext(DBclient);
    }

    [DynamoDBTable("character_info")]
    public class Character
    {
        [DynamoDBHashKey]
        public string id { get; set; }
        [DynamoDBProperty]
        public int score { get; set; }
    }

    private void Start()
    {
        // 씬이 로드될 때 자동으로 데이터를 가져오고 표시
        FindTopItemsAndStore();
    }

   private void FindTopItemsAndStore()
{
    ScanRequest scanRequest = new ScanRequest
    {
        TableName = "character_info",
        Limit = 5,
    };

    DBclient.ScanAsync(scanRequest, (result) =>
    {
        if (result.Exception != null)
        {
            Debug.LogException(result.Exception);
            return;
        }

        List<Dictionary<string, AttributeValue>> items = result.Response.Items;

        // Sort items based on the "score" attribute in descending order
        items.Sort((a, b) => int.Parse(b["score"].N).CompareTo(int.Parse(a["score"].N)));

        characterList.Clear(); // Clear the list

        foreach (var item in items)
        {
            string id = item["id"].S;
            int score = int.Parse(item["score"].N);

            Character character = new Character
            {
                id = id,
                score = score
            };
            characterList.Add(character);

            Debug.Log($"ID: {id}, Score: {score}");
        }

        Debug.Log("Top 5 items stored in characterList.");

        // Display the top 5 ranks
        DisplayRanks();
    });
}


    private void DisplayRanks()
    {
        // 각 Text UI 요소에 데이터를 표시
        for (int i = 0; i < characterList.Count; i++)
        {
            string text = $"ID: {characterList[i].id}, Score: {characterList[i].score}";
            
            switch (i)
            {
                case 0:
                    rank1Text.text = text;
                    break;
                case 1:
                    rank2Text.text = text;
                    break;
                case 2:
                    rank3Text.text = text;
                    break;
                case 3:
                    rank4Text.text = text;
                    break;
                case 4:
                    rank5Text.text = text;
                    break;
                default:
                    break;
            }
        }
    }
}
