using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Zegogo
{
	public class showLabel:Label
	{
		public void showMore()
		{
			MaxLines = -1;
		}
	}
}
