using System.Collections.Generic;
using System;
using static System.Console;
using GetOptNameSpace;
using NDesk.Options;

namespace ExamenFomatif_Ératosthène

{
	public class Program
	{
		//List of all the command that can be pass to a program via the command line input
		private static CommandArgument intCommand = new CommandArgument("i|integer", "Execute Fibonachi with an integer", TypeCode.Boolean);
		private static CommandArgument doubleCommand = new CommandArgument("d|double", "Execute Fibonachi with a doubles", TypeCode.Boolean);
		private static CommandArgument bigIntCommand = new CommandArgument("b|big", "Execute Fibonachi with a big integer", TypeCode.Boolean);
		private static CommandArgument maxLimit = new CommandArgument("n|limit=", "Execute Fibonachi with a {MAX}", TypeCode.Int32);
		private static CommandConstraits constraints = new CommandConstraits(new CommandArgument[] { intCommand, doubleCommand, bigIntCommand });

		static void Main(string[] args)
		{
			new Program().Principal(args);
		}

		private void Principal(string[] args)
		{
			try
			{
				GetOpt getOpt = new GetOpt(	args,
											new CommandArgument[] { intCommand, doubleCommand, bigIntCommand, maxLimit},
											new CommandConstraits[] { constraints });

				if (doubleCommand.Value != null)
					WriteLine("Execution of the double command...");

				if (intCommand.Value != null)
					WriteLine("Execution of the int command...");

				if (bigIntCommand.Value != null)
					WriteLine("Execution of the bigInt command...");

				if (maxLimit.Value != null)
					WriteLine((int)maxLimit.Value);
			}
			catch(OptionException e)
			{
				//The exception information needs to be more precise, right i dont pass any relavant information when i throw a new exception
				Write(e.OptionName + " " + e.Message);
			}
		}

		private void AfficherNombresPremier(List<bool> nombrePremiers)
		{
			for (int i = 0; i < nombrePremiers.Count; i++)
			{
				if (nombrePremiers[i])
					WriteLine(i);
			}
		}
	}
}
