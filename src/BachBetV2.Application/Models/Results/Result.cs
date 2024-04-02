namespace BachBetV2.Application.Models.Results
{
    public abstract class Result
    {
        public bool Success { get; protected set; }
        public bool Failure => !Success;
    }

    public abstract class Result<T> : Result
    {
        private T? _data;

        public T? Data
        {
            get => Success ? _data : throw new Exception($"Cannot access .{nameof(Data)} when .{nameof(Success)} is false");
            set => _data = value;
        }

        protected Result(T data)
        {
            Data = data;
        }
    }
}
