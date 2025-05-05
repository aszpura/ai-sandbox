namespace ai_tic_tac_toe;

public class UpdateResult
{
    public bool IsSuccess { get; }
    public string ErrorMessage { get; }

    public UpdateResult(bool isSuccess, string errorMessage = "")
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }
}