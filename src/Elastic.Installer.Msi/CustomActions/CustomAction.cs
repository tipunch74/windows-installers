﻿using System.IO;
using System.Linq;
using WixSharp;
using System;
using Elastic.Installer.Domain.Elasticsearch.Model;

namespace Elastic.Installer.Msi.CustomActions
{
	public abstract class CustomAction
	{
		public abstract Type ProductType { get; }

		public abstract int Order { get; }

		public abstract string Name { get; }

		public abstract Return Return { get; }

		public abstract When When { get; }

		public abstract Step Step { get; }

		public abstract Condition Condition { get; }

		public abstract Sequence Sequence { get; }

		public virtual Execute Execute => Execute.immediate;

		public virtual bool NeedsElevatedPrivileges => true;

		public virtual ManagedAction ToManagedAction()
		{
			return new ManagedAction
			{
				Name = this.Name,
				Id = this.Name,
				MethodName = this.Name.Replace("Action", ""),
				RefAssemblies =
					new[] { Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Elastic.Installer.Domain.dll") },
				Return = this.Return,
				When = this.When,
				Step = this.Step,
				Condition = this.Condition,
				Sequence = this.Sequence,
				Execute = this.Execute,
				Impersonate = !this.NeedsElevatedPrivileges,
				UsesProperties = string.Join(",", InstallationModelArgumentParser.AllArguments
					.Concat(new string[] { "UILevel", "INSTALLDIRECTORY.bin", "VERSION" }))
			};
		}
	}

	public abstract class CustomAction<TProduct> : CustomAction
		where TProduct : Product
	{
		public override Type ProductType => typeof(TProduct);
	}
}