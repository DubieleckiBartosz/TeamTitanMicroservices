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
}