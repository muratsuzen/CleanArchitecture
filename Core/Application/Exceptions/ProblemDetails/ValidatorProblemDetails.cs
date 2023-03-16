using Newtonsoft.Json;

namespace Application.Exceptions.ProblemDetails
{
    public class ValidatorProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        public object Errors { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
