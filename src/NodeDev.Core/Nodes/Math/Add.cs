﻿using NodeDev.Core.Connections;
using NodeDev.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeDev.Core.Nodes.Math
{
	public class Add: TwoOperationMath
	{
		protected override string OperatorName => "Addition";

		public Add(Graph graph, string? id = null) : base(graph, id)
		{
			Name = "Add";
		}

		protected override void ExecuteInternal(GraphExecutor graphExecutor, object? self, Span<object?> inputs, Span<object?> outputs)
		{
			dynamic? a = inputs[0];
			dynamic? b = inputs[1];

			outputs[0] = a + b;
        }
    }
}
