using UnityEngine;
//
//  VectorLabelsAttribute.cs
//  VectorLabels
//
//  Created by Hellium on 21/11/18.
//  https://answers.unity.com/questions/1573537/how-to-change-the-names-of-a-vector-3-that-is-set.html
//

public class VectorLabelsAttribute : PropertyAttribute
{
	public readonly string[] Labels;

	public VectorLabelsAttribute(params string[] labels)
	{
		Labels = labels;
	}
}