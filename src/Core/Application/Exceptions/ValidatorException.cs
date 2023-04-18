namespace Application.Exceptions
{
    public class ValidatorException : Exception
    {
        public ValidatorException(string message) : base(message)
        {

        }

        public ValidatorException(string[] errors) : base("Multiple errors occurred. See error details.")
        {
            Errors = errors;
        }

        public string[] Errors { get; set; }
    }
}
