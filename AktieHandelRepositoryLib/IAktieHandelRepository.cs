namespace AktieHandelRepositoryLib
{
    public interface IAktieHandelRepository
    {
        AktieHandel Add(AktieHandel aktieHandel);
        AktieHandel? Delete(int id);
        List<AktieHandel> Get(double exchangePrice, string? name);
        List<AktieHandel> GetAll();
        AktieHandel? GetById(int id);
        AktieHandel? Update(int id, AktieHandel aktie);
    }
}