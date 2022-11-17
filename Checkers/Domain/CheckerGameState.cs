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
}