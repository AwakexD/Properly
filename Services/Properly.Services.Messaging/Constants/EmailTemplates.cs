namespace Properly.Services.Messaging.Constants
{
    public static class EmailTemplates
    {
        public const string ConfirmEmailSubject = "Confirm your email";
        public const string WelcomeEmailSubject = "Welcome to Our Platform!";
        public const string PasswordResetSubject = "Reset your password";
        public const string EmailChangeSubject = "Your email address has been changed";
        public const string PasswordChangeSubject = "Your password has been changed";
        public const string PhoneNumberChangeSubject = "Your phone number has been changed";

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

        public static string GetPasswordResetEmailBody(string resetLink)
        {
            return $@"
                <h1>Password Reset</h1>
                <p>You have requested to reset your password. You can do so by clicking the following link:</p>
                <p><a href='{resetLink}'>Reset Password</a></p>
                <p>If you didn't request a password reset, please ignore this email.</p>";
        }

        public static string GetEmailChangeNotificationBody(string oldEmail, string newEmail)
        {
            return $@"
                <h1>Email Address Changed</h1>
                <p>Your email address has been successfully changed from <strong>{oldEmail}</strong> to <strong>{newEmail}</strong>.</p>
                <p>If you did not request this change, please contact our support team immediately.</p>";
        }

        public static string GetPasswordChangeNotificationBody()
        {
            return @"
                <h1>Password Changed</h1>
                <p>Your account password has been successfully changed.</p>
                <p>If you did not request this change, please reset your password immediately and contact our support team for further assistance.</p>";
        }

        public static string GetPhoneNumberChangeNotificationBody(string oldPhoneNumber, string newPhoneNumber)
        {
            return $@"
                <h1>Phone Number Changed</h1>
                <p>Your phone number has been successfully updated from <strong>{oldPhoneNumber}</strong> to <strong>{newPhoneNumber}</strong>.</p>
                <p>If you did not request this change, please contact our support team immediately.</p>";
        }
    }
}
