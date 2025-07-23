namespace Estatistica.BusinessLogicLayer.DTO;

public record ConcorrenteGetResponse(int Id,string? Nome)
{
    public ConcorrenteGetResponse() : this(default, default)
    {
        
    }
}

