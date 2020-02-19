using System;
using System.IO;
using System.Threading.Tasks;

namespace ToAsyncOrNotToAsync
{
	// https://sharplab.io/#v2:EYLgxg9gTgpgtADwGwBYA0ATEBqAPgAQCYBGAWACh8AGAAn2IFYBuC/AZjsJoGEaBvCgEhB9JDQCWAOwAuNACIQAyhAC2MaQAspAcwAUAShoBeAHw0qLcsNF0AHHSQAeKdLMLlazToCCAZwCekmAGxmYWFEIixGL4KDQAsgCGUrr0VADaALo0iVDavvqRAlbC7qrqWpJ6+pbCgmWeldp+gcH6AHQA6snSBrWCAL4UA0A
	class Program
    {
		static int DoSomething() => 0;
		static async Task<int> DoSomethingAsync() => 0;

		static void Main(string[] args)
		{
			DoSomething();
			DoSomethingAsync().Wait();
		}

		//static async Task Main(string[] args)
		//{
		//	var aaw = new AsyncAllTheWay();
		//	await aaw.CheckAvailabilityOf(new Uri[] { new Uri("https://google.com") });

		//	var arw = new AvoidReturnAwait();
		//	await arw.CheckAvailabilityOf(new Uri("https://google.com"));

		//	var afa = new AvoidFakeAsync();
		//	await afa.CheckAvailabilityOf(new Uri[] { new Uri("https://google.com") });

		//}
	}

	#region spoiler
	// Overhead added: state machine + Task that taht is not run but finishes right away
	/*
internal class Program
{
	[CompilerGenerated]
	private sealed class <DoSomethingAsync>d__2 : IAsyncStateMachine
	{
		public int <>1__state;

		public AsyncTaskMethodBuilder<int> <>t__builder;

		private void MoveNext()
		{
			int num = <>1__state;
			int result;
			try
			{
				result = 0;
			}
			catch (Exception exception)
			{
				<>1__state = -2;
				<>t__builder.SetException(exception);
				return;
			}
			<>1__state = -2;
			<>t__builder.SetResult(result);
		}

		void IAsyncStateMachine.MoveNext()
		{
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			this.SetStateMachine(stateMachine);
		}
	}

	private static void Main(string[] args)
	{
		DoSomething();
		DoSomethingAsync().Wait();
	}

	private static int DoSomething()
	{
		return 0;
	}

	[AsyncStateMachine(typeof(<DoSomethingAsync>d__2))]
	[DebuggerStepThrough]
	private static Task<int> DoSomethingAsync()
	{
		<DoSomethingAsync>d__2 stateMachine = new <DoSomethingAsync>d__2();
		stateMachine.<>t__builder = AsyncTaskMethodBuilder<int>.Create();
		stateMachine.<>1__state = -1;
		AsyncTaskMethodBuilder<int> <>t__builder = stateMachine.<>t__builder;
		<>t__builder.Start(ref stateMachine);
		return stateMachine.<>t__builder.Task;
	}
}
    */
	#endregion spoiler
}
