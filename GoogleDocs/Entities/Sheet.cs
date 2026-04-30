namespace Entities;

public class Sheet(SpreadSheet spreadSheet, string id)
{
    public SpreadSheet SpreadSheet { get; set; } = spreadSheet;
    public string Id { get; set; } = id;
}