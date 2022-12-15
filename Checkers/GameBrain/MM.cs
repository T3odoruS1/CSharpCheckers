namespace GameBrain;

public static class MM
{
     public static (Move move, double evaluation) Minimax(
        int depth,
        CheckersBrain brain,
        bool isMax,
        Move? move = null)
    {
        if (depth == 0 || brain.IsGameOver() && move != null)
        {
            var evaluation = brain.EvaluateBoard(brain.GetBoard());
            // Console.WriteLine($"Reached depth 0. Returning {move} coords and {evaluation}");
            return (move!,evaluation);
        }

        if (isMax)
        {
            var maxEval = (double) int.MinValue;
            Move? bestMove = null;
            // Maximum is white.
            foreach (var possibleMove in brain.GetAllMoves(brain.NextMoveByBlack()))
            {

                var brainClone = brain.CloneBrain();
                // Console.WriteLine($"ismin Trying to do move by black {brainClone.NextMoveByBlack()}, to coords {possibleMove}, depth {depth}");

                brainClone.MoveChecker(possibleMove.x, possibleMove.y, possibleMove.destX, possibleMove.destY);
                
                var minimax = Minimax(
                    
                    depth - 1,
                    brainClone,
                    !brainClone.NextMoveByBlack(), 
                    possibleMove);
                
                if (maxEval <= minimax.evaluation)
                {
                    bestMove = possibleMove;
                }
                maxEval = Math.Max(maxEval, minimax.evaluation);
                
            }
            return (bestMove!, brain.EvaluateBoard(brain.GetBoard()));
        }
        else
        {
            var minEval = (double) int.MaxValue;
            Move? bestMove = null;
            // Maximum is white.
            foreach (var possibleMove in brain.GetAllMoves(brain.NextMoveByBlack()))
            {
                var brainClone = brain.CloneBrain();
                // Console.WriteLine($"ismin Trying to do move by black {brainClone.NextMoveByBlack()}, to coords {possibleMove}, depth {depth}");
                brainClone.MoveChecker(possibleMove.x, possibleMove.y, possibleMove.destX, possibleMove.destY);
                var minimax = Minimax(
                    depth - 1,
                    brainClone,
                    !brainClone.NextMoveByBlack(),
                    possibleMove);
                
                var eval = minimax.evaluation;
                if (minEval >= eval)
                {
                    bestMove = possibleMove;
                }
                minEval = Math.Min(minEval, eval);
                
            }
            return (bestMove!, brain.EvaluateBoard(brain.GetBoard()));
        }
    }
    
}