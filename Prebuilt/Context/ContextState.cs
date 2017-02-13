/*
 * Created by SharpDevelop.
 * User: Alexander
 * Date: 2/11/2017
 * Time: 12:09 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Prebuilt.Context
{
	/// <summary>
	/// Description of State.
	/// </summary>
	public class ContextState
	{
		public static readonly ContextState Instance = new ContextState();
		
		public ContextState()
		{
			VoiceEnabled = true;
		}
		
		public bool VoiceEnabled {get; set;}
	}
}
