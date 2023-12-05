using UnityEngine;
using UnityEngine.SceneManagement;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using System.Collections.Generic; 

public class StartProgram : MonoBehaviour
{
    DynamoDBContext context;
    AmazonDynamoDBClient DBclient;
    CognitoAWSCredentials credentials;
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
        [DynamoDBHashKey] // Hash key.
        public string id { get; set; }
        [DynamoDBProperty]
        public int score { get; set; }
    }

    public void CreateCharacter(string id2, int score) //캐릭터 정보를 DB에 올리기
    {
        Character c1 = new Character
        {
            id = id2,
            score = score,
        };
        context.SaveAsync(c1, (result) =>
        {
            //id가 happy, item이 1111인 캐릭터 정보를 DB에 저장
            if (result.Exception == null)
                Debug.Log("Success!");
            else
                Debug.Log(result.Exception);
        });
    }

    public void FindTopItemsAndStore()
{
    ScanRequest scanRequest = new ScanRequest
    {
        TableName = "character_info",
        Limit = 10,
    };

    DBclient.ScanAsync(scanRequest, (result) =>
    {
        if (result.Exception != null)
        {
            Debug.LogException(result.Exception);
            return;
        }

        List<Dictionary<string, AttributeValue>> items = result.Response.Items;

        // 디버깅을 위해 변수에 저장
        List<Character> characterList = new List<Character>();

        foreach (var item in items)
        {
            string id = item["id"].S;
            int score = int.Parse(item["score"].N);

            // 각 아이템을 Character 객체로 변환하여 리스트에 추가
            Character character = new Character
            {
                id = id,
                score = score
            };
            characterList.Add(character);

            Debug.Log($"ID: {id}, Score: {score}");
        }
        Debug.Log("Items stored in characterList.");
        HandleStoredItems(characterList);
    });
}

// 디버깅이나 추가 처리를 위한 예제 메서드
private void HandleStoredItems(List<Character> characterList)
{
    foreach (var character in characterList)
    {
        Debug.Log($"Handling stored item - ID: {character.id}, Score: {character.score}");
    }
}

}