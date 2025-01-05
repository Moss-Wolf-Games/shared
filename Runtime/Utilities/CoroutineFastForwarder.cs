using System.Collections;

public class CoroutineFastForwarder
{
    public void FastForward(IEnumerator coroutine)
    {
        while (coroutine.MoveNext())
        {
            if (coroutine.Current != null && coroutine.Current is IEnumerator childCoroutine)
            {
                FastForward(childCoroutine);
            }
        }
    }
}
