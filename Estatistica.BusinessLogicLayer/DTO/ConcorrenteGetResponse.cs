namespace Estatistica.BusinessLogicLayer.DTO;

public class ConcorrenteGetResponse()
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public List<string> Filiais { get; set; }
}

