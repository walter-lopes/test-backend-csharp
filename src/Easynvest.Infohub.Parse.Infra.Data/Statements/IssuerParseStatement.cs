using System;
using System.Collections.Generic;
using System.Text;

namespace Easynvest.Infohub.Parse.Infra.Data.Statements
{
    internal static class IssuerParseStatement
    {
        internal const string GetAll = @"SELECT IssuerNameCetip, IssuerNameCustodyManager FROM API_TICKETMANAGER.ISSUER_PARSE";

        internal const string GetBy = @"SELECT IssuerNameCetip, IssuerNameCustodyManager FROM API_TICKETMANAGER.ISSUER_PARSE 
                                            WHERE ISSUERNAMECETIP = UPPER(:issuerNameCetip)";

        internal const string Create = @"INSERT INTO API_TICKETMANAGER.ISSUER_PARSE (ISSUERNAMECETIP, ISSUERNAMECUSTODYMANAGER) 
                                                    VALUES(:issuerNameCetip, :issuerNameCustodyManager)";

        internal const string Update = @"UPDATE API_TICKETMANAGER.ISSUER_PARSE SET ISSUERNAMECUSTODYMANAGER= :issuerNameCustodyManager 
                                                    WHERE ISSUERNAMECETIP= :issuerNameCetip";

        internal const string Delete = @"DELETE FROM API_TICKETMANAGER.ISSUER_PARSE WHERE ISSUERNAMECETIP= :issuerNameCetip";
    }
}
