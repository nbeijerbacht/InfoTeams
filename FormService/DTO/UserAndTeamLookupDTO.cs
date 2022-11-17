namespace FormService.DTO;

public class UserAndTeamLookupDTO
{
    public List<UT> users { get; set; }
    public List<UT> teams { get; set; }
}

public class UT
{
    public string name { get; set; }
    public string id { get; set; }
}