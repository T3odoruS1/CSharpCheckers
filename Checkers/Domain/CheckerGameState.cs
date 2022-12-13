using System.ComponentModel.DataAnnotations.Schema;
using GameBrain;

namespace Domain;


public class CheckerGameState
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool NextMoveByBlack { get; set; }
    
    public string SerializedGameBoard { get; set; } = default!;

    public int? CheckerThatPreformedTakingX { get; set; }
    public int? CheckerThatPreformedTakingY { get; set; }


    public int CheckerGameId { get; set; }
    
    public CheckerGame? CheckerGame { get; set; }


    public override string ToString()
    {
        return $"Next move by black: {NextMoveByBlack}, PreformedX: {CheckerThatPreformedTakingX}, PreformedY: {CheckerThatPreformedTakingY}";
    }

    public override bool Equals(object? obj)
    {
        if (GetType() != obj!.GetType())
        {
            return false;
        }

        var other = (CheckerGameState)obj;
        return Equals(other);
    }

    private bool Equals(CheckerGameState other)
    {
        return Id == other.Id &&
               CreatedAt.Equals(other.CreatedAt) &&
               NextMoveByBlack == other.NextMoveByBlack &&
               SerializedGameBoard == other.SerializedGameBoard &&
               CheckerThatPreformedTakingX == other.CheckerThatPreformedTakingX &&
               CheckerThatPreformedTakingY == other.CheckerThatPreformedTakingY &&
               CheckerGameId == other.CheckerGameId &&
               Equals(CheckerGame, other.CheckerGame);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, 
            CreatedAt, 
            NextMoveByBlack, 
            SerializedGameBoard, 
            CheckerThatPreformedTakingX, 
            CheckerThatPreformedTakingY, 
            CheckerGameId, 
            CheckerGame);
    }
}