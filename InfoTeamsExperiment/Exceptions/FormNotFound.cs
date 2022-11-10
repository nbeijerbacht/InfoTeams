namespace ZenyaBot.Exceptions;

public class FormNotFound : Exception
{
    public FormNotFound(string id) : base ($"Could not find form with id {id}")
    {
    }
}
