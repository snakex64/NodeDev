﻿using NodeDev.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeDev.Core.Nodes.Flow
{
	public class EntryNode : Node
	{
		public EntryNode(Graph graph) : base(graph)
		{
			Name = "Entry";

			Outputs.Add(new("Exec", this, TypeFactory.ExecType));
		}
	}
}