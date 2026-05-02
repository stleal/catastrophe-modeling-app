public class Cell
{
  public int Latitude { get; set; }
  public int Longitude { get; set; }

  public Cell()
  {
  }

  public Cell(int latitude, int longitude)
  {
    Latitude = latitude;
    Longitude = longitude;
  }
}