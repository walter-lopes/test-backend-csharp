namespace Easynvest.Infohub.Parse.Infra.Data.Statements
{
    internal static class BondParseStatement
    {
        internal const string GetAll = "SELECT bondType, bondIndex, isAntecipatedSell, idCustodyManagerBond FROM API_TICKETMANAGER.BOND_PARSE";

        internal const string GetBy = @"SELECT bondType, bondIndex, isAntecipatedSell, idCustodyManagerBond FROM API_TICKETMANAGER.BOND_PARSE
                                          WHERE BONDTYPE= UPPER(:bondType) AND BONDINDEX= UPPER(:bondIndex) AND ISANTECIPATEDSELL= UPPER(:isAntecipatedSell)";

        internal const string Create = @"INSERT INTO API_TICKETMANAGER.BOND_PARSE  (BONDTYPE, BONDINDEX, ISANTECIPATEDSELL, IDCUSTODYMANAGERBOND)
                                       VALUES(:bondType, :bondIndex, :isAntecipatedSell, :idCustodyManagerBond)";

        internal const string Update = @"UPDATE API_TICKETMANAGER.BOND_PARSE SET IDCUSTODYMANAGERBOND= :idCustodyManagerBond
                                       WHERE BONDTYPE= :bondType AND BONDINDEX= :bondIndex AND ISANTECIPATEDSELL= :IsAntecipatedSell";

        internal const string Delete = @"DELETE FROM API_TICKETMANAGER.BOND_PARSE WHERE BONDTYPE= :bondType AND BONDINDEX= :bondIndex
                                                AND ISANTECIPATEDSELL= :isAntecipatedSell";
    }
}
