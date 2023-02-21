namespace Calculator.Domain.BonusProgram;

public class Bonus
{ 
    public string Creator { get; }
    public bool Settled { get; private set; } 
    public DateTime Created { get; } 

    private Bonus(string creator)
    {
        Settled = false;  
        Created = DateTime.UtcNow; 
        Creator = creator;
    }

    public static Bonus Create(string creator) => new Bonus(creator);

    public void AsSettled()
    {
        Settled = true;
    } 
}