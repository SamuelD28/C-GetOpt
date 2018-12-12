using System;
using System.Collections.Generic;
using System.Linq;
using NDesk.Options; //Needed package to make this work. Available in nugget

namespace GetOptNameSpace
{
	/// <summary>
	/// Class that holds information for parsing a command line argument
	/// </summary>
	public class CommandArgument
	{
		/// <summary>
		/// Constructor for a command line argument
		/// </summary>
		/// <param name="name">Name that will be used to parse the command line</param>
		/// <param name="description">Description used for showing the help message</param>
		/// <param name="commandType">The underlying type of the command value</param>
		/// <param name="initialValue">Optionnal initial value</param>
		public CommandArgument(string name, string description, TypeCode commandType, object initialValue = null)
		{
			Name		= name;
			Description = description;
			Action		= CreateActionCommand(commandType);
			Value		= initialValue;
		}

		public string Name { get;}
		public string Description { get;}
		public object Value { get; private set; }
		public Action<string> Action { get; private set; }

		/// <summary>
		/// Function that create an action command based on the underlying type of the command
		/// </summary>
		/// <param name="commandType">Underlying type of the command</param>
		/// <returns></returns>
		private Action<object> CreateActionCommand(TypeCode commandType)
		{
			switch (commandType)
			{
				case TypeCode.Int16:
				case TypeCode.Int32:
				case TypeCode.Int64:	return (v) => 
				{
					int value;
					if (int.TryParse(v.ToString(), out value))
						Value = value;
					else
						throw new OptionException("Wrong Value", "?");
				};
				case TypeCode.Boolean:	return (v) => 
				{
					Value = v != null;
				};
				case TypeCode.String:	return (v) => 
				{
					//Maybe add a verification
					Value = v.ToString();
				};
				case TypeCode.Char:		return (v) => 
				{
					if (v.ToString().Length == 1)
						Value = v.ToString();
					else
						throw new OptionException("The char value exceed 1 character", "?"); //Need work
				};
				default: throw new TypeAccessException("The specified type is not handled");
			}
		}
	}

	/// <summary>
	/// Class used to create constraints between different commands
	/// </summary>
	public class CommandConstraits
	{
		public IEnumerable<CommandArgument> mutuallyExclusif = null; //Mutually exclusif commands
		public CommandArgument dependsOn = null;					 //Direct dependance between two commands. TODO

		/// <summary>
		/// Constructor for creating a new constraints between commands
		/// </summary>
		/// <param name="mutuallyExclusif">List of all the commands that are mutually exclusif</param>
		/// <param name="dependsOn">Commands that depends on one another</param>
		public CommandConstraits(IEnumerable<CommandArgument> mutuallyExclusif, CommandArgument dependsOn = null)
		{
			this.mutuallyExclusif = mutuallyExclusif;
			this.dependsOn = dependsOn;
		}

		/// <summary>
		/// Function that verify that the list mutually excusif is indeed mutually exclusif
		/// </summary>
		/// <returns></returns>
		public bool AreCommandsMutuallyExclusif()
		{
			if (mutuallyExclusif.Where(m => m.Value != null).Count() > 1)
				return false;

			return true;
		}

		//TODO Add the direct dependance constrait
	}

	/// <summary>
	/// Class used to create a new GetOpt object for parsing command line arguments
	/// </summary>
	public class GetOpt
	{
		private static CommandArgument helpCommand = new CommandArgument("h|help", "Show Help", TypeCode.Boolean, false); //The help command is automaticly added
		private List<CommandArgument> commandArguments = new List<CommandArgument>(){ helpCommand };	//List of all the command line arguments possible
		private List<CommandConstraits> commandConstraits = new List<CommandConstraits>();	//List of all the constraints for the commands

		/// <summary>
		/// Constructor for a new GetOpt for the program
		/// </summary>
		/// <param name="args">Raw Arguments that were passed to the main function</param>
		/// <param name="commands">Commands possible for the program</param>
		/// <param name="constraits">Optionnal constraint for the program</param>
		public GetOpt(string[] args, IEnumerable<CommandArgument> commands, IEnumerable<CommandConstraits> constraits = null)
		{
			commandArguments.AddRange(commands);
			commandConstraits.AddRange(constraits);
			ParseCommandLineArguments(args);
		}

		/// <summary>
		/// Function that parsed the raw arguments passed to the main function and apply them to the Commands list
		/// </summary>
		/// <param name="args">Raw arguments passed to the main function</param>
		public void ParseCommandLineArguments(string[] args)
		{
				OptionSet options = CreateOptionSet();
				options.Parse(args); 

				foreach (CommandConstraits c in commandConstraits)
				{
					if (!c.AreCommandsMutuallyExclusif())
						throw new OptionException("Options must be mutually exclusif", "-i|-d|-m");

					//TODO : Check for the idrrect dependance constraits
				}

				if ((bool)helpCommand.Value)
					ShowHelp(options);
		}

		/// <summary>
		/// Function used to create a new option set that will be used for parsing
		/// </summary>
		/// <returns></returns>
		public OptionSet CreateOptionSet()
		{
			OptionSet options = new OptionSet();
			foreach (var c in commandArguments)
			{
				options.Add(c.Name, c.Description, c.Action);
			}
			return options;
		}

		/// <summary>
		/// Function that show all the command helps
		/// </summary>
		/// <param name="options">Option set used to display all the command lines description</param>
		static void ShowHelp(OptionSet options) => options.WriteOptionDescriptions(Console.Out);
	}
}
