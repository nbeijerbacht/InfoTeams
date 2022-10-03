using AdaptiveCards;

namespace FormService.DTO;

public class SearchResultDTO
{
    public IEnumerable<string> Results { get; init; } = new List<string>();
}
