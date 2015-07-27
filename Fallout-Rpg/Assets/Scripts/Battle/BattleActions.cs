using System.Collections;

public enum BattleActions
{
    Move, MeleeAttack

}

public class Action
{
    public BattleActions actionType;
    public object data;
    public Action(BattleActions action, object data)
    {
        actionType = action;
        this.data = data;
    }

    public void dispose()
    {
        data = null;
    }
}