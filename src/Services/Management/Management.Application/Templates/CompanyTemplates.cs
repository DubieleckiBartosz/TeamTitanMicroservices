using System.Drawing;

namespace Management.Application.Templates;

public class CompanyTemplates
{
    public static string InitCompanyMessageTemplate(string verificationUri) => $@"<html>
     <head>
        <meta charset=""UTF-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <title>Confirm Your Account</title>
        <style>
            body {{
                font-family: Arial, Helvetica, sans-serif;
                font-size: 16px;
                line-height: 1.5;
            }} 
            .container {{
                max-width: 600px;
                margin: 0 auto;
                padding: 20px;
                background-color: #f7f7f7;
                border-radius: 5px;
                color: #444;
            }}
            .contact{{
                color: #fff !important;
            }}
            .btn {{
                display: inline-block;
                padding: 10px 20px;
                background-color: #007bff;
                color: #fff;
                text-decoration: none;
                border-radius: 5px;
                transition: background-color 0.3s ease;
            }}
            .btn:hover {{
                background-color: #0069d9;
                color: #444;
            }}
            </style>
        </head>
            <body>
              <div class=""container"">
                <h1>Confirm your account</h1> 
                <p>Thank you for creating company on our website.</p>
                <p>Please click the button below to confirm your choice:</p>
                <a class=""btn"" href={verificationUri}><span class=""contact"">Confirm Account</span></a> 
              </div>  
            </body>
    </html>";

    public static string NewDayOffRequestMessageTemplate(string employeeName, string employeeCode) => $@"
        <!DOCTYPE html>
        <html>
        <head>
            <title>New Leave Application</title>
            <style>
                body {{
                    font - family: Arial, sans-serif;
                    font-size: 16px;
                    line-height: 1.5;
                    color: #333;
                    background-color: #f7f7f7;
                }}
                .container {{
                    max - width: 600px;
                    margin: 0 auto;
                    padding: 20px;
                    background-color: #fff;
                    border: 1px solid #ddd;
                    border-radius: 4px;
                    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.05);
                }}
                h1, h2, h3, h4, h5, h6 {{
                    font - family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                    font-weight: bold;
                    color: #444;
                }}
                p {{
                    margin - bottom: 1em;
                }}
                strong {{
                    color: #007bff;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <h1>New Leave Request</h1>
                <p>Dear Manager,</p>
                <p>Employee <strong>{employeeName}</strong> with the code <strong>{employeeCode}</strong> has submitted a new leave application.</p>
                <p>Please review the application and respond accordingly.</p>
                <p>Thank you.</p>
            </div>
        </body>
        </html>
    ";
}