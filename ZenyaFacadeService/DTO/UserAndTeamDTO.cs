namespace ZenyaFacadeService.DTO;

public class UserAndTeamDTO
{
    public List<User> users { get; set; }
    public List<Team> teams { get; set; }
}

public class User
{
    public string name { get; set; }
    public string user_id { get; set; }
}

public class Team
{
    public string name { get; set; }
    public string team_id { get; set; }
}
