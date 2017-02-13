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
	/// Description of Manager.
	/// </summary>
	public class Management : PrebuiltBase
	{
		public Management(EventHandler<string> onMessage, string commandText) : base(onMessage, commandText)
        {
        }
		
		public void VoiceOn()
		{
			OnMessage(this, "Voice is on");
			ContextState.Instance.VoiceEnabled = true;
		}
		
		public void VoiceOff()
		{
			OnMessage(this, "Voice is off");
			ContextState.Instance.VoiceEnabled = false;
		}
	}
}
