using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class State
{
    public virtual void Enter( IStateHolder stateHolder ) { }
    public virtual void Exit( IStateHolder stateHolder ) { }
    public void Next( IStateHolder stateHolder, State next_state )
    {
        stateHolder.State.Exit( stateHolder );

        stateHolder.State = next_state;

        next_state.Enter( stateHolder );
    }
    public virtual void Handle( IStateHolder stateHolder ) { }
}

public interface IStateHolder
{
    public State State { get; set; }
}