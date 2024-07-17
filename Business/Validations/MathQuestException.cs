namespace Business.Validations
{
    public class MathQuestException : Exception
    {
        public MathQuestException() { }

        public MathQuestException(string message) : base(message) { }

        public MathQuestException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
