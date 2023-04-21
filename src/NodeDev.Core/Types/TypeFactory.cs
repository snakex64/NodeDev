﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeDev.Core.Types
{
	public static class TypeFactory
	{
		private static readonly Dictionary<Type, RealType> RealTypes = new();

		private static List<string> IncludedNamespaces = new()
		{
			"System",
			"System.Collections.Generic",
			"System.Linq",
			"System.Text",
			"System.Threading.Tasks",
		};
		private static Dictionary<string, List<string>> TypeCorrespondances = new()
		{
			["System.Int32"] = new() { "int" },
			["System.Int64"] = new() { "long" },
			["System.Single"] = new() { "float" },
			["System.Double"] = new() { "double" },
			["System.Boolean"] = new() { "bool" },
			["System.String"] = new() { "string" },
			["System.Void"] = new() { "void" },
		};

		public static RealType Get(Type type) => RealTypes.TryGetValue(type, out var realType) ? realType : RealTypes[type] = new(type);

		public static readonly ExecType ExecType = new();

		public static UndefinedGenericType CreateGenericType(string name) => new(name);

		public static string? CreateBaseFromUserInput(string typeName, out Type? type)
		{
			typeName = typeName.Replace(" ", "");
			if( typeName.Count(c => c == '<') != typeName.Count(c => c == '>') )
			{
				type = null;
				return "Bracket opened but never closed";
			}

			if(!typeName.Any(c => c == '<'))
			{
				// easy, just find the type, it's either a full name or a name we can find in the included namespaces
				var correspondance = TypeCorrespondances.FirstOrDefault(x => x.Value.Contains(typeName));
				if(correspondance.Key != null)
					typeName = correspondance.Key;
				type = Type.GetType(typeName) ?? IncludedNamespaces.Select(ns => Type.GetType($"{ns}.{typeName}")).FirstOrDefault(t => t != null);

				if (type?.IsGenericType == true)
				{
					type = null;
					return "Not all generics are provided for type:" + typeName;
				}
				return type == null ? $"Type {typeName} not found" : null;
			}

			// we have a generic type, we need to find the base type and the generic arguments
			var name = typeName[..typeName.IndexOf('<')];
			var genericArgs = typeName[(typeName.IndexOf('<') + 1)..^1].Split(',').Select(s => s.Trim()).ToArray();

			// find the base type
			var correspondance2 = TypeCorrespondances.FirstOrDefault(x => x.Value.Contains(name));
			if (correspondance2.Key != null)
				name = correspondance2.Key;

			var baseType = Type.GetType(name + "`" + genericArgs.Length) ?? IncludedNamespaces.Select(ns => Type.GetType($"{ns}.{name}`{genericArgs.Length}")).FirstOrDefault(t => t != null);
			if(baseType == null)
			{
				type = null;
				return $"Type {name} not found";
			}

			// find the generic arguments
			var genericArgsTypes = new Type[genericArgs.Length];
			for(int i = 0; i < genericArgs.Length; i++)
			{
				var error = CreateBaseFromUserInput(genericArgs[i], out var genericArgType);
				if(error != null)
				{
					type = null;
					return error;
				}
				genericArgsTypes[i] = genericArgType!;
			}

			// create the generic type
			type = baseType.MakeGenericType(genericArgsTypes);

			if(type.ContainsGenericParameters)
			{
				type = null;
				return "Not all generics are provided for type:" + name;
			}
			return null;
		}
	}
}
