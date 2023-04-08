namespace Calculator.Application.Constants;

public class Keys
{
    //Background
    public static string SettlementBackgroundJobName(string identifierAccount) => "account_settlement" + identifierAccount;

    //Routing keys  
    public const string NewAccountQueueRoutingKey = "new_account_process_key";
    public const string DataCompletionQueueRoutingKey = "new_contract_process_key";
}