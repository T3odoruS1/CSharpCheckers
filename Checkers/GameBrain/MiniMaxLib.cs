namespace GameBrain;

public static class MiniMaxLib
{
    public static List<(TBoard boardState, int utility)> MiniMaxDecision<TBoard, TPlayer>(
        TBoard board,
        TPlayer player,
        Func<TPlayer, TPlayer> getNextPlayer,
        Func<TBoard, TPlayer, List<TBoard>> getNextBoardStates,
        Func<TBoard, bool> isTerminalState,
        Func<TBoard, TPlayer, int> getUtilityValue
    )
    {
        var res = new List<(TBoard boardState, int utility)>();

        foreach (var nextState in getNextBoardStates(board, player))
        {
            res.Add(
                (nextState,
                    MiniMaxMin(
                        player,
                        nextState,
                        player,
                        getNextPlayer,
                        getNextBoardStates,
                        isTerminalState,
                        getUtilityValue
                    )
                )
            );
        }

        return res;
    }

    private static int MiniMaxMin<TBoard, TPlayer>(
        TPlayer initialPlayer,
        TBoard board,
        TPlayer player,
        Func<TPlayer, TPlayer> getNextPlayer,
        Func<TBoard, TPlayer, List<TBoard>> getNextBoardStates,
        Func<TBoard, bool> isTerminalState,
        Func<TBoard, TPlayer, int> getUtilityValue
    )
    {
        if (isTerminalState(board)) return getUtilityValue(board, initialPlayer);
        var value = int.MaxValue;

        foreach (var nextState in getNextBoardStates(board, getNextPlayer(player)))
        {
            value = Math.Min(
                value,
                MiniMaxMax(
                    initialPlayer,
                    nextState,
                    getNextPlayer(player),
                    getNextPlayer,
                    getNextBoardStates,
                    isTerminalState,
                    getUtilityValue
                ));
        }

        return value;
    }

    private static int MiniMaxMax<TBoard, TPlayer>(
        TPlayer initialPlayer,
        TBoard board,
        TPlayer player,
        Func<TPlayer, TPlayer> getNextPlayer,
        Func<TBoard, TPlayer, List<TBoard>> getNextBoardStates,
        Func<TBoard, bool> isTerminalState,
        Func<TBoard, TPlayer, int> getUtilityValue
    )
    {
        if (isTerminalState(board)) return getUtilityValue(board,initialPlayer);
        var value = int.MinValue;

        foreach (var nextState in getNextBoardStates(board, getNextPlayer(player)))
        {
            value = Math.Max(
                value,
                MiniMaxMin(
                    initialPlayer,
                    nextState,
                    getNextPlayer(player),
                    getNextPlayer,
                    getNextBoardStates,
                    isTerminalState,
                    getUtilityValue
                ));
        }

        return value;
    }

    
}