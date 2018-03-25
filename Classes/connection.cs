using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;


class connnection
{
    private string constr;
    public connnection()
    {
        constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
    }
    public string DbConnectionString
    {
        get
        {
            return constr;
        }
    }
}
