import json
import boto3
import decimal
from boto3.dynamodb.conditions import Key, Attr
from botocore.exceptions import ClientError

class DecimalEncoder(json.JSONEncoder):
    def default(self, o):
        if isinstance(o, decimal.Decimal):
            if abs(o) % 1 >0:
                return float(o)
            else:
                return int(o)
        return super(DecimalEncoder, self).default(o)

def lambda_handler(event, context):
    message = ""
    dynamoDB = boto3.resource("dynamodb")
    table = dynamoDB.Table("Connect4Players")
    
    temp = json.loads(event["body"])
    
    username = temp["Username"]
    level = temp["PlayerLevel"]
    draw = temp["MatchesDrawn"]
    lost = temp["MatchesLost"]
    won = temp["MatchesWon"]
    skill = temp["SkillLv"]
    total = temp["TotalMatches"]
    
    response = table.update_item(
        Key={ "Username":username },
        UpdateExpression="set PlayerLevel=:l, MatchesDrawn=:d, MatchesLost=:t, MatchesWon=:w, SkillLv=:s, TotalMatches=:m",
        ExpressionAttributeValues={
            ":l":str(level),
            ":d":str(draw),
            ":t":str(lost),
            ":w":str(won),
            ":s":str(skill),
            ":m":str(total)
        })
    
    return {
        'statusCode': 200,
        'body': json.dumps(response, indent=4, cls=DecimalEncoder)
    }
