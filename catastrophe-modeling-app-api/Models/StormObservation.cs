using System.Globalization;

public class StormObservation
{
  public DateTime YYYYMMDD { get; set; }
  public DateTime HHMM { get; set; }
  public char RecordID { get; set; }
  public string Status { get; set; }
  public double Latitude { get; set; }
  public double Longitude { get; set; }
  public double MaxWind { get; set; }
  public int MinPressure { get; set; }
  public int WindRadius34ktNE { get; set; }
  public int WindRadius34ktSE { get; set; }
  public int WindRadius34ktSW { get; set; }
  public int WindRadius34ktNW { get; set; }
  public int WindRadius50ktNE { get; set; }
  public int WindRadius50ktSE { get; set; }
  public int WindRadius50ktSW { get; set; }
  public int WindRadius50ktNW { get; set; }
  public int WindRadius64ktNE { get; set; }
  public int WindRadius64ktSE { get; set; }
  public int WindRadius64ktSW { get; set; }
  public int WindRadius64ktNW { get; set; }

  public StormObservation()
  {
  }

  public StormObservation(string line)
  {
    string[] storm  = line.Split(",");
    YYYYMMDD = DateTime.ParseExact(storm[0], "yyyyMMdd", CultureInfo.InvariantCulture);
    HHMM = DateTime.ParseExact(storm[1].Trim(), "HHmm", CultureInfo.InvariantCulture);
    RecordID = storm[2].Trim().Equals("") ? ' ' : char.Parse(storm[2].Trim());
    Status = storm[3];
    Latitude = Convert.ToDouble(storm[4].Trim().Substring(0, storm[4].Trim().Length-2));
    Longitude = Convert.ToDouble(storm[5].Trim().Substring(0, storm[4].Trim().Length-2));
    MaxWind = double.Parse(storm[6]);
    MinPressure = int.Parse(storm[7]);
    WindRadius34ktNE = int.Parse(storm[8]);
    WindRadius34ktSE = int.Parse(storm[9]);
    WindRadius34ktSW = int.Parse(storm[10]);
    WindRadius34ktNW = int.Parse(storm[11]);
    WindRadius50ktNE = int.Parse(storm[12]);
    WindRadius50ktSE = int.Parse(storm[13]);
    WindRadius50ktSW = int.Parse(storm[14]);
    WindRadius50ktNW = int.Parse(storm[15]);
    WindRadius64ktNE = int.Parse(storm[16]);
    WindRadius64ktSE = int.Parse(storm[17]);
    WindRadius64ktSW = int.Parse(storm[18]);
    WindRadius64ktNW = int.Parse(storm[19]);
  }
}