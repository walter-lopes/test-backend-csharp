namespace Easynvest.Infohub.Parse.Application.Command.Dtos
{
    public class BondParseDto
    {
        public string BondType { get; set; }
        public string BondIndex { get; set; }
        public string IsAntecipatedSell { get; set; }
        public int IdCustodyManagerBond { get; set; }
    }
}
