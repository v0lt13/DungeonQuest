﻿using System;

namespace DungeonQuest.Debuging
{
	public class DebugCommandBase
	{
		private string commandID;
		private string commandDescription;
		private string commandFormat;

		public string CommandID { get { return commandID; } }
		public string CommandDescription { get { return commandDescription; } }
		public string CommandFormat { get { return commandFormat; } }

		public DebugCommandBase(string id, string description, string format) 
		{
			commandID = id;
			commandDescription = description;
			commandFormat = format;
		}
	}

	public class DebugCommand : DebugCommandBase
	{
		private Action command;

		public DebugCommand(string id, string description, string format, Action command) : base(id, description, format)
		{
			this.command = command;
		}

		public void Invoke()
		{
			command.Invoke();
		}
	}

	public class DebugCommand<T1> : DebugCommandBase
	{
		private Action<T1> command;

		public DebugCommand(string id, string description, string format, Action<T1> command) : base(id, description, format)
		{
			this.command = command;
		}

		public void Invoke(T1 value)
		{
			command.Invoke(value);
		}
	}

	public class DebugCommand<T1, T2> : DebugCommandBase
	{
		private Action<T1, T2> command;

		public DebugCommand(string id, string description, string format, Action<T1, T2> command) : base(id, description, format)
		{
			this.command = command;
		}

		public void Invoke(T1 primaryValue, T2 secondaryValue)
		{
			command.Invoke(primaryValue, secondaryValue);
		}
	}
}
