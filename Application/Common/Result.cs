namespace Application.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public string? Error { get; }
        public T? Value { get; }

        private Result(T value)
        {
            IsSuccess = true;
            Value = value;
        }

        private Result(string error)
        {
            IsSuccess = false;
            Error = error;
        }

        public static Result<T> Success(T value) => new Result<T>(value);
        public static Result<T> Fail(string error) => new Result<T>(error);
    }
}