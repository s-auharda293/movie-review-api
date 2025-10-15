using MovieReviewApi.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReviewApi.Infrastructure.Jobs
{
    public class EmailJob
    {
        private readonly IEmailService _emailService;

        public EmailJob(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task SendWelcomeEmail(string to, string userName)
        {
            var htmlBody = $@"
        <html>
          <head>
            <style>
              body {{
                font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                background-color: #f4f4f4;
                margin: 0;
                padding: 0;
              }}
              .container {{
                max-width: 600px;
                margin: 40px auto;
                padding: 30px;
                background: #ffffff;
                border-radius: 12px;
                box-shadow: 0 4px 20px rgba(0,0,0,0.1);
                text-align: center;
              }}
              h1 {{
                color: #222222;
                font-size: 24px;
                margin-bottom: 15px;
              }}
              p {{
                font-size: 16px;
                color: #555555;
                line-height: 1.5;
                margin-bottom: 25px;
              }}
     
              @media only screen and (max-width: 620px) {{
                .container {{
                  padding: 20px;
                }}
                h1 {{
                  font-size: 20px;
                }}
                p {{
                  font-size: 14px;
                }}
              }}
            </style>
          </head>
          <body>
            <div class='container'>
              <h1>Welcome to Movie Review App, {userName}!</h1>
              <p>Thanks for registering. We're excited to have you onboard and can't wait for you to start exploring our movies and reviews.</p>
            </div>
          </body>
        </html>";


            await _emailService.SendEmailAsync(to, "Welcome!", htmlBody, true);
        }
    }

}
