public abstract class Solver : ISolver
{
    private char Part { get; set; }

    public void ReadInput(int day)
    {
        throw new NotImplementedException();
    }

    public virtual void SolvePart(char part)
    {
        Part = part;
    }
}