using Mert.EventBus;

public class GameStateChanged : IEvent
{
    public GameState State;
}

public class PlayerLevelChanged : IEvent
{
    public int Level;
}

public class CoinChanged : IEvent
{
    public float Coin;
}

public class PowerUpPurchased : IEvent
{
    public PowerUpData PowerUpData;
}

public class AmmoFired : IEvent
{
    public AmmoData AmmoData;
}
