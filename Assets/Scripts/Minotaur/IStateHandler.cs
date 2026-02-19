/*****************************************************************************
// File Name : IStateHandler.cs
// Author : Brandon Koederitz
// Creation Date : 2/16/2026
// Last Modified : 2/16/2026
//
// Brief Description : Interface for objects that handle a collection of minotaur states.  Done so that the sub-states
// can exist.
*****************************************************************************/
namespace GGL.Minotaur
{
    public interface IStateHandler
    {
        public T GetState<T>() where T : MinotaurState;
        public T SetState<T>() where T : MinotaurState;
        public void SetState(MinotaurState state);
    }
}
