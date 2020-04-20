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
    
    try:
        response = table.get_item(Key={"Username":username})
    except ClientError as e:
        message = str(e.response["Error"]["Message"])
    else:
        item = response["Item"]
        if item["Password"] == password:
            message = {
                "Username": item["Username"],
                "MatchesDrawn":int(decimal.Decimal(item["MatchesDrawn"])),
                "MatchesLost":int(decimal.Decimal(item["MatchesLost"])),
                "MatchesWon":int(decimal.Decimal(item["MatchesWon"])),
                "PlayerLevel":int(decimal.Decimal(item["PlayerLevel"])),
                "SkillLv":float(decimal.Decimal(item["SkillLv"])),
                "TotalMatches":int(decimal.Decimal(item["TotalMatches"]))
            }
        else:
            message = "Wrong password"
            
    return {
        'statusCode': 200,
        'body': json.dumps(message)
    }
