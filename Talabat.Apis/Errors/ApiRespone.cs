namespace Talabat.APIs.Errors
{
    public class ApiRespone
    {
        public int Status { get; set; }
        public string? Message{ get; set; }

        public ApiRespone(int status , string? message = null)
        {
            Status = status;
            Message = message ?? DefaultErrorMessage(status);
        }

        private string? DefaultErrorMessage(int statuscode)
        {
            return statuscode switch
            {
                400 => "A Bad Request You Made",
                401 => "UnAuthirized ,You Made",
                404 => "Resource Not Found",
                500 => "An unexpected error occurred. Please try again later.",
                _  => null
            };
        }
    }
}
