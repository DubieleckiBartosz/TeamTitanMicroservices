namespace Calculator.Application.Constants;

public class Keys
{
    //Background
    public static string SettlementBackgroundJobName(string identifierAccount) => "account_settlement" + identifierAccount;

    //Routing keys  
    public const string NewAccountQueueRoutingKey = "new_account_process_key";
    public const string DataCompletionQueueRoutingKey = "new_contract_process_key"; 
    public const string NewFinancialDataQueueRoutingKey = "new_financial_data_process_key";
    public const string NewPaymentDayQueueRoutingKey = "new_payment_month_day_process_key";
    public const string NewSettlementTypeQueueRoutingKey = "new_settlement_type_process_key"; 
    public const string NewDayHoursQueueRoutingKey = "new_day_hours_process_key"; 

}