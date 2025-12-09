namespace Talabat.APIs.Errors
{
    public class ApiValidationErrors : ApiRespone
    {
        public List<string> Errors { get; set; }
        public ApiValidationErrors():base(400)
        {
            Errors = new List<string>();
        }

    }
}
