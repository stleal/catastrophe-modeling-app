public class ProcessService : IProcessService
{
  private static IConfiguration? _configuration { get; set; }

  public ProcessService(IConfiguration configuration)
  {
    _configuration = configuration;
  }

  public List<StormInfo> ProcessHURDAT2(IFormFile file)
  {
    List<StormObject> stormObjects = CreateStormObjects(file).Result.Where(s => s.StormYear.Year >= 1900).ToList();
    Grid grid = new Grid(GenerateFloridaCells());
    List<StormInfo> stormInfoList = FilterOnFlorida(stormObjects, grid);
    return stormInfoList;
  }

  public List<Cell> GenerateFloridaCells()
  {
    int[] latitudes = _configuration!.GetSection("CatastropheModelingApp:FloridaLatitudeRangeArray").Get<int[]>()!;
    int[] longitudes = _configuration.GetSection("CatastropheModelingApp:FloridaLongitudeRangeArray").Get<int[]>()!;
    List<Cell> cells = new List<Cell>();
    foreach (var latitude in latitudes)
    {
        for (int i = 0; i < longitudes.Length; i++)
        {
            Cell cell = new Cell(latitude, longitudes[i]);
            cells.Add(cell);
        }
    }
    return cells;
  }

  private static bool IsWaterCell(StormObservation observation, Grid grid)
  {
    if (observation.Latitude >= 24 && observation.Latitude <= 29 &&
        observation.Longitude >= 84 && observation.Longitude <= 88)
        return true;
    return false;
  }

  public static List<StormInfo> FilterOnFlorida(List<StormObject> stormObjects, Grid grid)
  {
    List<StormInfo> stormInfoList = new List<StormInfo>();
    List<StormObject> stormsThatHitFlorida = new List<StormObject>();
    foreach (var storm in stormObjects)
    {
      foreach (var observation in storm.StormObservations)
      {
        if (!IsWaterCell(observation, grid))
        {
          foreach (var cell in grid.GetCells())
          {
            if (observation.Latitude >= cell.Latitude && observation.Latitude <= cell.Latitude + 1 &&
              observation.Longitude <= cell.Longitude && observation.Longitude >= cell.Longitude - 1)
            {
              if (!stormsThatHitFlorida.Contains(storm))
              {
                stormsThatHitFlorida.Add(storm);
                StormInfo stormInfo = new StormInfo
                {
                  StormObject = storm,
                  StormName = storm.StormHeader.NameOfStorm,
                  DateOfLandfall = observation.YYYYMMDD,
                  MaxWind = storm.StormObservations.Max(s => s.MaxWind)
                };
                stormInfoList.Add(stormInfo);
              }
              break;
            }
          }
        }
      }
    }
    return stormInfoList;
  }

  private static async Task<List<StormObject>> CreateStormObjects(IFormFile file)
  {
    var index = 0;
    List<StormObject> stormObjects = new List<StormObject>();
    try
    {
      string[] lines = new string[0];
      using (var reader = new StreamReader(file.OpenReadStream()))
      {
        List<string> lineList = new List<string>();
        var line = string.Empty;
        while ((line = await reader.ReadLineAsync()) != null)
        {
          lineList.Add(line);
        }
        lines = lineList.ToArray();
      }
      for (int i = index; i < lines.Length; i++)
      {
        // get the next header and observations
        StormHeader stormHeader = new StormHeader(lines[index].Split(","));
        List<StormObservation> stormObservations = new List<StormObservation>();
        for (int j = index+1; j <= i + stormHeader.NumberOfObservations; j++)
        {
          stormObservations.Add(new StormObservation(lines[j]));
          index++;
        }
        stormObjects.Add(new StormObject(stormHeader, stormObservations));
        i = index;
        index+=1;
      }
      return stormObjects;
    }
    catch (IOException ex)
    {
      throw new Exception("Error reading file", ex);
    }
  }

}