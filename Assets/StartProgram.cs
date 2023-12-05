using UnityEngine;
using UnityEngine.SceneManagement;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

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

    public void FindItem() //DB에서 캐릭터 정보 받기
    {
        Character c;
        context.LoadAsync<Character>("abcd", (AmazonDynamoDBResult<Character> result) =>
        {
            // id가 abcd인 캐릭터 정보를 DB에서 받아옴
            if (result.Exception != null)
            {
                Debug.LogException(result.Exception);
                return;
            }
            c = result.Result;
            Debug.Log(c.score); //찾은 캐릭터 정보 중 아이템 정보 출력
        }, null);
    }
}