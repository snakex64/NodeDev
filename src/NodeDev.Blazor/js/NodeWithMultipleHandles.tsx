import React, { memo } from 'react';
import { Handle, Position } from 'reactflow';

import * as Types from './Types'

interface NodeWithMultipleHandlesProps {
	data: Types.NodeData;
}
export default memo(({ data }: NodeWithMultipleHandlesProps) => {

	function getConnection(inputOrOutput: Types.NodeCreationInfo_Connection, type: 'source' | 'target') {


		return <div className={'nodeConnection_' + type} >
			<div style={{ paddingRight: 10, paddingLeft: 10 }}>
				{inputOrOutput.name}
			</div>
			<Handle
				type={type as any}
				position={type == 'source' ? Position.Right : Position.Left}
				id={inputOrOutput.id}
			/>
		</div>
	}


	return (
		<>
			<div>
				{data.name}
			</div>

			{data.inputs.map(x => getConnection(x, 'target'))}

			{data.outputs.map(x => getConnection(x, 'source'))}
		</>
	);
});