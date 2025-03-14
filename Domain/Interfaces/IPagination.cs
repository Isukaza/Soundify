namespace Domain.Interfaces;

public interface IPagination
{
    public int Page { get; init; }

    public int Size { get; init; }
}