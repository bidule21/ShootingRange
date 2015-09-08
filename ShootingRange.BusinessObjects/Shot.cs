namespace ShootingRange.BusinessObjects
{
  public class Shot
  {
    public decimal PrimaryScore { get; set; }
    public decimal? SecondaryScore { get; set; }
    public int Ordinal { get; set; }
    public decimal? ValueX { get; set; }
    public decimal? ValueY { get; set; }
    public int LaneNumber { get; set; }
    public int ShotId { get; set; }
    public int SubtotalId { get; set; }
  }
}
