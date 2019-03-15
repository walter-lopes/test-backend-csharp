namespace Easynvest.Infohub.Parse.Application.Query.Dtos
{
    public class BondParseDto
    {
        public string BondType { get; set; }
        public string BondIndex { get; set; }
        public string IsAntecipatedSell { get; set; }
        public decimal IdCustodyManagerBond { get; set; }
    }
}
