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
    password = temp["Password"]
    #No check for duplicates
    response = table.put_item(
        Item={
            "Username":username,
            "Password":password,
            "PlayerLevel":decimal.Decimal(1),
            "MatchesDrawn":decimal.Decimal(0),
            "MatchesLost":decimal.Decimal(0),
            "MatchesWon":decimal.Decimal(0),
            "SkillLv":decimal.Decimal(0),
            "TotalMatches":decimal.Decimal(0)
        })
    
    return {
        'statusCode': 200,
        'body': json.dumps(response, indent=4, cls=DecimalEncoder)
    }
