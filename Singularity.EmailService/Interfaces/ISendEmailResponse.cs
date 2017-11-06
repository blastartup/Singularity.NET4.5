using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace

namespace Singularity.EmailService
{
	public interface ISendEmailResponse
	{
		SendEmailCode StatusCode { get; set; }
	}
}
