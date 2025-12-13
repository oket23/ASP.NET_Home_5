namespace Home_5.API.Responses.Base;

public class ResponseError
{
    public required string Message { get; set; }
    
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public string? StackTrace { get; set; }
}