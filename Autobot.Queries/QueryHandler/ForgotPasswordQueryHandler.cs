using Autobot.Infrastructure.Email;
using Autobot.Infrastructure.Identity;
using Autobot.Queries.Query;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Autobot.Queries.QueryHandler
{
    public class ForgotPasswordQueryHandler : IRequestHandler<ForgotPasswordQuery, string>
    {
        private readonly IUserManagerRepository _repository;
        private readonly IEmailService _emailService;
        public ForgotPasswordQueryHandler(IUserManagerRepository repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        /// <summary>
        /// Get user details by username
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>user detail</returns>
        public async Task<string> Handle(ForgotPasswordQuery request, CancellationToken cancellationToken)
        {
            var user = await _repository.FindUserByEmail(request.Email);
            if (user == null)
            {
                return "User doesnot exist";
            }

            var code = await _repository.GeneratePasswordResetToken(user);
            code = HttpUtility.UrlEncode(code);
            var callbackUrl = "http://XXXX.amazonaws.com/reset-password" + $"?userid={user.Id}&code={code}";
            string subject = "Autobot PasswordReset";
            string content = "<html><body><p style='color:black'>You recently asked to reset your password. Please click the link below to reset your password. <br><br> <a href=\"" + callbackUrl + "\">Change password</a> <br><br> Many thanks, <br> The Team <br><br><br></p></body></html>";
            var recepient = user.Email;
            var message = new Message(recepient, subject, content);
            _emailService.SendEmail(message);

            return "A password reset email has been sent";
        }
    }
}