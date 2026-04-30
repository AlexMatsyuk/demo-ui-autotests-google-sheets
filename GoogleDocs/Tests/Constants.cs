using Entities;

namespace Tests;

public class Constants
{
    // in the real project those values should never be hard-coded, but generated in preconditions instead
    // the access to the tested application is limited: it's not possible to generate new spreadsheets and sheets using API or DB requests
    public static readonly SpreadSheet SpreadSheet1 = new("1f4fxwBr8PUxBZk65f_Edx9Nt6pTzK34gTvgCF08Npyg");
}