namespace Properly.Services.Messaging.Constants
{
    public static class EmailTemplates
    {
        public const string ConfirmEmailSubject = "Confirm your email";
        public const string WelcomeEmailSubject = "Welcome to Our Platform!";

        public static string GetConfirmEmailBody(string callbackUrl)
        {
            return $@"
                <h1>Email Confirmation</h1>
                <p>Please confirm your account by <a href='{callbackUrl}'>clicking here</a>.</p>
                <p>If you didn't request this, please ignore this email.</p>";
        }

        public static string GetWelcomeEmailBody(string firstName)
        {
            return $@"
                <h1>Welcome, {firstName}!</h1>
                <p>Thank you for registering with us. We're thrilled to have you on board.</p>
                <p>If you have any questions, feel free to contact our support team at any time.</p>";
        }

        public const string PasswordResetSubject = "Reset your password";
        public static string GetPasswordResetEmailBody(string resetLink)
        {
            return $@"
                <h1>Password Reset</h1>
                <p>You have requested to reset your password. You can do so by clicking the following link:</p>
                <p><a href='{resetLink}'>Reset Password</a></p>
                <p>If you didn't request a password reset, please ignore this email.</p>";
        }
    }
}
